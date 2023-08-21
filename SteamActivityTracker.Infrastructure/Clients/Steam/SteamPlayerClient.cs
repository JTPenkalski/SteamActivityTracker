using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SteamActivityTracker.Abstractions.Clients;
using SteamActivityTracker.Core.Models;
using SteamActivityTracker.Core.Options;
using SteamActivityTracker.Infrastructure.Clients.Steam.Models;

namespace SteamActivityTracker.Infrastructure.Clients.Steam;

public class SteamPlayerClient : BaseSteamClient, ISteamPlayerClient
{
    public override string Interface => "IPlayerService";

    public SteamPlayerClient(
        IOptions<SteamClientOptions> config,
        ILogger<SteamPlayerClient> logger,
        IHttpClientFactory httpClientFactory
    ) : base(config, logger, httpClientFactory) { }

    public async Task<IEnumerable<TotalPlaytime>> GetOwnedGames(string steamId)
    {
        var request = new SteamRequest
        {
            Interface = Interface,
            Endpoint = "GetOwnedGames",
            Version = SteamRequest.VERSION_1
        }.WithQuery(
            ("steamid", steamId)
        );

        OwnedGames? ownedGames = await GetAsync<OwnedGames>(request);

        return ownedGames?.Games is null
            ? Enumerable.Empty<TotalPlaytime>()
            : ownedGames.Games.Select(x => new TotalPlaytime
            {
                AppId = x.AppId.ToString(),
                Playtime = x.PlaytimeForever
            });
    }    
}
