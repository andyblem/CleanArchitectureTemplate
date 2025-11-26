using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.DTOs.Book
{
    public sealed class BookDTO
    {
        public decimal Price { get; init; }

        public int Id { get; init; }

        public string ISBN { get; init; }
        public string Summary { get; set; }
        public string Title { get; set; }
    }
}
