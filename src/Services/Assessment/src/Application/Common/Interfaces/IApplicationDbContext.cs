using Assessment.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Assessment.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }
    DbSet<MainCategory> MainCategories { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
