namespace SteamActivityTracker.Infrastructure.Clients.Steam.Models;

public record OwnedGames(
    int GameCount,
    Game[] Games
) : SteamClientModel();
