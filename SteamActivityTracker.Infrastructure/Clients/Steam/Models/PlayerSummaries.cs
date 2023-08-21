namespace SteamActivityTracker.Infrastructure.Clients.Steam.Models;

public record PlayerSummaries(
    Player[] Players
) : SteamClientModel();
