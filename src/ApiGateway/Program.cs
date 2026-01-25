using System.Threading.RateLimiting;
using ApiGateway.Middleware;
using NoDaysOff.ServiceDefaults;
using Shared.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add Aspire service defaults
builder.AddServiceDefaults();

// Add Redis for distributed caching
builder.AddRedisClient("redis");

// Add YARP reverse proxy
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// Add JWT authentication for gateway-level validation
builder.Services.AddJwtAuthenticationForGateway(builder.Configuration);
builder.Services.AddAuthorization();

// Add rate limiting
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy("fixed", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 10
            }));

    options.AddPolicy("sliding", context =>
        RateLimitPartition.GetSlidingWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
            factory: _ => new SlidingWindowRateLimiterOptions
            {
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1),
                SegmentsPerWindow = 4,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 10
            }));
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins(
                "http://localhost:4200",
                "https://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// Add health checks
builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure middleware pipeline
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseCors("AllowAngular");

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

// Map default Aspire endpoints (health checks)
app.MapDefaultEndpoints();

// Map YARP reverse proxy
app.MapReverseProxy();

app.Run();
