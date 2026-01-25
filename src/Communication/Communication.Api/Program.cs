using Microsoft.EntityFrameworkCore;
using NoDaysOff.ServiceDefaults;
using Shared.Authentication;
using Shared.Infrastructure;
using Communication.Core.Features.CreateNotification;
using Communication.Infrastructure;
using Communication.Infrastructure.Data;
using Communication.Infrastructure.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddRedisClient("redis");
builder.AddCommunicationInfrastructureWithAspire();
builder.Services.AddSharedMessagingWithAspire();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateNotificationCommand).Assembly));
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<CommunicationDbContext>();
    await db.Database.MigrateAsync();
}

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultEndpoints();
app.MapControllers();
app.MapHub<NotificationHub>("/hubs/notifications");
app.Run();
