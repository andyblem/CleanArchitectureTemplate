using CleanArchitecture.Application.Filters;
using CleanArchitecture.Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Features.Books.Queries.GetAllBooks
{
    public class GetAllBooksQuery : IRequest<PagedResponse<IEnumerable<GetAllBooksViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    //public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, PagedResponse<IEnumerable<GetAllBooksViewModel>>>
    //{
    //    private readonly IBookRepositoryAsync _bookRepository;
    //    private readonly IMapper _mapper;
    //    public GetAllBooksQueryHandler(IBookRepositoryAsync bookRepository, IMapper mapper)
    //    {
    //        _bookRepository = bookRepository;
    //        _mapper = mapper;
    //    }

    //    public async Task<PagedResponse<IEnumerable<GetAllBooksViewModel>>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
    //    {
    //        var validFilter = _mapper.Map<GetAllBooksParameter>(request);
    //        var books = await _bookRepository.GetPagedReponseAsync(validFilter.PageNumber,validFilter.PageSize);
    //        var booksViewModel = _mapper.Map<IEnumerable<GetAllBooksViewModel>>(books);
    //        return new PagedResponse<IEnumerable<GetAllBooksViewModel>>(booksViewModel, validFilter.PageNumber, validFilter.PageSize);           
    //    }
    //}
}
