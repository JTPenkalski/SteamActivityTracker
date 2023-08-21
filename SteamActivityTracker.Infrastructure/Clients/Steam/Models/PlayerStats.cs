namespace SteamActivityTracker.Infrastructure.Clients.Steam.Models;

public record PlayerStats(
    string SteamId,
    string GameName,
    Achievement[] Achievements,
    bool Success
) : SteamClientModel();
