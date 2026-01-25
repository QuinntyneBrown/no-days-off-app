using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Shared.Authentication;
using Shared.Infrastructure;
using Media.Core.Features.UploadMediaFile;
using Media.Infrastructure;
using Media.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddRedisClient("redis");
builder.AddMediaInfrastructureWithAspire();
builder.Services.AddSharedMessagingWithAspire();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UploadMediaFileCommand).Assembly));
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
    var db = scope.ServiceProvider.GetRequiredService<MediaDbContext>();
    await db.Database.MigrateAsync();
}

var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
Directory.CreateDirectory(uploadsPath);
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsPath),
    RequestPath = "/files"
});

app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultEndpoints();
app.MapControllers();
app.Run();
