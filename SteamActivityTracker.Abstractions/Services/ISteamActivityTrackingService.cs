namespace SteamActivityTracker.Abstractions.Services;

/// <summary>
/// Main service for tracking Steam activity for a user.
/// </summary>
public interface ISteamActivityTrackingService
{
    /// <summary>
    /// Detects changes in user state and records entries in a local database
    /// for each session of an app used on Steam.
    /// </summary>
    /// <param name="steamId">The steam ID of the user.</param>
    Task TrackActivityAsync(string steamId);
}
