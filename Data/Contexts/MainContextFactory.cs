using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispoDataAssistant.Data.Contexts
{
    public class MainContextFactory : IDesignTimeDbContextFactory<AssetContext>
    {
        public AssetContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<AssetContext> optionsBuilder = new();

            optionsBuilder
                .UseSqlite("Data Source=DispoAssistant.db")
                .UseLazyLoadingProxies();

            return new AssetContext(optionsBuilder.Options);
        }
    }
}
