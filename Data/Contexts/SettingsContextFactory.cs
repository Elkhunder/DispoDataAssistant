using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DispoDataAssistant.Data.Contexts
{
    public class SettingsContextFactory : IDesignTimeDbContextFactory<SettingsContext>
    {
        public SettingsContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<SettingsContext> optionsBuilder = new();
            optionsBuilder.UseSqlite("Data Source=DispoAssisstant.db");
            optionsBuilder.UseLazyLoadingProxies();

            return new SettingsContext(optionsBuilder.Options);
        }
    }
}
