using CleanArchitecture.Application.DTOs.Email;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequestDTO request);
    }
}
