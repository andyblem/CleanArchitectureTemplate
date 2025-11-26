using AutoMapper;
using CleanArchitecture.Application.DTOs.Book;
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

namespace CleanArchitecture.Application.Features.Books.Queries.GetAllBooks
{
    public class GetAllBooksQuery : IRequest<PagedResponse<IEnumerable<BookListItemDTO>>>
    {
        public GetAllBooksParameter Parameters { get; set; }
    }

    public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, PagedResponse<IEnumerable<BookListItemDTO>>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger _logger;

        public GetAllBooksQueryHandler(IApplicationDbContext dbContext, ILogger logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<PagedResponse<IEnumerable<BookListItemDTO>>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            // get books
            var booksQuery = _dbContext.Books
                .Select(b => new BookListItemDTO
                {
                    Id = b.Id,
                    Title = b.Title,
                    ISBN = b.ISBN,
                    Price = b.Price
                })
                .AsNoTracking()
                .AsQueryable();

            // count total books
            var totalBooks = await booksQuery.CountAsync();
            var result = booksQuery
                .Skip((request.Parameters.PageNumber - 1) * request.Parameters.PageSize)
                .Take(request.Parameters.PageSize)
                .ToList();

            // return paged response
            return new PagedResponse<IEnumerable<BookListItemDTO>>(result, 
                request.Parameters.PageNumber, 
                request.Parameters.PageSize,
                totalBooks);
        }
    }
}
