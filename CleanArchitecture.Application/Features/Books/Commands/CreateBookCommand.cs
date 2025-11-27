using CleanArchitecture.Application.Wrappers;
using AutoMapper;
using CleanArchitecture.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System;

namespace CleanArchitecture.Application.Features.Books.Commands
{
    public partial class CreateBookCommand : IRequest<Response<int>>
    {
        public Book Book { get; set; }
    }

    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, Response<int>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger<CreateBookCommandHandler> _logger;

        public CreateBookCommandHandler(IApplicationDbContext dbContext, ILogger<CreateBookCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Response<int>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // save book
                var book = request.Book;
                await _dbContext.Books.AddAsync(book, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);

                // return response
                return Response<int>.Success(book.Id);
            }
            catch (DbUpdateException dbEx)
            {
                // log error and return response
                _logger.LogWarning(dbEx, "Database update failed (possible duplicate ISBN).");
                return Response<int>.Failure("ISBN already exists.");
            }
            catch (Exception ex)
            {
                // log error and return response
                _logger.LogError(ex, "Error creating book");
                return Response<int>.Failure("Error creating book", new List<string> { ex.Message });
            }
        }
    }
}
