using CleanArchitecture.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CleanArchitecture.Presentation.Web.API.Services
{
    public class AuthenticatedUserService : IAuthenticatedUserService
    {
        public string UserId { get; }

        private string _email;
        public string Email
        {
            get => string.IsNullOrEmpty(_email) ? "System" : _email;
            set => _email = value ?? "System";
        }


        public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
