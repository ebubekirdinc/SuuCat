 
using Microsoft.EntityFrameworkCore;

namespace Payment.Application.Common.Interfaces;

public interface IApplicationDbContext
{ 

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
