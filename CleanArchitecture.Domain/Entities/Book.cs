using CleanArchitecture.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Domain.Entities
{
    public class Book : BaseEntity
    {
        public decimal Price { get; init; }

        public string ISBN { get; init; }
        public string Summary { get; init; }
        public string Title { get; init; }
    }
}
