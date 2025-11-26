using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.IdentityProvider.DTOs
{
    public class JwtSettingsDTO
    {
        public string Audience { get; init; } = null!;
        public string DurationInMinutes { get; init; } = null!;
        public string Issuer { get; init; } = null!;
        public string Key { get; init; } = null!;
    }
}
