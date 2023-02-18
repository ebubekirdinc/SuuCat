 
using Microsoft.EntityFrameworkCore;

namespace Discount.Application.Common.Interfaces;

public interface IApplicationDbContext
{ 

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
