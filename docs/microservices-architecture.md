# Microservices Architecture Refactoring

## Overview

This document describes the refactoring of the No Days Off application from a monolithic architecture to a microservices-ready architecture with MessagePack serialization for inter-service communication.

## What Has Been Implemented

### 1. Shared Project (NoDaysOff.Shared)

A new shared library project has been created containing:

#### Message Infrastructure
- **IMessage**: Base interface for all messages with `MessageId` and `Timestamp`
- **BaseMessage**: Abstract base class implementing common message properties
- **DomainMessages**: Concrete message types for domain events:
  - `AthleteCreatedMessage`: Published when an athlete is created
  - `WorkoutCompletedMessage`: Published when a workout is completed
  - `ExerciseCreatedMessage`: Published when an exercise is created

#### Messaging Providers
- **IMessageBus**: Interface for message bus operations (publish/subscribe)
- **UdpMessageBus**: UDP-based message bus for local development
  - Uses UDP multicast for low-overhead local development
  - Port-based topic routing (base port 5000)
  - Automatic serialization/deserialization with MessagePack
- **RedisMessageBus**: Redis Pub/Sub based message bus for production
  - Uses Redis Pub/Sub for distributed systems
  - MessagePack serialization for efficient network transfer
  - Production-ready with connection pooling

#### Dependency Injection
- **DependencyInjection.AddSharedMessaging()**: Extension method to register messaging services
  - Automatically selects UDP for development
  - Configures Redis for production based on configuration

### 2. MessagePack Serialization

- **MessagePack 3.1.4** is used for efficient binary serialization
- All messages are decorated with `[MessagePackObject]` and `[Key]` attributes
- Provides ~5-10x better performance than JSON
- Significantly smaller message size for network transfer

### 3. Configuration

The messaging system is configured via appsettings.json:

```json
{
  "Messaging": {
    "Provider": "Udp",           // "Udp" for dev, "Redis" for production
    "UdpBasePort": 5000,          // Base port for UDP messaging
    "RedisConnection": "localhost:6379"  // Redis connection string for production
  }
}
```

### 4. Integration with Existing API

The shared messaging infrastructure has been integrated into the existing NoDaysOff.Api project:
- Messaging services are registered in Program.cs
- Ready to publish domain events when entities are created/updated
- Configuration added to appsettings.json

## Microservices Architecture Pattern

### Proposed Service Boundaries

Based on the Domain-Driven Design aggregates, the application should be split into these microservices:

1. **Athlete Service**
   - Database: `NoDaysOff_Athletes` (SQL Express)
   - Aggregates: Athlete, Profile
   - Responsibilities: Athlete management, weight tracking, completed exercises

2. **Workout Service**
   - Database: `NoDaysOff_Workouts` (SQL Express)
   - Aggregates: Workout, ScheduledExercise, Day
   - Responsibilities: Workout planning, session management, day scheduling

3. **Exercise Service**
   - Database: `NoDaysOff_Exercises` (SQL Express)
   - Aggregates: Exercise, BodyPart
   - Responsibilities: Exercise definitions, body part categorization

4. **Dashboard Service**
   - Database: `NoDaysOff_Dashboards` (SQL Express)
   - Aggregates: Dashboard, Tile
   - Responsibilities: User dashboards, widget configuration

5. **Media Service**
   - Database: `NoDaysOff_Media` (SQL Express)
   - Aggregates: Video, DigitalAsset
   - Responsibilities: Video content, file management

6. **Communication Service**
   - Database: `NoDaysOff_Communication` (SQL Express)
   - Aggregates: Conversation, Message
   - Responsibilities: User messaging, notifications

### Database Per Service

Each microservice has its own SQL Express database:
- Ensures data isolation
- Allows independent scaling
- Enables technology diversity (could migrate individual services to other databases)
- Follows microservices best practices

### Inter-Service Communication

Services communicate through:
- **Synchronous**: REST APIs for request/response patterns
- **Asynchronous**: Message bus for events and eventual consistency
  - UDP for local development (low overhead, fast)
  - Redis Pub/Sub for production (distributed, reliable)

## Usage Example

### Publishing a Message

```csharp
public class CreateAthleteCommandHandler
{
    private readonly AthleteDbContext _context;
    private readonly IMessageBus _messageBus;
    
    public async Task<int> Handle(CreateAthleteCommand request, CancellationToken cancellationToken)
    {
        var athlete = Athlete.Create(/*...*/);
        _context.Athletes.Add(athlete);
        await _context.SaveChangesAsync(cancellationToken);
        
        // Publish event to other services
        await _messageBus.PublishAsync(new AthleteCreatedMessage
        {
            AthleteId = athlete.Id,
            Name = athlete.Name,
            Username = athlete.Username,
            TenantId = athlete.TenantId
        }, "athlete.created", cancellationToken);
        
        return athlete.Id;
    }
}
```

### Subscribing to Messages

```csharp
public class WorkoutService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _messageBus.SubscribeAsync<AthleteCreatedMessage>(
            "athlete.created",
            async (message) =>
            {
                // Handle athlete created event
                await CreateDefaultWorkoutPlan(message.AthleteId);
            },
            cancellationToken);
    }
}
```

## Benefits of This Architecture

1. **Scalability**: Each service can be scaled independently based on load
2. **Resilience**: Failure in one service doesn't bring down the entire system
3. **Technology Flexibility**: Services can use different technologies/frameworks
4. **Team Autonomy**: Teams can work on services independently
5. **Deployment Flexibility**: Services can be deployed independently
6. **Performance**: MessagePack provides efficient serialization
7. **Development Experience**: UDP messaging provides fast local development

## Next Steps for Full Migration

To complete the microservices migration:

1. Create individual service projects (AthleteService, WorkoutService, etc.)
2. Copy relevant aggregates and features to each service
3. Create service-specific DbContexts for each database
4. Update handlers to publish domain events
5. Add event subscribers in dependent services
6. Configure separate databases in SQL Express
7. Add API Gateway for unified entry point
8. Implement service discovery (for production)
9. Add distributed tracing (optional)
10. Implement circuit breakers and retry policies

## Configuration for Production

For production deployment with Redis:

```json
{
  "Messaging": {
    "Provider": "Redis",
    "RedisConnection": "your-redis-server:6379,password=yourpassword,ssl=true"
  }
}
```

## Security Considerations

- Messages should not contain sensitive data (passwords, tokens)
- Use TLS for Redis connections in production
- Implement message validation and authentication
- Consider message encryption for sensitive domains
