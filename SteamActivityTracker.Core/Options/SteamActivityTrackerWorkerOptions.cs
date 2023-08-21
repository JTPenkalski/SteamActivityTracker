using System.ComponentModel.DataAnnotations;

namespace SteamActivityTracker.Core.Options;

public sealed class SteamActivityTrackerWorkerOptions
{
    public string SteamId { get; set; } = string.Empty;

    [Range(5, 3600)]
    public int PollInterval { get; set; }
}