using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using MultiTenantApp.Common.Models;

namespace MultiTenantApp.Common.Interfaces
{
    public interface ITenantService
    {
        TenantModel GetTenant(HttpContext content);
    }
}