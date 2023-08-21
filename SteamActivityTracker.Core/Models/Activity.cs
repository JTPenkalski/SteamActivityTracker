namespace SteamActivityTracker.Core.Models;

/// <summary>
/// A specific usage of a Steam app by a user.
/// </summary>
public class Activity
{
    public int Id { get; set; }

    public DateTimeOffset StartTime { get; set; }

    public DateTimeOffset? EndTime { get; set; }

    public int LifetimePlaytime { get; set; }

    public User User { get; set; } = null!;

    public App App { get; set; } = null!;
}
