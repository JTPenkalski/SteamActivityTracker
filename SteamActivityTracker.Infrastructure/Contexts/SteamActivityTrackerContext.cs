using Microsoft.EntityFrameworkCore;
using SteamActivityTracker.Core.Models;

namespace SteamActivityTracker.Core.Contexts;

/// <inheritdoc cref="ISteamActivityTrackerContext"/>
public sealed class SteamActivityTrackerContext : DbContext
{
    public DbSet<Achievement> Achievements => Set<Achievement>();
    public DbSet<Activity> Activities => Set<Activity>();
    public DbSet<App> Apps => Set<App>();
    public DbSet<User> Users => Set<User>();

    public SteamActivityTrackerContext(DbContextOptions<SteamActivityTrackerContext> options)
        : base(options)
    {
        //Database.EnsureDeleted();
        Database.EnsureCreated();
    }
}
