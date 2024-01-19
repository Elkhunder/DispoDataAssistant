using DispoDataAssistant.Data.Models.Settings;
using Microsoft.EntityFrameworkCore;

namespace DispoDataAssistant.Data.Contexts;

public class SettingsContext : DbContext
{
    public DbSet<Settings> Settings => Set<Settings>();
    public DbSet<Integration> Integrations => Set<Integration>();
    public DbSet<General> General => Set<General>();

    public SettingsContext(DbContextOptions<SettingsContext> options) : base(options)
    {
    }

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
            .WithOne(i => i.Settings);
        //.HasForeignKey(i => i.SettingsId);
        modelBuilder.Entity<Settings>()
            .HasMany(s => s.General)
            .WithOne(g => g.Settings);
        //.HasForeignKey(g => g.SettingsId);

        base.OnModelCreating(modelBuilder);
    }


}
