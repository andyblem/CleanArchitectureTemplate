using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Book> Books { get; }

       void SoftRemove<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class, IAuditable;
       Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}