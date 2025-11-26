using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CleanArchitecture.Application.DTOs.Account;
using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Presentation.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [Authorize]
        [HttpGet]
        [Route("claims")]
        public object Claims()
        {
            return User.Claims.Select(c => new
            {
                Type = c.Type,
                Value = c.Value
            });
        }

        [Authorize]
        [HttpGet]
        [Route("userid")]
        public object UserId()
        {
            // The user's ID is available in the NameIdentifier claim
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            return new
            {
                UserId = userId
            };
        }

        [Authorize]
        [HttpGet]
        [Route("userinfo")]
        public async Task<object> UserInformation()
        {
            // Retrieve the access_token claim which we saved in the OnTokenValidated event
            string accessToken = User.Claims.FirstOrDefault(c => c.Type == "access_token").Value;

            // If we have an access_token, then retrieve the user's information
            if (!string.IsNullOrEmpty(accessToken))
            {
                return await _accountService.GetUserProfileAsync(accessToken);
            }

            return null;
        }

        [HttpGet("private-scoped")]
        [Authorize("read:messages")]
        public IActionResult Scoped()
        {
            return Ok(new
            {
                Message = "Hello from a private endpoint! You need to be authenticated and have a scope of read:messages to see this."
            });
        }

        [HttpPost]
        [Route("auth")]
        public async Task<IActionResult> Authenticate(AuthenticationRequestDTO request)
        {
            return Ok(await _accountService.AuthenticateAsync(request));

        }
    }
}