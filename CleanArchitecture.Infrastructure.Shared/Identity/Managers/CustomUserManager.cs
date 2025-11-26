using CleanArchitecture.Infrastructure.Shared.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Shared.Identity.Managers
{
    public class CustomUserManager<T> : UserManager<CustomIdentityUser> where T : CustomIdentityUser
    {
        public CustomUserManager(IUserStore<CustomIdentityUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<CustomIdentityUser> passwordHasher,
            IEnumerable<IUserValidator<CustomIdentityUser>> userValidators,
            IEnumerable<IPasswordValidator<CustomIdentityUser>> passwordValidators,
            ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<CustomUserManager<CustomIdentityUser>> logger)
            : base(store,
                  optionsAccessor,
                  passwordHasher,
                  userValidators,
                  passwordValidators,
                  keyNormalizer,
                  errors,
                  services,
                  logger)
        {
        }
    }
}
