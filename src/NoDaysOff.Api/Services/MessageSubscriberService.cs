using NoDaysOff.Shared.Messages;
using NoDaysOff.Shared.Messaging;

namespace NoDaysOff.Api.Services;

/// <summary>
/// Example background service that demonstrates subscribing to messages from other microservices.
/// In a full microservices architecture, this would be in a separate service project.
/// </summary>
public class MessageSubscriberService : BackgroundService
{
    private readonly IMessageBus _messageBus;
    private readonly ILogger<MessageSubscriberService> _logger;

    public MessageSubscriberService(IMessageBus messageBus, ILogger<MessageSubscriberService> logger)
    {
        _messageBus = messageBus;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Subscribe to athlete created events
        await _messageBus.SubscribeAsync<AthleteCreatedMessage>(
            "athlete.created",
            async (message) =>
            {
                _logger.LogInformation(
                    "Received AthleteCreated event: Id={AthleteId}, Name={Name}, Username={Username}",
                    message.AthleteId, message.Name, message.Username);
                
                // In a real microservice, this could trigger:
                // - Creating a default workout plan
                // - Setting up a dashboard
                // - Sending a welcome email
                // - etc.
            },
            stoppingToken);

        // Subscribe to workout completed events
        await _messageBus.SubscribeAsync<WorkoutCompletedMessage>(
            "workout.completed",
            async (message) =>
            {
                _logger.LogInformation(
                    "Received WorkoutCompleted event: WorkoutId={WorkoutId}, AthleteId={AthleteId}",
                    message.WorkoutId, message.AthleteId);
                
                // In a real microservice, this could trigger:
                // - Updating athlete statistics
                // - Calculating achievements
                // - Updating dashboard widgets
                // - etc.
            },
            stoppingToken);

        _logger.LogInformation("Message subscriber service started and listening for events");

        // Keep the service running
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Message subscriber service stopping");
        await _messageBus.UnsubscribeAsync("athlete.created");
        await _messageBus.UnsubscribeAsync("workout.completed");
        await base.StopAsync(cancellationToken);
    }
}
