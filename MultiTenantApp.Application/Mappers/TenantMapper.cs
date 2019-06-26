using MultiTenantApp.Application.Services;
using MultiTenantApp.Domain.Catalog;

namespace MultiTenantApp.Application.Mappers
{
    public static class TenantMapper
    {
        public static TenantModel ToTenantModel(this TenantConfiguration tenantEntity)
        {
            return new TenantModel
            {
                DomainName = tenantEntity.DomainName,
                ConnectionString = tenantEntity.ConnectionString,
                Default = tenantEntity.Default
            };
        }
    }
}