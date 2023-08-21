namespace SteamActivityTracker.Infrastructure.Clients.Steam.Models;

public record Achievement(
    string ApiName,
    int Achieved,
    int UnlockTime,
    string Name,
    string Description
) : SteamClientModel();
