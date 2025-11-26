using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CleanArchitecture.Application.DTOs.Account
{
    public class AuthenticationResponseDTO
    {
        public int ExpiresIn { get; init; }

        public string UserId { get; init; }
        public string UserName { get; init; }
        public string Email { get; init; }
        public string? ProfilePicture { get; init; }
        public string TokenId { get; init; }
        public string TokenType { get; init; } = "Bearer";
        public string AccessToken { get; init; }

        public DateTime ExpiresAt { get; init; }
        public DateTime IssuedAt { get; init; } = DateTime.UtcNow;

        [JsonIgnore]
        public string RefreshToken { get; init; }
    }
}
