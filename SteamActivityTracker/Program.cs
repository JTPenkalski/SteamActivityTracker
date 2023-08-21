using Microsoft.EntityFrameworkCore;
using SteamActivityTracker.Abstractions.Clients;
using SteamActivityTracker.Abstractions.Services;
using SteamActivityTracker.Core.Contexts;
using SteamActivityTracker.Core.Options;
using SteamActivityTracker.Infrastructure.Clients.Steam;
using SteamActivityTracker.Infrastructure.Services;
using SteamActivityTracker.Workers;

var builder = Host.CreateApplicationBuilder(args);

// Add Logging
builder.Logging
    .ClearProviders()
    .AddConsole()
    .Configure(options =>
    {
        // These flags are based on common distributed tracing concepts
        options.ActivityTrackingOptions =
            ActivityTrackingOptions.TraceId
            | ActivityTrackingOptions.SpanId;
    });

// Add Configuration
builder.Services
    .AddOptions<SteamActivityTrackerWorkerOptions>().BindConfiguration(nameof(SteamActivityTrackerWorkerOptions)).Services
    .AddOptions<SteamClientOptions>().BindConfiguration(nameof(SteamClientOptions));

// Add Services
builder.Services
    .AddHttpClient()
    .AddDbContextFactory<SteamActivityTrackerContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")))
    .AddSingleton<ISteamActivityTrackingService, SteamActivityTrackingService>()
    .AddSingleton<ISteamUserClient, SteamUserClient>()
    .AddSingleton<ISteamUserStatsClient, SteamUserStatsClient>()
    .AddSingleton<ISteamPlayerClient, SteamPlayerClient>();

// Add Service Workers
builder.Services
    .AddHostedService<SteamActivityTrackerWorker>();

builder.Build().Run();
