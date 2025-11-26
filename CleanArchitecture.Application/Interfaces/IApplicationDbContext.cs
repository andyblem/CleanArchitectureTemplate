using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Book> Books { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}