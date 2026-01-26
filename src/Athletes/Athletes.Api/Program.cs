using System.Text.Json;
using Athletes.Core.Features.Athletes.CreateAthlete;
using Athletes.Infrastructure;
using Athletes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using NoDaysOff.ServiceDefaults;
using Shared.Authentication;
using Shared.Domain.Exceptions;
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

// Exception handling middleware
app.Use(async (context, next) =>
{
    try
    {
        await next(context);
    }
    catch (UnauthorizedException ex)
    {
        context.Response.StatusCode = 401;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(new { message = ex.Message }));
    }
    catch (NotFoundException ex)
    {
        context.Response.StatusCode = 404;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(new { message = ex.Message }));
    }
    catch (ConflictException ex)
    {
        context.Response.StatusCode = 409;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(new { message = ex.Message }));
    }
    catch (ValidationException ex)
    {
        context.Response.StatusCode = 400;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(new { message = ex.Message, errors = ex.Errors }));
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        var errorResponse = new { message = ex.Message, stackTrace = app.Environment.IsDevelopment() ? ex.StackTrace : null };
        await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
    }
});

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
