using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SteamActivityTracker.Abstractions.Clients;
using SteamActivityTracker.Abstractions.Services;
using SteamActivityTracker.Core.Contexts;
using SteamActivityTracker.Core.Models;

namespace SteamActivityTracker.Infrastructure.Services;

public class SteamActivityTrackingService : ISteamActivityTrackingService
{
    protected readonly ILogger logger;
    protected readonly IDbContextFactory<SteamActivityTrackerContext> contextFactory;
    protected readonly ISteamUserClient userClient;
    protected readonly ISteamUserStatsClient userStatsClient;
    protected readonly ISteamPlayerClient playerClient;

    public SteamActivityTrackingService(
        ILogger<SteamActivityTrackingService> logger,
        IDbContextFactory<SteamActivityTrackerContext> contextFactory,
        ISteamUserClient userClient,
        ISteamUserStatsClient userStatsClient,
        ISteamPlayerClient playerClient
    )
    {
        this.logger = logger;
        this.contextFactory = contextFactory;
        this.userClient = userClient;
        this.userStatsClient = userStatsClient;
        this.playerClient = playerClient;
    }

    public async Task TrackActivityAsync(string steamId)
    {
        logger.LogInformation("{method} started.", nameof(TrackActivityAsync));

        using SteamActivityTrackerContext context = await contextFactory.CreateDbContextAsync();
        Activity? lastKnownActivity = await GetLastKnownIncompleteActivityAsync(context);
        User? currentUserStatus = await userClient.GetPlayerSummaryAsync(steamId);

        if (lastKnownActivity?.App.Id is null && currentUserStatus?.CurrentApp?.Id is not null)
        {
            // User began a new activity
            await StartActivityAsync(context, currentUserStatus);
        }
        else if (lastKnownActivity?.App.Id is not null && currentUserStatus?.CurrentApp?.Id is null)
        {
            // User finished an existing activity
            await StopActivityAsync(context, lastKnownActivity);
        }
        else if (lastKnownActivity?.App.Id is not null && currentUserStatus?.CurrentApp?.Id is not null && lastKnownActivity.App.Id != currentUserStatus.CurrentApp.Id)
        {
            // User switched activities
            await StopActivityAsync(context, lastKnownActivity);
            await StartActivityAsync(context, currentUserStatus);
        }

        await context.SaveChangesAsync();

        logger.LogInformation("{method} finished.", nameof(TrackActivityAsync));
    }

    protected async Task StartActivityAsync(SteamActivityTrackerContext context, User currentUserStatus)
    {
        TotalPlaytime playtimeForApp = await GetTotalPlaytimeForAppAsync(currentUserStatus.Id, currentUserStatus.CurrentApp!.Id);

        var newActivity = new Activity
        {
            StartTime = DateTimeOffset.Now,
            LifetimePlaytime = playtimeForApp.Playtime,
            User = currentUserStatus,
            App = currentUserStatus.CurrentApp!
        };

        logger.LogInformation("A new activity was started for {app} at {time}", newActivity.App.Name, newActivity.StartTime);

        context.Activities.Add(newActivity);
    }

    protected async Task StopActivityAsync(SteamActivityTrackerContext context, Activity activity)
    {
        TotalPlaytime playtimeForApp = await GetTotalPlaytimeForAppAsync(activity.User.Id, activity.App.Id);

        activity.EndTime = activity.StartTime + TimeSpan.FromMinutes(playtimeForApp.Playtime - activity.LifetimePlaytime);

        await TrackAchievementsAsync(context, activity);

        logger.LogInformation("An existing activity was stopped for {app} at {time}", activity.App.Name, activity.EndTime);
    }

    protected async Task TrackAchievementsAsync(SteamActivityTrackerContext context, Activity activity)
    {
        IEnumerable<Achievement> lastKnownAchievements =
            context.Achievements
            .Where(x => x.AppId == activity.App.Id)
            .ToList();

        IEnumerable<Achievement> newAchievements =
            (await userStatsClient.GetPlayerAchievementsAsync(activity.User.Id, activity.App.Id))
            .Where(x => x.UnlockTime is not null && !lastKnownAchievements.Any(a => a.Id == x.Id));

        logger.LogInformation(
            "{count} new achievements were unlocked during the most recent activity: {achievements}",
            newAchievements.Count(),
            string.Join(',', newAchievements.Select(x => x.Name))
        );

        context.Achievements.AddRange(newAchievements);
    }

    protected async Task<Activity?> GetLastKnownIncompleteActivityAsync(SteamActivityTrackerContext context)
        => await context.Activities
            .Include(x => x.User)
            .Include(x => x.App)
            .Where(x => x.EndTime == null)
            .OrderByDescending(x => x.StartTime)
            .FirstOrDefaultAsync();

    protected async Task<TotalPlaytime> GetTotalPlaytimeForAppAsync(string userId, string appId)
        => (await playerClient.GetOwnedGames(userId))
            .First(x => x.AppId == appId);
}
