using Microsoft.Extensions.Options;
using SteamActivityTracker.Abstractions.Services;
using SteamActivityTracker.Core.Options;

namespace SteamActivityTracker.Workers;

public sealed class SteamActivityTrackerWorker : BackgroundService
{
    private readonly SteamActivityTrackerWorkerOptions config;
    private readonly ILogger<SteamActivityTrackerWorker> logger;
    private readonly ISteamActivityTrackingService steamActivityTrackingService;

    public SteamActivityTrackerWorker(
        IOptions<SteamActivityTrackerWorkerOptions> config,
        ILogger<SteamActivityTrackerWorker> logger,
        ISteamActivityTrackingService steamActivityTrackingService
    )
    {
        this.config = config.Value;
        this.logger = logger;
        this.steamActivityTrackingService = steamActivityTrackingService;
    }

    protected sealed override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            logger.LogInformation("{worker} configured to run every {interval} seconds.", nameof(SteamActivityTrackerWorker), config.PollInterval);

            while (!stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("{worker} running at: {time}", nameof(SteamActivityTrackerWorker), DateTimeOffset.Now);

                await DoWorkAsync();
                
                await Task.Delay(TimeSpan.FromSeconds(config.PollInterval), stoppingToken);
            }
        }
        catch (TaskCanceledException)
        {
            // When the stopping token is explicitly canceled, we expect this and shouldn't exit with a non-zero exit code
            logger.LogInformation("Service cancelation requested. Shutting down {worker}.", nameof(SteamActivityTrackerWorker));
        }
        catch (Exception ex)
        {
            // Terminate this process and return a non-zero exit code to the operating system
            logger.LogError(ex, "{message}", ex.Message);

            Environment.Exit(1);
        }
    }

    private async Task DoWorkAsync()
    {
        await steamActivityTrackingService.TrackActivityAsync(config.SteamId);
    }
}