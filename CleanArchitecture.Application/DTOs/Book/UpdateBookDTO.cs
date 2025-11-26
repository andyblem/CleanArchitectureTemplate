using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.DTOs.Book
{
    public sealed class UpdateBookDTO
    {
        public decimal Price { get; init; }

        public int Id { get; init; }

        public string Summary { get; init; }
        public string Title { get; init; }
    }
}
