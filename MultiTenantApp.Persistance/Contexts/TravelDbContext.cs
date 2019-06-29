using Microsoft.EntityFrameworkCore;
using MultiTenantApp.Application.Interfaces;
using MultiTenantApp.Application.Services;
using MultiTenantApp.Domain.Travel;

namespace MultiTenantApp.Persistance.Contexts
{
    public class TravelDbContext : DbContext,ITravelDbContext
    {
        private readonly TenantModel _tenant;

        public TravelDbContext(DbContextOptions<TravelDbContext> options)
            :base(options)
        {
            
        }
        public TravelDbContext(DbContextOptions<TravelDbContext> options, ITenantService tenantService)
            : base(options)
        {
            _tenant = tenantService.GetTenant();
        }
        public DbSet<Travel> Travels { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                            optionsBuilder.UseSqlServer(_tenant.ConnectionString);
            }

            base.OnConfiguring(optionsBuilder);
        }
    }
}