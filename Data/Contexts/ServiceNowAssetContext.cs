using DispoDataAssistant.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DispoDataAssistant.Data.Contexts
{
    public class ServiceNowAssetContext : DbContext
    {
        public DbSet<ServiceNowAsset> ServiceNowAssets => Set<ServiceNowAsset>();

        public ServiceNowAssetContext(DbContextOptions<ServiceNowAssetContext> options) : base(options)
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
            base.OnModelCreating(modelBuilder);
        }
    }
}
