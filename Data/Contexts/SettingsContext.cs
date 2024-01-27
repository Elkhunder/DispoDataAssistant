using DispoDataAssistant.Data.Models.Settings;
using Microsoft.EntityFrameworkCore;

namespace DispoDataAssistant.Data.Contexts;

public class SettingsContext(DbContextOptions<SettingsContext> options) : DbContext(options)
{
    public DbSet<Settings> Settings => Set<Settings>();
    public DbSet<Integration> Integrations => Set<Integration>();
    public DbSet<General> General => Set<General>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=DispoAssisstant.db");
        optionsBuilder.UseLazyLoadingProxies();
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Settings>()
            .HasMany(s => s.Integrations)
            .WithOne(i => i.Settings)
            .HasForeignKey(i => i.SettingsId);
        modelBuilder.Entity<Settings>()
            .HasMany(s => s.General)
            .WithOne(g => g.Settings)
            .HasForeignKey(g => g.SettingsId);

        modelBuilder.Entity<Integration>()
            .HasOne<Settings>(e => e.Settings)
            .WithMany(e => e.Integrations)
            .HasForeignKey(e => e.SettingsId);

        modelBuilder.Entity<General>()
            .HasOne<Settings>(e => e.Settings)
            .WithMany(e => e.General)
            .HasForeignKey(e => e.SettingsId);

        base.OnModelCreating(modelBuilder);
    }


}
