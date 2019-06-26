using Microsoft.AspNetCore.Http;
using MultiTenantApp.Application.Services;

namespace MultiTenantApp.Application.Interfaces
{
    public interface ITenantService
    {
        TenantModel GetTenant();
        void SetTenantsToCache();
    }
}