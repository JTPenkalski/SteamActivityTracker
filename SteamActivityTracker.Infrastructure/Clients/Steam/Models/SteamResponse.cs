namespace SteamActivityTracker.Infrastructure.Clients.Steam.Models;

public record SteamResponse<T>(T Response) where T : SteamClientModel;
