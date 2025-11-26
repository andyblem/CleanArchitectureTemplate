using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.DTOs.Book
{
    public class CreateBookDTO
    {
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string Summary { get; set; }
        public decimal Price { get; set; }
    }
}
