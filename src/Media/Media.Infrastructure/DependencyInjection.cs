using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Media.Core;
using Media.Core.Services;
using Media.Infrastructure.Data;
using Media.Infrastructure.Services;

namespace Media.Infrastructure;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddMediaInfrastructureWithAspire(this IHostApplicationBuilder builder)
    {
        builder.AddSqlServerDbContext<MediaDbContext>("mediadb");
        builder.Services.AddScoped<IMediaDbContext>(sp => sp.GetRequiredService<MediaDbContext>());

        var storagePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        var baseUrl = builder.Configuration["Media:BaseUrl"] ?? "http://localhost:5007/files";
        builder.Services.AddSingleton<IFileStorageService>(new LocalFileStorageService(storagePath, baseUrl));

        return builder;
    }
}
