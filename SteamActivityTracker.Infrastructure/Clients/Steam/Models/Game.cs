using System.Text.Json.Serialization;

namespace SteamActivityTracker.Infrastructure.Clients.Steam.Models;

public record Game(
    int AppId,
    [property: JsonPropertyName("playtime_forever")] int PlaytimeForever,
    [property: JsonPropertyName("playtime_windows_forever")] int PlaytimeWindowsForever,
    [property: JsonPropertyName("playtime_mac_forever")] int PlaytimeMacForever,
    [property: JsonPropertyName("playtime_linux_forever")] int PlaytimeLinuxForever,
    [property: JsonPropertyName("rtime_last_played")] int TimeLastPlayed,
    [property: JsonPropertyName("playtime_disconnected")] int PlaytimeDisconnected
) : SteamClientModel();
