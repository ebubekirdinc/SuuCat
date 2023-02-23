using Microsoft.EntityFrameworkCore;

namespace Order.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Domain.Entities.Order> Orders { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
