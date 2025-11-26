using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Shared.Identity.Models
{

    public class CustomIdentityUser : IdentityUser
    {
        public string? ProfilePicture { get; set; }
    }
}
