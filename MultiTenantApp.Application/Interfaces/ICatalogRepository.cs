using System.Collections.Generic;
using MultiTenantApp.Application.Services;

namespace MultiTenantApp.Application.Interfaces
{
    public interface ICatalogRepository
    {
        TenantModel GetTenantByDomain(string domain);
        List<TenantModel> GetTenants();
    }
}