using SteamActivityTracker.Core.Models;

namespace SteamActivityTracker.Abstractions.Clients;

/// <summary>
/// Provides additional methods for interacting with Steam Users.
/// </summary>
public interface ISteamPlayerClient
{
    /// <summary>
    /// Gets information about a player's recently played games.
    /// </summary>
    /// <param name="steamId">The steam ID of the user.</param>
    /// <returns>A collection of <see cref="TotalPlaytime"/> instances.</returns>
    Task<IEnumerable<TotalPlaytime>> GetOwnedGames(string steamId);
}
