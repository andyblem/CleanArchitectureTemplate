using CleanArchitecture.Application.DTOs.Book;
using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Application.Wrappers;
using CleanArchitecture.Domain.Entities;
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
    public class GetBookByIdQuery : IRequest<Response<BookDTO>>
    {
        public int Id { get; set; }
    }

    public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, Response<BookDTO>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger<GetBookByIdQueryHandler> _logger;

        public GetBookByIdQueryHandler(IApplicationDbContext dbContext, ILogger<GetBookByIdQueryHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Response<BookDTO>> Handle(GetBookByIdQuery query, CancellationToken cancellationToken)
        {
            try
            {
                // get book by id
                var book = await _dbContext.Books
                    .Where(b => b.Id == query.Id)
                    .Select(b => new BookDTO()
                    {
                        Price = b.Price,

                        Id = b.Id,

                        ISBN = b.ISBN,
                        Summary = b.Summary,
                        Title = b.Title
                    })
                    .FirstOrDefaultAsync();

                if (book == null)
                {
                    return new Response<BookDTO>()
                    {
                        Succeeded = false,
                        Message = "Book not found",
                        Data = null
                    };
                }
                else
                {
                    return new Response<BookDTO>()
                    {
                        Succeeded = true,
                        Message = "Book retrieved successfully",
                        Data = book
                    };
                }
            }
            catch (Exception ex)
            {
                // log exception and return failure response
                _logger.LogError(ex, "An error occurred while retrieving the book with ID {BookId}", query.Id);
                return new Response<BookDTO>()
                {
                    Succeeded = false,
                    Message = $"An error occurred while retrieving the book: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}
