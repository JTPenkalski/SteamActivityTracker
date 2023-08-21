using SteamActivityTracker.Core.Models;

namespace SteamActivityTracker.Abstractions.Clients;

/// <summary>
/// Used to access information and interact with users.
/// </summary>
public interface ISteamUserClient
{
    /// <summary>
    /// Retrieves basic profile information about a specific Steam ID.
    /// </summary>
    /// <param name="steamId">The steam ID of the user.</param>
    /// <returns>A <see cref="User"/> instance, or null if the user wasn't found.</returns>
    Task<User?> GetPlayerSummaryAsync(string steamId);
}
