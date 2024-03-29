﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using MultiTenantApp.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using MultiTenantApp.Const;

namespace MultiTenantApp.Application.Services
{
    public class TenantService : ITenantService
    {
        private const string TENANT_NOT_FOUND = "Tenant not found";
        private const string EMPTY_URL = "Url without domain";

        internal static readonly object Locker = new object();
        private readonly ICatalogRepository _catalogRepository;
        private readonly IHttpContextAccessor _accessor;
        private readonly IMemoryCache _cache;
        public TenantService(ICatalogRepository catalogRepository, IHttpContextAccessor accessor, IMemoryCache cache)
        {
            _catalogRepository = catalogRepository;
            _accessor = accessor;
            _cache = cache;
        }

        public void SetTenantsToCache()
        {
            var tenants = (List<TenantModel>)_cache.Get(TenantConst.CACHE_NAME);

            if (tenants == null)
            {
                lock (Locker)
                {
                    if (tenants == null)
                    {
                         tenants = _catalogRepository.GetTenants();
                        _cache.Set(TenantConst.CACHE_NAME, tenants, new TimeSpan(0, 0,30));
                    }
                }
            }
        }

        public TenantModel GetTenant()
        {
            var domain = _accessor.HttpContext.Request.Host.HasValue ? _accessor.HttpContext.Request.Host.Host : null;
            if (domain == null)
                throw new ApplicationException(EMPTY_URL);

            if (_accessor.HttpContext.Items.TryGetValue(TenantConst.HTTPCONTEXT_KEY, out var tenant))
                return tenant as TenantModel;

            if (_cache.TryGetValue(TenantConst.CACHE_NAME, out var tenantsTemp))
            {
                var tenants = (List<TenantModel>) tenantsTemp;

                return tenants.FirstOrDefault(x => x.DomainName.ToLower() == domain.ToLower()) ??
                       tenants.FirstOrDefault(x => x.Default);
            }

            tenant = _catalogRepository.GetTenantByDomain(domain);
            if (tenant == null)
                throw new ApplicationException(TENANT_NOT_FOUND);

            return (TenantModel)tenant;
        }
    }
}