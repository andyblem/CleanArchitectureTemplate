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

namespace CleanArchitecture.Application.Features.Books.Queries
{
    public class CheckIfBookExistsQuery : IRequest<Response<bool>>
    {
        public int Id { get; set; }
    }

    public class CheckIfBookExistsQueryHandler : IRequestHandler<CheckIfBookExistsQuery, Response<bool>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger<CheckIfBookExistsQueryHandler> _logger;

        public CheckIfBookExistsQueryHandler(IApplicationDbContext dbContext, ILogger<CheckIfBookExistsQueryHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Response<bool>> Handle(CheckIfBookExistsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // check if exists
                bool exists = await _dbContext.Books
                    .AnyAsync(b => b.Id == request.Id);

                return Response<bool>.Success(exists);
            }
            catch (Exception ex)
            {
                // log error and return failure response
                _logger.LogError(ex, "Error checking if book exists with Id {BookId}", request.Id);
                return Response<bool>.Failure("Error checking if book exists.", new List<string> { ex.Message });
            }
        }
    }
}
