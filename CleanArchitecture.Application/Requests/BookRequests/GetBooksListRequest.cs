using CleanArchitecture.Application.DTOs.Book;
using CleanArchitecture.Application.Features.Books.Queries;
using CleanArchitecture.Application.Parameters.Book;
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
    public class GetBooksListRequest : IRequest<PagedResponse<IEnumerable<BookListItemDTO>>>
    {
        public GetBooksListParameter BookParameters { get; set; }
    }

    public class GetBooksListRequestHandler : IRequestHandler<GetBooksListRequest, PagedResponse<IEnumerable<BookListItemDTO>>>
    {
        private readonly IMediator _mediator;

        public GetBooksListRequestHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<PagedResponse<IEnumerable<BookListItemDTO>>> Handle(GetBooksListRequest request, CancellationToken cancellationToken)
        {
            // get books
            var getBooksListResult = await _mediator.Send(new GetBooksListQuery() 
            { 
                Parameters = request.BookParameters 
            }, 
            cancellationToken);

            // return result
            return getBooksListResult;
        }
    }
}
