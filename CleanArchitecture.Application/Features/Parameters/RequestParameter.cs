using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.Features.Parameters
{
    public class RequestParameter
    {
        public const int _defaultPageSize = 10;
        public const int _maxPageSize = 100;

        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public string SearchString { get; init; }
        public string SortFilter { get; init; }
        public string SortOrder { get; init; }


        public RequestParameter()
        {
            PageNumber = 1;
            PageSize = _defaultPageSize;
        }

        public RequestParameter(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize > _maxPageSize ? _maxPageSize : pageSize;
        }
    }
}
