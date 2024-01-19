using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DispoDataAssistant.Data.Contexts
{
    public class ServiceNowAssetContextFactory : IDesignTimeDbContextFactory<ServiceNowAssetContext>
    {
        public ServiceNowAssetContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<ServiceNowAssetContext> optionsBuilder = new();
            optionsBuilder.UseSqlite("Data Source=DispoAssisstant.db");
            optionsBuilder.UseLazyLoadingProxies();

            return new ServiceNowAssetContext(optionsBuilder.Options);
        }
    }
}
