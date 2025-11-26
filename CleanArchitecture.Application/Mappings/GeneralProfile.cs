using CleanArchitecture.Application.Features.Books.Queries.GetAllBooks;
using AutoMapper;
using CleanArchitecture.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Application.Features.Books.Commands;

namespace CleanArchitecture.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<Book, GetAllBooksViewModel>().ReverseMap();
            CreateMap<GetAllBooksQuery, GetAllBooksParameter>();
        }
    }
}
