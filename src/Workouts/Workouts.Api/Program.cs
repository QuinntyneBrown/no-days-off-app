using Microsoft.EntityFrameworkCore;
using NoDaysOff.ServiceDefaults;
using Shared.Authentication;
using Shared.Infrastructure;
using Workouts.Core.Features.Workouts.CreateWorkout;
using Workouts.Infrastructure;
using Workouts.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddRedisClient("redis");
builder.AddWorkoutsInfrastructureWithAspire();
builder.Services.AddSharedMessagingWithAspire();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateWorkoutCommand).Assembly));
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<WorkoutsDbContext>();
    await db.Database.MigrateAsync();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultEndpoints();
app.MapControllers();
app.Run();
