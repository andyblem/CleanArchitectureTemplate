using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Application.Wrappers;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Features.Books.Commands
{
    public class UpdateBookCommand : IRequest<Response<int>>
    {
        public Book Book { get; set; }
    }

    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, Response<int>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger<UpdateBookCommandHandler> _logger;


        public UpdateBookCommandHandler(IApplicationDbContext dbContext, ILogger<UpdateBookCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Response<int>> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // get book
                var book = request.Book;

                // set fields to update
                _dbContext.Books.Entry(book).Property(p => p.Price).IsModified = true;
                _dbContext.Books.Entry(book).Property(p => p.Summary).IsModified = true;
                _dbContext.Books.Entry(book).Property(p => p.Title).IsModified = true;

                // save changes
                await _dbContext.SaveChangesAsync(cancellationToken);

                // return result
                return Response<int>.Success(book.Id, "Book updated successfully");
            }
            catch (Exception ex)
            {
                // log error and return result
                _logger.LogError(ex, "Error updating book");
                return Response<int>.Failure("Error updating book", new List<string> { ex.Message });
            }
        }
    }
}
