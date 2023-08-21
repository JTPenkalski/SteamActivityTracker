using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SteamActivityTracker.Abstractions.Clients;
using SteamActivityTracker.Core.Options;
using SteamActivityTracker.Infrastructure.Clients.Steam.Models;

namespace SteamActivityTracker.Infrastructure.Clients.Steam;

public class SteamUserStatsClient : BaseSteamClient, ISteamUserStatsClient
{
    public override string Interface => "ISteamUserStats";

    public SteamUserStatsClient(
        IOptions<SteamClientOptions> config,
        ILogger<SteamUserStatsClient> logger,
        IHttpClientFactory httpClientFactory
    ) : base(config, logger, httpClientFactory) { }

    public async Task<IEnumerable<Core.Models.Achievement>> GetPlayerAchievementsAsync(string steamId, string appId)
    {
        var request = new SteamRequest
        {
            Interface = Interface,
            Endpoint = "GetPlayerAchievements",
            Version = SteamRequest.VERSION_1,
            WrapResponse = false
        }.WithQuery(
            ("steamid", steamId),
            ("appid", appId),
            ("l", "en")
        );

        PlayerAchievements? playerStats = await GetAsync<PlayerAchievements>(request);

        return playerStats?.PlayerStats?.Achievements is null || !playerStats.PlayerStats.Success
            ? Enumerable.Empty<Core.Models.Achievement>()
            : playerStats.PlayerStats.Achievements.Select(x => new Core.Models.Achievement
            {
                Name = x.Name,
                AppId = appId,
                Description = x.Description,
                UnlockTime = x.Achieved == 1 ? DateTimeOffset.FromUnixTimeSeconds(x.UnlockTime) : null
            });
    }
}
