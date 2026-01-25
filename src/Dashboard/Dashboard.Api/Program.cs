using Microsoft.EntityFrameworkCore;
using Shared.Authentication;
using Shared.Infrastructure;
using Dashboard.Core.Features.Widgets.CreateWidget;
using Dashboard.Infrastructure;
using Dashboard.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddRedisClient("redis");
builder.AddDashboardInfrastructureWithAspire();
builder.Services.AddSharedMessagingWithAspire();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateWidgetCommand).Assembly));
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
    var db = scope.ServiceProvider.GetRequiredService<DashboardDbContext>();
    await db.Database.MigrateAsync();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultEndpoints();
app.MapControllers();
app.Run();
