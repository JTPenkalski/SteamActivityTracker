using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SteamActivityTracker.Abstractions.Clients;
using SteamActivityTracker.Core.Models;
using SteamActivityTracker.Core.Options;
using SteamActivityTracker.Infrastructure.Clients.Steam.Models;

namespace SteamActivityTracker.Infrastructure.Clients.Steam;

public class SteamUserClient : BaseSteamClient, ISteamUserClient
{
    public override string Interface => "ISteamUser";

    public SteamUserClient(
        IOptions<SteamClientOptions> config,
        ILogger<SteamUserClient> logger,
        IHttpClientFactory httpClientFactory
    ) : base(config, logger, httpClientFactory) { }

    public async Task<User?> GetPlayerSummaryAsync(string steamId)
    {
        var request = new SteamRequest
        {
            Interface = Interface,
            Endpoint = "GetPlayerSummaries",
            Version = SteamRequest.VERSION_2
        }.WithQuery(
            ("steamids", steamId)
        );

        PlayerSummaries? playerSummaries = await GetAsync<PlayerSummaries>(request);
        Player? player = playerSummaries?.Players.FirstOrDefault();

        return player is null
            ? null
            : new User()
            {
                Id = player.SteamId,
                LastLogOff = DateTimeOffset.FromUnixTimeSeconds(player.LastLogOff),
                Persona = new Persona
                {
                    Name = player.PersonaName,
                    AvatarUri = player.AvatarFull,
                    State = (PersonaState)player.PersonaState
                },
                CurrentApp = player.GameId is null
                    ? null
                    : new App
                    {
                        Id = player.GameId,
                        Name = player.GameExtraInfo!
                    } 
            };
    }
}
