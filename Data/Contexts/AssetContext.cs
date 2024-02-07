using DispoDataAssistant.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DispoDataAssistant.Data.Contexts
{
    public class AssetContext(DbContextOptions<AssetContext> options) : DbContext(options)
    {
        public DbSet<ServiceNowAsset> ServiceNowAssets => Set<ServiceNowAsset>();
        public DbSet<TabModel> Tabs => Set<TabModel>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlite("Data Source=DispoAssisstant.db")
                .UseLazyLoadingProxies();

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TabModel>()
                .HasMany(e => e.ServiceNowAssets)
                .WithOne(e => e.Tab)
                .HasForeignKey(e => e.TabId)
                .IsRequired(true);

            modelBuilder.Entity<ServiceNowAsset>()
                .HasOne<TabModel>(e => e.Tab)
                .WithMany(e => e.ServiceNowAssets)
                .HasForeignKey(e => e.TabId)
                .IsRequired(true);

            base.OnModelCreating(modelBuilder);
        }
    }
}
