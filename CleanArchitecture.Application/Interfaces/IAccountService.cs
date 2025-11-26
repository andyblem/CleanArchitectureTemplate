using CleanArchitecture.Application.DTOs.Account;
using CleanArchitecture.Application.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Interfaces
{
    public interface IAccountService
    {
        Task<Response<AuthenticationResponseDTO>> AuthenticateAsync(AuthenticationRequestDTO request);
        Task<Response<UserProfileDTO>> GetUserProfileAsync(string userId);
    }
}
