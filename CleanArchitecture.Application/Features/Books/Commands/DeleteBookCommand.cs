using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Features.Books.Commands
{
    public class DeleteBookCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        //public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, Response<int>>
        //{
        //    //private readonly IBookRepositoryAsync _bookRepository;
        //    //public DeleteBookCommandHandler(IBookRepositoryAsync bookRepository)
        //    //{
        //    //    _bookRepository = bookRepository;
        //    //}

        //    public async Task<Response<int>> Handle(DeleteBookCommand command, CancellationToken cancellationToken)
        //    {
        //        //var book = await _bookRepository.GetByIdAsync(command.Id);
        //        //if (book == null) throw new ApiException($"Book Not Found.");
        //        //await _bookRepository.DeleteAsync(book);
        //        //return new Response<int>(book.Id);
        //    }
        //}
    }
}
