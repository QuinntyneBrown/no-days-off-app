var builder = DistributedApplication.CreateBuilder(args);

// Infrastructure Resources
var redis = builder.AddRedis("redis")
    .WithDataVolume("redis-data");

var sqlServer = builder.AddSqlServer("sqlserver")
    .WithDataVolume("sqlserver-data");

// Databases - one per service
var identityDb = sqlServer.AddDatabase("identitydb", "NoDaysOff.Identity");
var athletesDb = sqlServer.AddDatabase("athletesdb", "NoDaysOff.Athletes");
var workoutsDb = sqlServer.AddDatabase("workoutsdb", "NoDaysOff.Workouts");
var exercisesDb = sqlServer.AddDatabase("exercisesdb", "NoDaysOff.Exercises");
var dashboardDb = sqlServer.AddDatabase("dashboarddb", "NoDaysOff.Dashboard");
var mediaDb = sqlServer.AddDatabase("mediadb", "NoDaysOff.Media");
var communicationDb = sqlServer.AddDatabase("communicationdb", "NoDaysOff.Communication");

// Microservices
var identityService = builder.AddProject<Projects.Identity_Api>("identity-api")
    .WithReference(identityDb)
    .WithReference(redis)
    .WaitFor(identityDb)
    .WaitFor(redis);

var athletesService = builder.AddProject<Projects.Athletes_Api>("athletes-api")
    .WithReference(athletesDb)
    .WithReference(redis)
    .WaitFor(athletesDb)
    .WaitFor(redis);

var workoutsService = builder.AddProject<Projects.Workouts_Api>("workouts-api")
    .WithReference(workoutsDb)
    .WithReference(redis)
    .WaitFor(workoutsDb)
    .WaitFor(redis);

var exercisesService = builder.AddProject<Projects.Exercises_Api>("exercises-api")
    .WithReference(exercisesDb)
    .WithReference(redis)
    .WaitFor(exercisesDb)
    .WaitFor(redis);

var dashboardService = builder.AddProject<Projects.Dashboard_Api>("dashboard-api")
    .WithReference(dashboardDb)
    .WithReference(redis)
    .WaitFor(dashboardDb)
    .WaitFor(redis);

var mediaService = builder.AddProject<Projects.Media_Api>("media-api")
    .WithReference(mediaDb)
    .WithReference(redis)
    .WaitFor(mediaDb)
    .WaitFor(redis);

var communicationService = builder.AddProject<Projects.Communication_Api>("communication-api")
    .WithReference(communicationDb)
    .WithReference(redis)
    .WaitFor(communicationDb)
    .WaitFor(redis);

// API Gateway
builder.AddProject<Projects.ApiGateway>("api-gateway")
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

builder.Build().Run();
