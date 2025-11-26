using CleanArchitecture.Application.DTOs.Book;
using CleanArchitecture.Application.Features.Books.Queries.GetBookById;
using CleanArchitecture.Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Requests.BookRequests
{
    public class GetBookRequest : IRequest<Response<BookDTO>>
    {
        public int Id { get; set; }
    }

    public class GetBookRequestHandler : IRequestHandler<GetBookRequest, Response<BookDTO>>
    {
        private readonly IMediator _mediator;

        public GetBookRequestHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Response<BookDTO>> Handle(GetBookRequest request, CancellationToken cancellationToken)
        {
            // send query to get book by id
            var bookResult = await _mediator.Send(new GetBookByIdQuery()
            {
                Id = request.Id
            }, 
            cancellationToken);

            // return response
            return bookResult;
        }
    }
}
