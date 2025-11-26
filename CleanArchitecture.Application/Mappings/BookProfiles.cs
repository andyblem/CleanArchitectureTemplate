using AutoMapper;
using CleanArchitecture.Application.DTOs.Book;
using CleanArchitecture.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Mappings
{
    public class BookProfiles : Profile
    {
        public BookProfiles()
        {
            CreateMap<CreateBookDTO, Book>();
            CreateMap<UpdateBookDTO, Book>();
        }
    }
}
