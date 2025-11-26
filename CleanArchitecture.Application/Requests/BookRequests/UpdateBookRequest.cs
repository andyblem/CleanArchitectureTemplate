using AutoMapper;
using CleanArchitecture.Application.DTOs.Book;
using CleanArchitecture.Application.Features.Books.Commands;
using CleanArchitecture.Application.Wrappers;
using CleanArchitecture.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Requests.BookRequests
{
    public class UpdateBookRequest : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public UpdateBookDTO Book { get; set; }
    }

    public class UpdateBookRequestHandler : IRequestHandler<UpdateBookRequest, Response<int>>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public UpdateBookRequestHandler(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(UpdateBookRequest request, CancellationToken cancellationToken)
        {
            // update book
            var book = _mapper.Map<Book>(request.Book);
            var updateBookResult = await _mediator.Send(new UpdateBookCommand
            {
                Book = book
            }, 
            cancellationToken);

            // return response
            return updateBookResult;
        }
    }
}
