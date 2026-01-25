using Microsoft.EntityFrameworkCore;
using NoDaysOff.ServiceDefaults;
using Shared.Authentication;
using Shared.Infrastructure;
using Exercises.Core.Features.Exercises.CreateExercise;
using Exercises.Infrastructure;
using Exercises.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddRedisClient("redis");
builder.AddExercisesInfrastructureWithAspire();
builder.Services.AddSharedMessagingWithAspire();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateExerciseCommand).Assembly));
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
    var db = scope.ServiceProvider.GetRequiredService<ExercisesDbContext>();
    await db.Database.MigrateAsync();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultEndpoints();
app.MapControllers();
app.Run();
