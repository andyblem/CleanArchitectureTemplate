using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.DTOs.Email
{
    public class EmailRequestDTO
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string From { get; set; }
    }
}
