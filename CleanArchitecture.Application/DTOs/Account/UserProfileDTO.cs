using System;

namespace CleanArchitecture.Application.DTOs.Account
{
    public class UserProfileDTO
    {
        public string? UserId { get; init; }
        public string? UserName { get; init; }
        public string? Email { get; init; }

        public string? ProfilePicture { get; set; }
    }
}