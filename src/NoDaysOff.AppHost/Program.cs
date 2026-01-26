var builder = DistributedApplication.CreateBuilder(args);

// Use existing infrastructure from docker-compose
var redis = builder.AddConnectionString("redis");

// Connection string references for databases (using existing SQL Server on localhost:1433)
var identityDb = builder.AddConnectionString("identitydb");
var athletesDb = builder.AddConnectionString("athletesdb");
var workoutsDb = builder.AddConnectionString("workoutsdb");
var exercisesDb = builder.AddConnectionString("exercisesdb");
var dashboardDb = builder.AddConnectionString("dashboarddb");
var mediaDb = builder.AddConnectionString("mediadb");
var communicationDb = builder.AddConnectionString("communicationdb");

// Microservices with fixed ports to match API Gateway configuration
var identityService = builder.AddProject<Projects.Identity_Api>("identity-api")
    .WithHttpEndpoint(port: 5000)
    .WithReference(identityDb)
    .WithReference(redis);

var athletesService = builder.AddProject<Projects.Athletes_Api>("athletes-api")
    .WithHttpEndpoint(port: 5001)
    .WithReference(athletesDb)
    .WithReference(redis);

var exercisesService = builder.AddProject<Projects.Exercises_Api>("exercises-api")
    .WithHttpEndpoint(port: 5002)
    .WithReference(exercisesDb)
    .WithReference(redis);

var workoutsService = builder.AddProject<Projects.Workouts_Api>("workouts-api")
    .WithHttpEndpoint(port: 5003)
    .WithReference(workoutsDb)
    .WithReference(redis);

var dashboardService = builder.AddProject<Projects.Dashboard_Api>("dashboard-api")
    .WithHttpEndpoint(port: 5004)
    .WithReference(dashboardDb)
    .WithReference(redis);

var mediaService = builder.AddProject<Projects.Media_Api>("media-api")
    .WithHttpEndpoint(port: 5005)
    .WithReference(mediaDb)
    .WithReference(redis);

var communicationService = builder.AddProject<Projects.Communication_Api>("communication-api")
    .WithHttpEndpoint(port: 5006)
    .WithReference(communicationDb)
    .WithReference(redis);

// API Gateway on port 5007
var apiGateway = builder.AddProject<Projects.ApiGateway>("api-gateway")
    .WithHttpEndpoint(port: 5007)
    .WithExternalHttpEndpoints()
    .WithReference(identityService)
    .WithReference(athletesService)
    .WithReference(workoutsService)
    .WithReference(exercisesService)
    .WithReference(dashboardService)
    .WithReference(mediaService)
    .WithReference(communicationService)
    .WaitFor(identityService)
    .WaitFor(athletesService)
    .WaitFor(workoutsService)
    .WaitFor(exercisesService)
    .WaitFor(dashboardService)
    .WaitFor(mediaService)
    .WaitFor(communicationService);

// Frontend on port 4200
builder.AddNpmApp("frontend", "../Ui", "start")
    .WithReference(apiGateway)
    .WaitFor(apiGateway)
    .WithHttpEndpoint(port: 4200, targetPort: 4200, isProxied: false)
    .WithExternalHttpEndpoints();

builder.Build().Run();
