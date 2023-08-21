using SteamActivityTracker.Core.Models;

namespace SteamActivityTracker.Abstractions.Clients;

/// <summary>
/// Used to access information about users.
/// </summary>
public interface ISteamUserStatsClient
{
    /// <summary>
    /// Returns a list of achievements for this user by App ID.
    /// </summary>
    /// <param name="steamId">Steam ID of the user.</param>
    /// <param name="appId">App ID of the game.</param>
    /// <returns>A collection of <see cref="Achievement"/>.</returns>
    Task<IEnumerable<Achievement>> GetPlayerAchievementsAsync(string steamId, string appId);
}