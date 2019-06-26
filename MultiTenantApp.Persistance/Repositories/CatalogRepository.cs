using System;
using System.Collections.Generic;
using System.Linq;
using MultiTenantApp.Application.Interfaces;
using MultiTenantApp.Application.Mappers;
using MultiTenantApp.Application.Services;
using MultiTenantApp.Persistance.Contexts;

namespace MultiTenantApp.Persistance.Repositories
{
    public class CatalogRepository : ICatalogRepository
    {
        private readonly CatalogDbContext _database;
        public CatalogRepository(CatalogDbContext catalogDbContext)
        {
            _database = catalogDbContext;
        }

        public TenantModel GetTenantByDomain(string domain)
        {
           var tenant= _database.TenantConfigurations.FirstOrDefault(x => x.DomainName.ToLower() == domain.ToLower())??
                       _database.TenantConfigurations.FirstOrDefault(x => x.Default);
            var result = tenant?.ToTenantModel();

            return result;
        }

        public List<TenantModel> GetTenants()
        {
            var tenants = _database.TenantConfigurations.Select(x => x.ToTenantModel()).ToList();

            return tenants;
        }
    }
}