 
using Microsoft.EntityFrameworkCore;
using Subscription.Domain.Entities;

namespace Subscription.Application.Common.Interfaces;

public interface IApplicationDbContext
{ 

    DbSet<Stock> Stocks { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
