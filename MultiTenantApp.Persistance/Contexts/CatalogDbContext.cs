using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MultiTenantApp.Domain.Catalog;
using System.IO;

namespace MultiTenantApp.Persistance.Contexts
{
    public class CatalogDbContext: DbContext
    {
        public CatalogDbContext(DbContextOptions<CatalogDbContext> options)
            :base(options)
        {
        }
        public DbSet<TenantConfiguration> TenantConfigurations { get; set; }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    // get the configuration from the app settings
        //    var config = new ConfigurationBuilder()
        //    .SetBasePath(Directory.GetCurrentDirectory())
        //    .AddJsonFile("appsettings.json")
        //    .Build();

        //    optionsBuilder?.UseSqlServer(config.GetConnectionString("CatalogDb"));
        //}
    }
}