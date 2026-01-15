using System.Net;
using System.Net.Sockets;
using MessagePack;
using Microsoft.Extensions.Logging;
using NoDaysOff.Shared.Messages;

namespace NoDaysOff.Shared.Messaging;

/// <summary>
/// UDP-based message bus for local development
/// </summary>
public class UdpMessageBus : IMessageBus, IDisposable
{
    private readonly UdpClient _sender;
    private readonly Dictionary<string, UdpClient> _receivers;
    private readonly Dictionary<string, CancellationTokenSource> _listenerCancellations;
    private readonly ILogger<UdpMessageBus> _logger;
    private readonly int _basePort;
    private bool _disposed;

    public UdpMessageBus(ILogger<UdpMessageBus> logger, int basePort = 5000)
    {
        _logger = logger;
        _basePort = basePort;
        _sender = new UdpClient();
        _receivers = new Dictionary<string, UdpClient>();
        _listenerCancellations = new Dictionary<string, CancellationTokenSource>();
    }

    public async Task PublishAsync<T>(T message, string topic, CancellationToken cancellationToken = default) where T : IMessage
    {
        try
        {
            var data = MessagePackSerializer.Serialize(message);
            var port = GetPortForTopic(topic);
            await _sender.SendAsync(data, data.Length, new IPEndPoint(IPAddress.Loopback, port));
            _logger.LogInformation("Published message {MessageId} to topic {Topic} on port {Port}", 
                message.MessageId, topic, port);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing message to topic {Topic}", topic);
            throw;
        }
    }

    public Task SubscribeAsync<T>(string topic, Func<T, Task> handler, CancellationToken cancellationToken = default) where T : IMessage
    {
        var port = GetPortForTopic(topic);
        
        if (_receivers.ContainsKey(topic))
        {
            _logger.LogWarning("Already subscribed to topic {Topic}", topic);
            return Task.CompletedTask;
        }

        var receiver = new UdpClient(port);
        _receivers[topic] = receiver;

        var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        _listenerCancellations[topic] = cts;

        _ = Task.Run(async () =>
        {
            _logger.LogInformation("Started listening on topic {Topic} at port {Port}", topic, port);
            
            while (!cts.Token.IsCancellationRequested)
            {
                try
                {
                    var result = await receiver.ReceiveAsync(cts.Token);
                    var message = MessagePackSerializer.Deserialize<T>(result.Buffer);
                    
                    _logger.LogInformation("Received message {MessageId} on topic {Topic}", 
                        message.MessageId, topic);
                    
                    await handler(message);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message on topic {Topic}", topic);
                }
            }
            
            _logger.LogInformation("Stopped listening on topic {Topic}", topic);
        }, cts.Token);

        return Task.CompletedTask;
    }

    public Task UnsubscribeAsync(string topic)
    {
        if (_listenerCancellations.TryGetValue(topic, out var cts))
        {
            cts.Cancel();
            _listenerCancellations.Remove(topic);
        }

        if (_receivers.TryGetValue(topic, out var receiver))
        {
            receiver.Close();
            receiver.Dispose();
            _receivers.Remove(topic);
        }

        _logger.LogInformation("Unsubscribed from topic {Topic}", topic);
        return Task.CompletedTask;
    }

    private int GetPortForTopic(string topic)
    {
        var hash = topic.GetHashCode();
        return _basePort + Math.Abs(hash % 1000);
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        foreach (var cts in _listenerCancellations.Values)
        {
            cts.Cancel();
            cts.Dispose();
        }

        foreach (var receiver in _receivers.Values)
        {
            receiver.Close();
            receiver.Dispose();
        }

        _sender.Close();
        _sender.Dispose();

        _disposed = true;
    }
}
