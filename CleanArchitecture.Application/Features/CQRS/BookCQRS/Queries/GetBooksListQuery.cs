using AutoMapper;
using CleanArchitecture.Application.DTOs.Book;
using CleanArchitecture.Application.Features.Parameters.Book;
using CleanArchitecture.Application.Filters;
using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Features.CQRS.Books.Queries
{
    public class GetBooksListQuery : IRequest<PagedResponse<IEnumerable<BookListItemDTO>>>
    {
        public GetBooksListParameter Parameters { get; set; }
    }

    public class GetBooksListQueryHandler : IRequestHandler<GetBooksListQuery, PagedResponse<IEnumerable<BookListItemDTO>>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger<GetBooksListQueryHandler> _logger;

        public GetBooksListQueryHandler(IApplicationDbContext dbContext, ILogger<GetBooksListQueryHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<PagedResponse<IEnumerable<BookListItemDTO>>> Handle(GetBooksListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // get books
                var booksQuery = _dbContext.Books
                    .AsNoTracking()
                    .OrderBy(b => b.Id)
                    .Select(b => new BookListItemDTO
                    {
                        Id = b.Id,
                        Title = b.Title,
                        ISBN = b.ISBN,
                        Price = b.Price
                    });

                // count total books
                var totalBooks = await booksQuery.CountAsync(cancellationToken);
                var result = await booksQuery
                    .Skip((request.Parameters.PageNumber - 1) * request.Parameters.PageSize)
                    .Take(request.Parameters.PageSize)
                    .ToListAsync(cancellationToken);

                // return paged response
                return PagedResponse<IEnumerable<BookListItemDTO>>.Success(result,
                    request.Parameters.PageNumber,
                    request.Parameters.PageSize,
                    totalBooks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting books list.");
                return PagedResponse<IEnumerable<BookListItemDTO>>.Failure("Error occurred while getting books list.", new List<string> { ex.Message });
            }
        }
    }
}
