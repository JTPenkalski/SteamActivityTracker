namespace SteamActivityTracker.Core.Models;

/// <summary>
/// A Steam achievement.
/// </summary>
public class Achievement
{
    public int Id { get; set; }

    public string AppId { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTimeOffset? UnlockTime { get; set; }
}
