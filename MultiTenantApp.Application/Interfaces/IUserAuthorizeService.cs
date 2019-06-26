using System;
using System.Collections.Generic;

namespace MultiTenantApp.Application.Interfaces
{
    public interface IUserAuthorizeService
    {
        bool IsUserAllowedForDomain();
        bool IsAdminForDomain();
        string GetEmail();
        Guid GetId();

    }
}