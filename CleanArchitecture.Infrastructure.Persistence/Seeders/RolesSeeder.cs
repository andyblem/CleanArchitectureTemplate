using CleanArchitecture.Infrastructure.Persistence.Constants;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Persistence.Seeders
{
    public class RolesSeeder
    {
        private static IEnumerable<RoleDTO> _roles = new List<RoleDTO>()
        {
            new RoleDTO()
            {
                Name = RoleConstants.AdministratorRole.Name,
                NormalizedName = RoleConstants.AdministratorRole.NormalizedName,
                Claims = new List<ClaimDTO>()
                {
                    // Book
                    new ClaimDTO() { ClaimType = "permission", ClaimValue = "create:books" },
                    new ClaimDTO() { ClaimType = "permission", ClaimValue = "delete:books" },
                    new ClaimDTO() { ClaimType = "permission", ClaimValue = "read:books" },
                    new ClaimDTO() { ClaimType = "permission", ClaimValue = "update:books" },
                }
            }
        };

        public static IEnumerable<RoleDTO> Roles { get => _roles; }
        public static IEnumerable<ClaimDTO> Claims { get => _roles.SelectMany(r => r.Claims); }

        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in _roles)
            {
                // variable to store rol
                IdentityRole roleIdentity = null;

                // check if role exists
                var isRoleExists = await roleManager.RoleExistsAsync(role.Name);

                // if doesn't exist create it first
                if (isRoleExists == false)
                {
                    // create role object
                    roleIdentity = new IdentityRole()
                    {
                        Name = role.Name,
                        NormalizedName = role.NormalizedName,
                    };

                    // save object
                    await roleManager.CreateAsync(roleIdentity);
                }

                // get role
                roleIdentity = await roleManager.FindByNameAsync(role.Name);


                // create role claims if role is available
                if (roleIdentity != null)
                {
                    // get role claims
                    var roleClaims = await roleManager.GetClaimsAsync(roleIdentity);

                    // create claims for role
                    foreach (var claim in role.Claims)
                    {
                        // check if claim exists
                        var isClaimExists = roleClaims.Any(rC => rC.Type == claim.ClaimType);

                        if (isClaimExists == false)
                        {
                            // add claim
                            var identityClaim = new Claim(claim.ClaimType, claim.ClaimValue);
                            await roleManager.AddClaimAsync(roleIdentity, identityClaim);
                        }
                    }
                }
            }
        }
    }

    public class ClaimDTO
    {
        public string ClaimValue { get; set; }
        public string ClaimType { get; set; }
    }

    public class RoleDTO
    {
        public string Name { get; set; }
        public string NormalizedName { get; set; }

        public IEnumerable<ClaimDTO> Claims { get; set; }
    }
}
