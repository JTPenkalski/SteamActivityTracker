namespace SteamActivityTracker.Infrastructure.Clients.Steam.Models;

public record Player(
    string SteamId,
    int CommunityVisibilityState,
    int ProfileState,
    string PersonaName,
    string ProfileUrl,
    string Avatar,
    string AvatarMedium,
    string AvatarFull,
    string AvatarHash,
    int LastLogOff,
    int PersonaState,
    string? RealName,
    string? PrimaryClanId,
    int? TimeCreated,
    int? PersonaStateFlags,
    string? GameExtraInfo,
    string? GameId,
    string? LocCountryCode,
    string? LocStateCode,
    string? LocCityId
) : SteamClientModel();
