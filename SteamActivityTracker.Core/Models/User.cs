using System.ComponentModel.DataAnnotations.Schema;

namespace SteamActivityTracker.Core.Models;

/// <summary>
/// A user with a Steam account.
/// </summary>
public class User
{
    public string Id { get; set; } = string.Empty;

    public DateTimeOffset LastLogOff { get; set; }

    public Persona Persona { get; set; } = null!;

    [NotMapped]
    public App? CurrentApp { get; set; }
}
