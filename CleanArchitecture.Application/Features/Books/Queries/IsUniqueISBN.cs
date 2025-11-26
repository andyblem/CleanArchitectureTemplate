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
    public class IsUniqueISBN : IRequest<Response<bool>>
    {
        public string ISBN { get; set; }
    }

    public class IsUniqueISBNHandler : IRequestHandler<IsUniqueISBN, Response<bool>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger<IsUniqueISBNHandler> _logger;

        public IsUniqueISBNHandler(IApplicationDbContext dbContext, ILogger<IsUniqueISBNHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Response<bool>> Handle(IsUniqueISBN request, CancellationToken cancellationToken)
        {
            try
            {
                // check if ISBN is unique
                bool isUnique = !await _dbContext.Books
                    .AnyAsync(p => p.ISBN == request.ISBN);

                // return response
                return new Response<bool>(isUnique, isUnique ? "ISBN is unique." : "ISBN already exists.");
            }
            catch (Exception ex)
            {
                // log error
                _logger.LogError(ex, "Error checking unique ISBN.");

                // return failure response
                return new Response<bool>(ex.Message);
            }
        }
    }
}
