namespace MultiTenantApp.Application.Tests.Infrastructure
{
    using Microsoft.EntityFrameworkCore;
    using MultiTenantApp.Persistance.Contexts;
    using System;
    public class TravelContextFactory
    {
        public static TravelDbContext Create()
        {
            var options = new DbContextOptionsBuilder<TravelDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new TravelDbContext(options);
            context.Database.EnsureCreated();

            return context;
        }

        public static void Destroy(TravelDbContext context)
        {
            context.Database.EnsureDeleted();

            context.Dispose();
        }
    }
}