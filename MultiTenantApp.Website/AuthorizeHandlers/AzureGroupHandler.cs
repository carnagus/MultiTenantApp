using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using MultiTenantApp.Application.Interfaces;

namespace MultiTenantApp.Website.AuthorizeHandlers
{
    public class AzureGroupHandler: AuthorizationHandler<AzureGroupRequirement>
    {
        private readonly IUserAuthorizeService _authorizeService;
        public AzureGroupHandler(IUserAuthorizeService authorizeService)
        {
            _authorizeService = authorizeService;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AzureGroupRequirement requirement)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                return Task.CompletedTask;
            }
            if(_authorizeService.IsUserAllowedForDomain())
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}