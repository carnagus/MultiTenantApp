using MultiTenantApp.Website.AuthorizeHandlers;

namespace MultiTenantApp.Website
{
    using MediatR;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.AzureAD.UI;
    using Microsoft.AspNetCore.Authentication.OpenIdConnect;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using MultiTenantApp.Application.Interfaces;
    using MultiTenantApp.Application.Services;
    using MultiTenantApp.Application.Travels.Queries.GetAllTravels;
    using MultiTenantApp.Persistance.Contexts;
    using MultiTenantApp.Persistance.Repositories;
    using System.Reflection;
    public class Startup
    {
        private ITenantService _tenantService;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAuthentication(AzureADDefaults.AuthenticationScheme)
                .AddAzureAD(options =>
                {
                   options.Instance= Configuration["Instance"];
                   options.Domain= Configuration["Domain"];
                   options.TenantId = Configuration["TenantId"];
                   options.ClientId = Configuration["ClientId"];
                   options.CallbackPath = Configuration["CallbackPath"];
                });

            //local config
            //services.AddAuthentication(AzureADDefaults.AuthenticationScheme)
            //    .AddAzureAD(options => Configuration.Bind("AzureAd", options));

            services.Configure<OpenIdConnectOptions>(AzureADDefaults.OpenIdScheme, options =>
            {
                options.Authority = options.Authority + "/v2.0/";         // Microsoft identity platform

                options.TokenValidationParameters.ValidateIssuer = false; // accept several tenants (here simplified)
            });



            //dbcontext
            var catalogDbConnectionFromAzure = Configuration["CatalogDb"];
            services.AddDbContext<CatalogDbContext>(options => options.UseSqlServer(catalogDbConnectionFromAzure));
            //local
            //services.AddDbContext<CatalogDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("CatalogDb")));
            //overriden in onconfigurating method for custom string from catalogDb
            services.AddDbContext<ITravelDbContext,TravelDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("CatalogDb")));

            services.AddAuthorization(options =>
            {
                options.AddPolicy("OnlyAzureGroupsForDomain",
                    policy => policy.Requirements.Add(new AzureGroupRequirement()));
            });
            services.AddTransient<IAuthorizationHandler, AzureGroupHandler>();

            //application services

            services.AddTransient<ICatalogRepository, CatalogRepository>();
            services.AddTransient<ITenantService, TenantService>();
            services.AddTransient<ISendGridService, SendGridService>();
            services.AddTransient<IUserAuthorizeService, UserAuthorizeService>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Add MediatR
            services.AddMediatR(typeof(GetAllTravelsQueryHandler).GetTypeInfo().Assembly);
            
            //create instance of service class
            var provider = services.BuildServiceProvider();
            _tenantService = provider.GetService<ITenantService>();

            //cache
            services.AddMemoryCache();

            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();
            

            //custom middleware
            app.Use(async (context, next) =>
            {
                _tenantService.SetTenantsToCache();
                var tenant = _tenantService.GetTenant();
                if (!context.Items.TryGetValue("Tenant",  out var tenantObject))
                {
                    context.Items.Add("Tenant", tenant);
                }
                await next.Invoke();
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
