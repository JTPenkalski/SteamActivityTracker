using Microsoft.EntityFrameworkCore;

namespace SteamActivityTracker.Core.Models;

/// <summary>
/// The public representation of a <see cref="User"/>.
/// </summary>
[Owned]
public class Persona
{
    public string Name { get; set; } = string.Empty;
    
    public string AvatarUri { get; set; } = string.Empty;
    
    public PersonaState State { get; set; }
}
