namespace SteamActivityTracker.Infrastructure.Clients.Steam.Models;

public record PlayerAchievements(
    PlayerStats PlayerStats
) : SteamClientModel();
