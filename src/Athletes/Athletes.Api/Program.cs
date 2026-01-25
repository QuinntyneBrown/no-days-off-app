using Athletes.Core.Features.Athletes.CreateAthlete;
using Athletes.Infrastructure;
using Athletes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;
using Shared.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add Aspire service defaults
builder.AddServiceDefaults();

// Add Redis for messaging
builder.AddRedisClient("redis");

// Add Athletes infrastructure with Aspire
builder.AddAthletesInfrastructureWithAspire();

// Add shared messaging
builder.Services.AddSharedMessagingWithAspire();

// Add MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateAthleteCommand).Assembly);
});

// Add JWT authentication
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

// Add controllers
builder.Services.AddControllers();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Athletes API", Version = "v1" });
});

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Auto-migrate in development
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AthletesDbContext>();
    await db.Database.MigrateAsync();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultEndpoints();
app.MapControllers();

app.Run();
