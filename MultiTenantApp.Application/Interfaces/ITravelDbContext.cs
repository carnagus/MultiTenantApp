using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MultiTenantApp.Domain.Travel;

namespace MultiTenantApp.Application.Interfaces
{
    public interface ITravelDbContext
    {
        DbSet<Travel> Travels { get; set; }
        DbSet<User> Users { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}