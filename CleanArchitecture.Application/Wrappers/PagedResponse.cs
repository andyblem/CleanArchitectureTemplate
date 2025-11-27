using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.Wrappers
{
    public class PagedResponse<T> : Response<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public object TotalSize { get; set; }

        public PagedResponse(T data, int pageNumber, int pageSize, int totalSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.TotalPages = pageSize == 0 ? 0 : (int)Math.Ceiling(totalSize / (double)pageSize);
            this.Data = data;
            this.Message = null;
            this.Succeeded = true;
            this.Errors = null;
        }
    }
}
