using System.Text.Json;
using Identity.Core.Features.Auth.Login;
using Identity.Infrastructure;
using Identity.Infrastructure.Data;
using Microsoft.Data.SqlClient;
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

    // Auto-migrate in development with retry logic for database creation
    var logger = app.Services.GetRequiredService<ILogger<Program>>();

    // Get connection string from configuration
    var connectionString = builder.Configuration.GetConnectionString("identitydb");
    logger.LogInformation("Connection string from config: {ConnectionString}", connectionString?.Substring(0, Math.Min(50, connectionString?.Length ?? 0)) + "...");

    if (string.IsNullOrEmpty(connectionString))
    {
        logger.LogError("Connection string is null or empty!");
        throw new InvalidOperationException("Connection string is null or empty");
    }

    var connectionBuilder = new SqlConnectionStringBuilder(connectionString);
    var databaseName = connectionBuilder.InitialCatalog;
    logger.LogInformation("Database name: {DatabaseName}", databaseName);

    // First ensure the database exists by connecting to master
    connectionBuilder.InitialCatalog = "master";
    var retryCount = 0;
    var maxRetries = 20;

    while (retryCount < maxRetries)
    {
        try
        {
            logger.LogInformation("Attempting to ensure database exists (attempt {Attempt}/{MaxAttempts})...", retryCount + 1, maxRetries);

            await using var masterConnection = new SqlConnection(connectionBuilder.ConnectionString);
            await masterConnection.OpenAsync();

            // Check if database exists, create if not
            await using var checkCmd = masterConnection.CreateCommand();
            checkCmd.CommandText = $"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{databaseName}') CREATE DATABASE [{databaseName}]";
            await checkCmd.ExecuteNonQueryAsync();

            logger.LogInformation("Database '{DatabaseName}' exists or was created.", databaseName);
            break;
        }
        catch (Exception ex)
        {
            retryCount++;
            if (retryCount >= maxRetries)
            {
                logger.LogError(ex, "Failed to create database after {MaxRetries} attempts.", maxRetries);
                throw;
            }
            logger.LogWarning(ex, "Database creation failed (attempt {Attempt}), retrying in 3 seconds...", retryCount);
            await Task.Delay(3000);
        }
    }

    // Now run migrations
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
    retryCount = 0;
    while (retryCount < maxRetries)
    {
        try
        {
            logger.LogInformation("Attempting to migrate database (attempt {Attempt}/{MaxAttempts})...", retryCount + 1, maxRetries);
            await db.Database.MigrateAsync();
            logger.LogInformation("Database migration completed successfully.");
            break;
        }
        catch (Exception ex)
        {
            retryCount++;
            if (retryCount >= maxRetries)
            {
                logger.LogError(ex, "Failed to migrate database after {MaxRetries} attempts.", maxRetries);
                throw;
            }
            logger.LogWarning(ex, "Database migration failed (attempt {Attempt}), retrying in 3 seconds...", retryCount);
            await Task.Delay(3000);
        }
    }
}

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultEndpoints();
app.MapControllers();

app.Run();
