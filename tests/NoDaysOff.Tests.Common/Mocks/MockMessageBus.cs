using Shared.Messaging;

namespace NoDaysOff.Tests.Common.Mocks;

public class MockMessageBus : IMessageBus
{
    public List<(string Topic, object Message)> PublishedMessages { get; } = new();
    public Dictionary<string, List<object>> Subscriptions { get; } = new();

    public Task PublishAsync<T>(string topic, T message, CancellationToken cancellationToken = default) where T : class
    {
        PublishedMessages.Add((topic, message!));
        return Task.CompletedTask;
    }

    public Task SubscribeAsync<T>(string topic, Func<T, CancellationToken, Task> handler, CancellationToken cancellationToken = default) where T : class
    {
        if (!Subscriptions.ContainsKey(topic))
            Subscriptions[topic] = new List<object>();
        Subscriptions[topic].Add(handler);
        return Task.CompletedTask;
    }

    public void Clear()
    {
        PublishedMessages.Clear();
        Subscriptions.Clear();
    }
}
