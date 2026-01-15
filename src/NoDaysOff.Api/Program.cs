using NoDaysOff.Infrastructure;
using NoDaysOff.Shared;
using NoDaysOff.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddInfrastructure(builder.Configuration);

// Add messaging infrastructure for microservices communication
builder.Services.AddSharedMessaging(builder.Configuration);

// Add message subscriber service to demonstrate event listening
builder.Services.AddHostedService<MessageSubscriberService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
