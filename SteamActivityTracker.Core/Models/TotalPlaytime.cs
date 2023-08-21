namespace SteamActivityTracker.Core.Models;

/// <summary>
/// Records the total time played for a specific <see cref="App"/>.
/// </summary>
public class TotalPlaytime
{
    public string AppId { get; set; } = string.Empty;

    public int Playtime { get; set; }
}
