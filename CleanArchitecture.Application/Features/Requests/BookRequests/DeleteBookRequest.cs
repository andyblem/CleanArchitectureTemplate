using CleanArchitecture.Application.Features.CQRS.Books.Commands;
using CleanArchitecture.Application.Features.CQRS.Books.Queries;
using CleanArchitecture.Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Features.Requests.BookRequests
{
    public class DeleteBookRequest : IRequest<Response<int>>
    {
        public int Id { get; set; }
    }

    public class DeleteBookRequestHandler : IRequestHandler<DeleteBookRequest, Response<int>>
    {
        private readonly IMediator _mediator;

        public DeleteBookRequestHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Response<int>> Handle(DeleteBookRequest request, CancellationToken cancellationToken)
        {
            // check if book exists
            var bookExistsResult = await _mediator.Send(new CheckIfBookExistsQuery { Id = request.Id }, cancellationToken);
            if(bookExistsResult.Succeeded == false || bookExistsResult.Data == false)
            {
                return new Response<int>
                {
                    Succeeded = false,
                    Message = bookExistsResult.Message,
                    Data = 0
                };
            }

            // delete book
            var deleteBookResult = await _mediator.Send(new DeleteBookCommand { Id = request.Id }, cancellationToken);
            return deleteBookResult;
        }
    }
}
