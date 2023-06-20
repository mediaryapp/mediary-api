using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Common;

public interface IApplicationDbContext
{
    DbSet<TodoItem> TodoItems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
