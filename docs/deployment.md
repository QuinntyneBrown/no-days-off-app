# Microservices Deployment Guide

This guide explains how to deploy and run the No Days Off application in a microservices architecture.

## Architecture Overview

The application has been refactored into 6 independent microservices:

| Service | Port (HTTP) | Port (HTTPS) | Database | Description |
|---------|-------------|--------------|----------|-------------|
| Athletes Service | 5100 | 7100 | NoDaysOff_Athletes | Athlete management and profiles |
| Workouts Service | 5101 | 7101 | NoDaysOff_Workouts | Workout planning and scheduling |
| Exercises Service | 5102 | 7102 | NoDaysOff_Exercises | Exercise definitions and body parts |
| Dashboards Service | 5103 | 7103 | NoDaysOff_Dashboards | User dashboards and widgets |
| Media Service | 5104 | 7104 | NoDaysOff_Media | Videos and digital assets |
| Communication Service | 5105 | 7105 | NoDaysOff_Communication | User conversations and messaging |

## Prerequisites

### Local Development
- .NET 9.0 SDK
- SQL Server (LocalDB, Express, or full version)
- Visual Studio 2022 or VS Code
- (Optional) Redis for production messaging

### Docker Deployment
- Docker Desktop or Docker Engine
- Docker Compose

## Running Locally

### Option 1: Run Individual Services

Each service can be run independently:

```bash
# Athletes Service
cd src/NoDaysOff.Services.Athletes
dotnet run

# Workouts Service
cd src/NoDaysOff.Services.Workouts
dotnet run

# Exercises Service
cd src/NoDaysOff.Services.Exercises
dotnet run

# And so on...
```

Access Swagger UI:
- Athletes: https://localhost:7100/swagger
- Workouts: https://localhost:7101/swagger
- Exercises: https://localhost:7102/swagger
- Dashboards: https://localhost:7103/swagger
- Media: https://localhost:7104/swagger
- Communication: https://localhost:7105/swagger

### Option 2: Run All Services with dotnet

Use the solution file to build all services:

```bash
# Build all services
dotnet build

# Run specific service
dotnet run --project src/NoDaysOff.Services.Athletes
```

### Option 3: Run with Visual Studio

1. Open `NoDaysOff.sln` in Visual Studio 2022
2. Set multiple startup projects:
   - Right-click solution → Properties → Startup Project → Multiple
   - Select all `NoDaysOff.Services.*` projects
3. Press F5 to start all services

## Running with Docker Compose

### Build and Start All Services

```bash
# Build all service images
docker-compose build

# Start all services in detached mode
docker-compose up -d

# View logs
docker-compose logs -f

# View specific service logs
docker-compose logs -f athletes-service
```

### Service URLs (Docker)
- Athletes: http://localhost:5100/swagger
- Workouts: http://localhost:5101/swagger
- Exercises: http://localhost:5102/swagger
- Dashboards: http://localhost:5103/swagger
- Media: http://localhost:5104/swagger
- Communication: http://localhost:5105/swagger

### Infrastructure Services
- SQL Server: localhost:1433
- Redis: localhost:6379

### Stop Services

```bash
# Stop all services
docker-compose down

# Stop and remove volumes (clean database)
docker-compose down -v
```

## Configuration

### Database Per Service

Each microservice has its own isolated database:

**Local Development (SQL Express):**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=NoDaysOff_Athletes;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

**Docker:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=sqlserver;Database=NoDaysOff_Athletes;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True"
  }
}
```

### Messaging Configuration

**Development (UDP - Local):**
```json
{
  "Messaging": {
    "Provider": "Udp",
    "UdpBasePort": 5001
  }
}
```

**Production (Redis - Docker):**
```json
{
  "Messaging": {
    "Provider": "Redis",
    "RedisConnection": "redis:6379"
  }
}
```

## Database Migrations

Each service manages its own database schema. To create and apply migrations:

```bash
# Example for Athletes Service
cd src/NoDaysOff.Services.Athletes
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## Inter-Service Communication

### Synchronous Communication
Services can make HTTP calls to each other using service discovery or direct URLs:

```csharp
// Example: Workout Service calling Exercise Service
var exerciseResponse = await _httpClient.GetAsync("http://localhost:5102/api/exercises/1");
```

### Asynchronous Communication
Services communicate via message bus for events:

```csharp
// Publishing an event
await _messageBus.PublishAsync(new AthleteCreatedMessage
{
    AthleteId = athlete.Id,
    Name = athlete.Name
}, "athlete.created", cancellationToken);

// Subscribing to an event
await _messageBus.SubscribeAsync<AthleteCreatedMessage>(
    "athlete.created",
    async (message) => {
        // Handle event
    },
    cancellationToken);
```

## Monitoring and Health Checks

Each service exposes health check endpoints (to be implemented):

```
GET /health
GET /health/ready
GET /health/live
```

## Production Considerations

### Security
1. Use HTTPS for all inter-service communication
2. Implement API Gateway with authentication
3. Use secrets management (Azure Key Vault, AWS Secrets Manager)
4. Enable TLS for Redis connections

### Scalability
1. Deploy services independently based on load
2. Use container orchestration (Kubernetes, Azure Container Apps)
3. Implement auto-scaling policies
4. Use load balancers for service instances

### Resilience
1. Implement circuit breakers (Polly)
2. Add retry policies with exponential backoff
3. Implement distributed tracing (OpenTelemetry)
4. Use service mesh (Istio, Linkerd) for advanced scenarios

### Deployment
1. CI/CD pipelines for each service
2. Blue-green or canary deployments
3. Database migration strategies
4. Rollback procedures

## Troubleshooting

### Service Won't Start
- Check port availability
- Verify database connection strings
- Check logs: `docker-compose logs [service-name]`

### Database Connection Issues
- Ensure SQL Server is running
- Verify connection string
- Check firewall rules
- Wait for SQL Server health check

### Messaging Issues
- Verify Redis is running
- Check Redis connection string
- Ensure correct messaging provider is configured

### Service Communication Failures
- Check service URLs and ports
- Verify network connectivity
- Check firewall rules
- Review service logs

## Next Steps

1. **API Gateway**: Add a unified entry point (Ocelot, YARP)
2. **Service Discovery**: Implement Consul or Eureka
3. **Distributed Tracing**: Add OpenTelemetry
4. **Centralized Logging**: Use ELK Stack or Azure Application Insights
5. **Authentication**: Add IdentityServer or Auth0
6. **CI/CD**: Set up GitHub Actions or Azure DevOps
7. **Kubernetes**: Create K8s manifests for production deployment

## References

- [Microservices Architecture Documentation](docs/microservices-architecture.md)
- [Docker Documentation](https://docs.docker.com/)
- [.NET Microservices Guide](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/)
- [MessagePack Documentation](https://github.com/MessagePack-CSharp/MessagePack-CSharp)
