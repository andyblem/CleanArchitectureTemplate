using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Application.Wrappers;
using CleanArchitecture.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Features.Books.Queries.GetBookById
{
    public class GetBookByIdQuery : IRequest<Response<Book>>
    {
        public int Id { get; set; }
        //public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, Response<Book>>
        //{
        //    private readonly IBookRepositoryAsync _bookRepository;
        //    public GetBookByIdQueryHandler(IBookRepositoryAsync bookRepository)
        //    {
        //        _bookRepository = bookRepository;
        //    }
        //    public async Task<Response<Book>> Handle(GetBookByIdQuery query, CancellationToken cancellationToken)
        //    {
        //        var book = await _bookRepository.GetByIdAsync(query.Id);
        //        if (book == null) throw new ApiException($"Book Not Found.");
        //        return new Response<Book>(book);
        //    }
        //}
    }
}
