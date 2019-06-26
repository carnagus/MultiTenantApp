using MultiTenantApp.Common.Models;
using MultiTenantApp.Domain.Catalog;

namespace MultiTenantApp.Common.Mappers
{
    public static class TenantMapper
    {
        public static TenantModel ToTenantModel(this TenantConfiguration tenantEntity)
        {
            return new TenantModel
            {
                Name = tenantEntity.Name,
                ConnectionString = tenantEntity.ConnectionString
            };
        }
    }
}