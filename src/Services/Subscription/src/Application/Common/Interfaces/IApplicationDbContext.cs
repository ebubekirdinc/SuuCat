 
using Microsoft.EntityFrameworkCore;

namespace Subscription.Application.Common.Interfaces;

public interface IApplicationDbContext
{ 

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
