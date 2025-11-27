using AutoMapper;
using CleanArchitecture.Application.DTOs.Book;
using CleanArchitecture.Application.Features.Books.Commands;
using CleanArchitecture.Application.Features.Books.Queries;
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
    public class CreateBookRequest : IRequest<Response<int>>
    {
        public CreateBookDTO Book { get; init; }
    }

    public class CreateBookRequestHandler : IRequestHandler<CreateBookRequest, Response<int>>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public CreateBookRequestHandler(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<Response<int>> Handle(CreateBookRequest request, CancellationToken cancellationToken)
        {
            // check if ISBN is unique
            var isUniquesISBNResult = await _mediator.Send(new IsUniqueISBNQuery { ISBN = request.Book.ISBN }, cancellationToken);
            if(isUniquesISBNResult.Succeeded == false || (isUniquesISBNResult.Succeeded == true && isUniquesISBNResult.Data == false))
            {
                return Response<int>.Failure(isUniquesISBNResult.Message);
            }

            // create book
            var book = _mapper.Map<Book>(request.Book);
            var createBookCommandResult = await _mediator.Send(new CreateBookCommand { Book = book }, cancellationToken);

            // return result
            return createBookCommandResult;
        }
    }
}
