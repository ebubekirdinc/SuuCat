using Microsoft.EntityFrameworkCore;

namespace Account.Application.Common.Interfaces;

public interface IApplicationDbContext
{


    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
