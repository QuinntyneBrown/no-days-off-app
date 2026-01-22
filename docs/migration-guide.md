# Microservices Migration Guide

This guide explains how the No Days Off application has been refactored from a monolithic architecture to microservices.

## Migration Overview

The application has been transformed from a single monolithic API into 6 independent microservices, each with its own database and deployment lifecycle.

## What Changed

### Before (Monolithic)
```
NoDaysOff.Api (Single Application)
├── Features/Athletes
├── Features/Workouts
├── Features/Exercises
├── Features/Dashboards
├── Features/Videos
├── Features/Conversations
└── Single Database (NoDaysOff)
```

### After (Microservices)
```
6 Independent Services:
├── NoDaysOff.Services.Athletes → NoDaysOff_Athletes DB
├── NoDaysOff.Services.Workouts → NoDaysOff_Workouts DB
├── NoDaysOff.Services.Exercises → NoDaysOff_Exercises DB
├── NoDaysOff.Services.Dashboards → NoDaysOff_Dashboards DB
├── NoDaysOff.Services.Media → NoDaysOff_Media DB
└── NoDaysOff.Services.Communication → NoDaysOff_Communication DB
```

## Architecture Decisions

### 1. Service Boundaries

Services were defined based on **Domain-Driven Design (DDD) bounded contexts**:

| Service | Bounded Contexts | Aggregates |
|---------|------------------|------------|
| Athletes | Athletes, Profiles | Athlete, Profile |
| Workouts | Workouts, Days, Scheduled Exercises | Workout, Day, ScheduledExercise |
| Exercises | Exercises, Body Parts | Exercise, BodyPart |
| Dashboards | Dashboards, Tiles | Dashboard, Tile |
| Media | Videos, Digital Assets | Video, DigitalAsset |
| Communication | Conversations | Conversation, Message |

### 2. Database Per Service

Each service has its own isolated database:
- **Benefit**: True service independence, separate scaling, technology freedom
- **Challenge**: No foreign keys across services
- **Solution**: Event-driven consistency, service-to-service APIs

### 3. Communication Patterns

**Synchronous (HTTP/REST):**
- For immediate consistency requirements
- Direct service-to-service calls
- Example: Validating exercise exists when creating scheduled exercise

**Asynchronous (Message Bus):**
- For eventual consistency
- Loosely coupled communication
- Example: Updating dashboard when athlete completes workout

### 4. Shared Infrastructure

**Shared Projects:**
- `NoDaysOff.Core` - Domain models (shared across services)
- `NoDaysOff.Infrastructure` - Data access (shared DbContext base)
- `NoDaysOff.Shared` - Messaging infrastructure

**Why Shared?**
- Reuse domain logic and entities
- Consistent data access patterns
- Centralized messaging infrastructure
- **Note**: In a pure microservices approach, these would be duplicated or packaged separately

## Implementation Details

### Athletes Service (Example)

The Athletes Service was fully implemented as a reference example:

**Structure:**
```
NoDaysOff.Services.Athletes/
├── Controllers/
│   └── AthletesController.cs
├── Features/
│   └── Athletes/
│       ├── AthleteDto.cs
│       ├── CreateAthleteCommand.cs
│       ├── CreateAthleteCommandHandler.cs
│       ├── GetAthletesQuery.cs
│       ├── GetAthletesQueryHandler.cs
│       └── ...
├── Program.cs
├── appsettings.json
└── Dockerfile
```

**Configuration:**
- **Port**: 5100 (HTTP), 7100 (HTTPS)
- **Database**: NoDaysOff_Athletes
- **Messaging**: UDP port 5001 (dev), Redis (prod)

### Other Services

The remaining services (Workouts, Exercises, Dashboards, Media, Communication) have been scaffolded with:
- Project structure created
- Basic Web API template
- References to Core, Infrastructure, and Shared projects
- Configuration files
- Added to solution

**Next Steps for Full Migration:**
1. Copy relevant features from NoDaysOff.Api
2. Configure unique ports and databases
3. Add Swagger documentation
4. Implement event publishers and subscribers

## Deployment Changes

### Local Development

**Before:**
```bash
dotnet run --project src/NoDaysOff.Api
```

**After:**
```bash
# Option 1: Run all services with Docker Compose
docker-compose up -d

# Option 2: Run individual services
dotnet run --project src/NoDaysOff.Services.Athletes
dotnet run --project src/NoDaysOff.Services.Workouts
# ...
```

### Docker Compose

A complete `docker-compose.yml` orchestrates:
- 6 microservices
- SQL Server (shared container with multiple databases)
- Redis (for production messaging)

### Port Assignments

| Service | HTTP | HTTPS | UDP Port |
|---------|------|-------|----------|
| Athletes | 5100 | 7100 | 5001 |
| Workouts | 5101 | 7101 | 5002 |
| Exercises | 5102 | 7102 | 5003 |
| Dashboards | 5103 | 7103 | 5004 |
| Media | 5104 | 7104 | 5005 |
| Communication | 5105 | 7105 | 5006 |

## Data Migration

### Database Splitting

When fully migrated, data from the monolithic `NoDaysOff` database needs to be split:

```sql
-- Example: Migrate Athletes data
INSERT INTO NoDaysOff_Athletes.dbo.Athletes
SELECT * FROM NoDaysOff.dbo.Athletes;

INSERT INTO NoDaysOff_Athletes.dbo.Profiles
SELECT * FROM NoDaysOff.dbo.Profiles;

-- Example: Migrate Workouts data
INSERT INTO NoDaysOff_Workouts.dbo.Workouts
SELECT * FROM NoDaysOff.dbo.Workouts;
```

### Handling Cross-Service References

**Problem:** Foreign keys can't span databases

**Solutions:**

1. **Store IDs, Validate via API:**
```csharp
// In Workout Service
var exerciseExists = await _exerciseServiceClient.ExerciseExistsAsync(exerciseId);
if (!exerciseExists)
    throw new InvalidOperationException("Exercise not found");
```

2. **Cache Reference Data:**
```csharp
// Cache exercise names locally in Workout Service
public class ExerciseReference
{
    public int ExerciseId { get; set; }
    public string Name { get; set; } // Cached from Exercise Service
}
```

3. **Event-Driven Synchronization:**
```csharp
// When exercise is created in Exercise Service
await _messageBus.PublishAsync(new ExerciseCreatedMessage 
{ 
    ExerciseId = exercise.Id,
    Name = exercise.Name 
});

// Workout Service subscribes and caches
await _messageBus.SubscribeAsync<ExerciseCreatedMessage>("exercise.created", 
    (msg) => _cache.Set(msg.ExerciseId, msg.Name));
```

## Messaging Implementation

### Publishing Events

Example from Athletes Service:
```csharp
public class CreateAthleteCommandHandler
{
    private readonly NoDaysOffDbContext _context;
    private readonly IMessageBus _messageBus;

    public async Task<int> Handle(CreateAthleteCommand request, CancellationToken ct)
    {
        var athlete = Athlete.Create(/*...*/);
        _context.Athletes.Add(athlete);
        await _context.SaveChangesAsync(ct);

        // Publish event
        await _messageBus.PublishAsync(new AthleteCreatedMessage
        {
            AthleteId = athlete.Id,
            Name = athlete.Name,
            Username = athlete.Username,
            TenantId = athlete.TenantId
        }, "athlete.created", ct);

        return athlete.Id;
    }
}
```

### Subscribing to Events

Example in Dashboard Service:
```csharp
public class AthleteEventSubscriber : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _messageBus.SubscribeAsync<AthleteCreatedMessage>(
            "athlete.created",
            async (message) =>
            {
                // Create default dashboard for new athlete
                var dashboard = Dashboard.Create(message.AthleteId, /*...*/);
                _context.Dashboards.Add(dashboard);
                await _context.SaveChangesAsync();
            },
            stoppingToken);
    }
}
```

## Testing Strategy

### Unit Tests
- Test domain logic in isolation
- No changes needed (domain models unchanged)

### Integration Tests
- Test each service's API endpoints
- Mock external service dependencies
- Test message publishing

### End-to-End Tests
- Test flows across multiple services
- Use test containers for SQL Server and Redis
- Verify event-driven workflows

## Rollback Strategy

### Phase 1: Parallel Run
- Keep monolithic API running
- Run new microservices in parallel
- Route small percentage of traffic to microservices
- Easy rollback: just route back to monolith

### Phase 2: Gradual Migration
- Migrate one service at a time
- Proxy requests from monolith to microservice
- Monitor and verify
- Rollback individual services if needed

### Phase 3: Full Migration
- Decommission monolithic API
- All traffic goes to microservices
- Rollback requires restoring monolith

## Monitoring and Observability

### What to Monitor

1. **Service Health**
   - Endpoint availability
   - Response times
   - Error rates

2. **Inter-Service Communication**
   - API call success/failure rates
   - Message publishing/consuming rates
   - Message processing latency

3. **Database Performance**
   - Query performance per service
   - Connection pool usage
   - Database size growth

### Recommended Tools

- **Application Insights** / **Prometheus** - Metrics
- **ELK Stack** / **Seq** - Centralized logging
- **OpenTelemetry** - Distributed tracing
- **Grafana** - Dashboards

## Benefits Realized

1. **Independent Deployment**
   - Deploy Athletes Service without affecting Workouts Service
   - Faster release cycles

2. **Independent Scaling**
   - Scale Athletes Service separately if it has higher load
   - Cost optimization

3. **Technology Freedom**
   - Could migrate Media Service to different storage (e.g., S3)
   - Experiment with different ORMs per service

4. **Team Autonomy**
   - Different teams can own different services
   - Clear boundaries and ownership

5. **Fault Isolation**
   - If Dashboards Service fails, Athletes Service keeps working
   - Better resilience

## Challenges and Mitigations

### Challenge: Distributed Transactions
- **Mitigation**: SAGA pattern, eventual consistency, compensating transactions

### Challenge: Data Consistency
- **Mitigation**: Event-driven updates, read models, CQRS

### Challenge: Testing Complexity
- **Mitigation**: Contract testing, service virtualization, test containers

### Challenge: Operational Overhead
- **Mitigation**: Docker Compose (dev), Kubernetes (prod), centralized logging

### Challenge: Network Latency
- **Mitigation**: Caching, async messaging, circuit breakers

## Next Steps

1. **Complete Service Migration**
   - Implement remaining 5 services (Workouts, Exercises, etc.)
   - Copy features from monolithic API

2. **Add API Gateway**
   - Unified entry point (YARP, Ocelot, Kong)
   - Authentication/Authorization
   - Rate limiting

3. **Implement Service Discovery**
   - Consul or Kubernetes DNS
   - Dynamic service resolution

4. **Add Resilience**
   - Circuit breakers (Polly)
   - Retry policies
   - Timeouts

5. **Enhance Observability**
   - Distributed tracing
   - Centralized logging
   - Health checks

6. **Production Deployment**
   - Kubernetes manifests
   - CI/CD pipelines
   - Blue-green deployments

## References

- [Microservices Architecture Documentation](microservices-architecture.md)
- [Deployment Guide](deployment.md)
- [.NET Microservices: Architecture for Containerized .NET Applications](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/)
- [Building Microservices by Sam Newman](https://www.oreilly.com/library/view/building-microservices-2nd/9781492034018/)
