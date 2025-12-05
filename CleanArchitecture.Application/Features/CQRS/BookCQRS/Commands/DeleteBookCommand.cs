using AutoMapper;
using CleanArchitecture.Application.Exceptions;
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

namespace CleanArchitecture.Application.Features.CQRS.Books.Commands
{
    public class DeleteBookCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
    }

    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, Response<int>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger<DeleteBookCommandHandler> _logger;

        public DeleteBookCommandHandler(IApplicationDbContext dbContext, ILogger<DeleteBookCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Response<int>> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // get book by id
                var book = await _dbContext.Books
                    .Where(b => b.Id == request.Id)
                    .FirstOrDefaultAsync();

                if (book != null)
                {
                    // removed book
                    _dbContext.SoftRemove(book);
                    await _dbContext.SaveChangesAsync(cancellationToken);

                    // return response
                    return Response<int>.Success(book.Id, "Book deleted successfully.");
                }
                else
                {
                    return Response<int>.Failure("Book not found.");
                }
            }
            catch (Exception ex)
            {
                // log error and return response
                _logger.LogError(ex, "Error deleting book with id {BookId}", request.Id);
                return Response<int>.Failure("Error deleting book with id {BookId}" + request.Id, new List<string> { ex.Message });
            }
        }
    }
}
