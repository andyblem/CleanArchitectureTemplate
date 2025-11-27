using CleanArchitecture.Application.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.Parameters.Book
{
    public class GetBooksListParameter : RequestParameter
    {

        public GetBooksListParameter()
            : base()
        {
        }

        public GetBooksListParameter(int pageNumber, int pageSize)
            : base(pageNumber, pageSize)
        {
        }
    }
}
