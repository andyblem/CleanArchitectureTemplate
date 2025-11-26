using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.Features.Books.Queries.GetAllBooks
{
    public class GetAllBooksViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string Summary { get; set; }
        public decimal Price { get; set; }
    }
}
