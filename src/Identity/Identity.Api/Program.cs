using Identity.Core.Features.Auth.Login;
using Identity.Infrastructure;
using Identity.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using NoDaysOff.ServiceDefaults;
using Shared.Authentication;
using Shared.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add Aspire service defaults
builder.AddServiceDefaults();

// Add Redis for messaging
builder.AddRedisClient("redis");

// Add Identity infrastructure with Aspire
builder.AddIdentityInfrastructureWithAspire();

// Add shared messaging
builder.Services.AddSharedMessagingWithAspire();

// Add MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(LoginCommand).Assembly);
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
    c.SwaggerDoc("v1", new() { Title = "Identity API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new()
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new()
    {
        {
            new()
            {
                Reference = new() { Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Auto-migrate in development
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
    await db.Database.MigrateAsync();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultEndpoints();
app.MapControllers();

app.Run();
