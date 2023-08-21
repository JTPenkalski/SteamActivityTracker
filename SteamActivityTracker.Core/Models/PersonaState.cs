namespace SteamActivityTracker.Core.Models;

/// <summary>
/// A user's current status.
/// Defaults to <see cref="PersonaState.Offline"/> if the profile is Friends Only or Private.
/// </summary>
public enum PersonaState
{
    Offline,
    Online,
    Busy,
    Away,
    Snooze,
    LookingToTrade,
    LookingToPlay
}