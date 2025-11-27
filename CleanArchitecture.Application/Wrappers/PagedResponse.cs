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
        public int TotalRecords { get; set; }


        public PagedResponse()
            : base() { }

        private PagedResponse(T data, int pageNumber, int pageSize, int totalRecords, string message = null)
        {
            // Set pagination properties
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalRecords = totalRecords;
            TotalPages = pageSize == 0 ? 0 : (int)Math.Ceiling(totalRecords / (double)pageSize);
            
            // Set response properties
            Data = data;
            Message = message;
            Succeeded = true;
            Errors = null;
        }

        private PagedResponse(string message, List<string> errors = null)
        {
            // Set failure properties
            Succeeded = false;
            Message = message;
            Errors = errors;
            
            // Set default pagination properties
            PageNumber = 0;
            PageSize = 0;
            TotalPages = 0;
            TotalRecords = 0;
            Data = default(T);
        }

        public static PagedResponse<T> Empty(int pageNumber, int pageSize, string message = "No records found")
        {
            return new PagedResponse<T>(default(T), pageNumber, pageSize, 0, message);
        }

        public static new PagedResponse<T> Failure(string message, List<string> errors = null)
        {
            return new PagedResponse<T>(message, errors);
        }

        public static PagedResponse<T> Success(T data, int pageNumber, int pageSize, int totalRecords, string message = "Records found")
        {
            return new PagedResponse<T>(data, pageNumber, pageSize, totalRecords, message);
        }
    }
}
