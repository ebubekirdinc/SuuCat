using Microsoft.EntityFrameworkCore;

namespace Order.Application.Common.Interfaces;

public interface IApplicationDbContext
{
 

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
