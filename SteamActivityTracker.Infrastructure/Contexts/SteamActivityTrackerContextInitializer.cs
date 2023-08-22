using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using SteamActivityTracker.Core.Contexts;

namespace SteamActivityTracker.Infrastructure.Contexts;

internal class SteamActivityTrackerContextInitializer : IDesignTimeDbContextFactory<SteamActivityTrackerContext>
{
    public SteamActivityTrackerContext CreateDbContext(string[] args)
    {
        string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        string basePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())!.ToString(), nameof(SteamActivityTracker));

        IConfigurationRoot config = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<SteamActivityTrackerContext>()
            .UseSqlServer(config.GetConnectionString("Default"));

        return new SteamActivityTrackerContext(optionsBuilder.Options);
    }
}
