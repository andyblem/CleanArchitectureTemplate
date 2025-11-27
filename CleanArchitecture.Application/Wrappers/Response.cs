using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.Wrappers
{
    public class Response<T>
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public T Data { get; set; }


        public Response()
        {
            Succeeded = false; // Initialize to false by default
        }
        
        
        public static Response<T> Failure(string message, List<string> errors = null)
        {
            return new Response<T>
            {
                Succeeded = false,
                Message = message,
                Errors = errors
            };
        }

        public static Response<T> Success(T data, string message = null)
        {
            return new Response<T>
            {
                Succeeded = true,
                Message = message,
                Data = data
            };
        }
    }
}
