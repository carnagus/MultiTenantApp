using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MultiTenantApp.Application.Interfaces;

namespace MultiTenantApp.Application.Services
{
    public class UserAuthorizeService: IUserAuthorizeService
    {
        private const string ADMIN = "admin";
        private readonly ITenantService _tenantService;
        private readonly IConfiguration _configuration;
        private readonly Dictionary<string,string> _groupsDictionary;
        private readonly Dictionary<string,string> _userGroupsDictionary;
        private readonly TenantModel _tenant;
        private readonly IHttpContextAccessor _accessor;
        public UserAuthorizeService(ITenantService tenantService, IConfiguration configuration, IHttpContextAccessor accessor)
        {
            _tenantService = tenantService;
            _configuration = configuration;
            _accessor = accessor;
            _groupsDictionary= new Dictionary<string, string>
            {
                {_configuration["SpanishTravelUser"],"SpanishTravelUser" },
                {_configuration["SpanishTravelAdmin"],"SpanishTravelAdmin" },
                {_configuration["PolishTravelUser"],"PolishTravelUser"},
                {_configuration["PolishTravelAdmin"],"PolishTravelAdmin" }
            };
            _tenant= _tenantService.GetTenant();
            _userGroupsDictionary = GetUsersGroupDictionary();
        }

        public bool IsUserAllowedForDomain()
        {
            return _userGroupsDictionary.Values.Any(x => x.ToLower().Contains(_tenant.Name.ToLower()));
        }
        public bool IsAdminForDomain()
        {
            return _groupsDictionary.Values.Any(x => x.ToLower().Contains(ADMIN) && x.ToLower().Contains(_tenant.Name.ToLower()));
        }

        public string GetEmail()
        {
            return _accessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == "preferred_username")?.Value;
        }

        public Guid GetId()
        {
            return new Guid(_accessor.HttpContext?.User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value);
        }

        private Dictionary<string, string> GetUsersGroupDictionary()
        {
            if (_accessor.HttpContext.User == null || !_accessor.HttpContext.User.Identity.IsAuthenticated)
                return null;

            var groups = _accessor.HttpContext.User.Claims.Where(x => x.Type == "groups");
            var groupsValues = groups.Select(x => x.Value).ToList();

            var userGroups = groupsValues.FindAll(x => _groupsDictionary.Keys.Contains(x));
            var dictionary = new Dictionary<string, string>();
            foreach (var key in userGroups)
            {
                dictionary.Add(key, _groupsDictionary[key]);
            }

            return dictionary;
        }
    }
}