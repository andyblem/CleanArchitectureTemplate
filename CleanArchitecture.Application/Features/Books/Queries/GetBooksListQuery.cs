using AutoMapper;
using CleanArchitecture.Application.DTOs.Book;
using CleanArchitecture.Application.Filters;
using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Application.Parameters.Book;
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

namespace CleanArchitecture.Application.Features.Books.Queries
{
    public class GetBooksListQuery : IRequest<PagedResponse<IEnumerable<BookListItemDTO>>>
    {
        public GetBooksListParameter Parameters { get; set; }
    }

    public class GetBooksListQueryHandler : IRequestHandler<GetBooksListQuery, PagedResponse<IEnumerable<BookListItemDTO>>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger _logger;

        public GetBooksListQueryHandler(IApplicationDbContext dbContext, ILogger logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<PagedResponse<IEnumerable<BookListItemDTO>>> Handle(GetBooksListQuery request, CancellationToken cancellationToken)
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
            return new PagedResponse<IEnumerable<BookListItemDTO>>(result, 
                request.Parameters.PageNumber, 
                request.Parameters.PageSize,
                totalBooks);
        }
    }
}
