using CleanArchitecture.Infrastructure.Persistence.Constants;
using CleanArchitecture.Infrastructure.Shared.Identity.Managers;
using CleanArchitecture.Infrastructure.Shared.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitecture.Admin.Infrastructure.Persistance.Seeders
{
    public class UsersSeeder
    {
        private static IEnumerable<UserDTO> DefaultUsers = new List<UserDTO>()
        {
            new UserDTO()
            {
                Email = "admin@email.com",
                Password = "Password12345!",
                Role = RoleConstants.AdministratorRole.Name,
                UserName = "admin"
            }
        };

        public static async Task SeedAsync(
            CustomUserManager<CustomIdentityUser> userManager, 
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            ILogger logger)
        {
            foreach (var user in DefaultUsers)
            {
                try
                {
                    var configuredPassword = configuration[$"Seed:Users:{user.UserName}:Password"];
                    var defaultPassword = configuration["Seed:DefaultPassword"];
                    var passwordToUse = !string.IsNullOrWhiteSpace(configuredPassword)
                        ? configuredPassword
                        : !string.IsNullOrWhiteSpace(defaultPassword)
                            ? defaultPassword
                            : user.Password;

                    if (string.IsNullOrWhiteSpace(passwordToUse))
                    {
                        logger.LogWarning("No seed password provided for user {User}. Skipping creation.", user.UserName);
                        continue;
                    }


                    // check if user exists
                    var savedUser = await userManager.FindByEmailAsync(user.Email);

                    if (savedUser != null)
                    {
                        // update existing user fields
                        savedUser.UserName = user.UserName;
                        savedUser.SecurityStamp = Guid.NewGuid().ToString();

                        var updateResult = await userManager.UpdateAsync(savedUser);
                        if (!updateResult.Succeeded)
                        {
                            logger.LogWarning("Failed to update user {Email}: {Errors}", user.Email, string.Join(", ", updateResult.Errors.Select(e => e.Description)));
                        }

                        // If password is configured and you want to enforce it, reset using token
                        if (!string.IsNullOrWhiteSpace(passwordToUse))
                        {
                            try
                            {
                                var token = await userManager.GeneratePasswordResetTokenAsync(savedUser);
                                var pwResult = await userManager.ResetPasswordAsync(savedUser, token, passwordToUse);
                                if (!pwResult.Succeeded)
                                {
                                    logger.LogWarning("Failed to reset password for user {Email}: {Errors}", user.Email, string.Join(", ", pwResult.Errors.Select(e => e.Description)));
                                }
                            }
                            catch (Exception ex)
                            {
                                logger.LogWarning(ex, "Error while attempting to reset password for existing user {Email}", user.Email);
                            }
                        }

                    }
                    else
                    {

                        // create user
                        var identityUser = new CustomIdentityUser
                        {
                            Email = user.Email,
                            UserName = user.UserName,
                            SecurityStamp = Guid.NewGuid().ToString()
                        };

                        // add user 
                        var result = await userManager.CreateAsync(identityUser, user.Password);

                        // add user to role
                        if (result.Succeeded)
                        {
                            // assign role (ensure role exists)
                            if (!string.IsNullOrWhiteSpace(user.Role))
                            {
                                var role = await roleManager.FindByNameAsync(user.Role);
                                if (role != null)
                                {
                                    await userManager.AddToRoleAsync(identityUser, user.Role);
                                }
                                else
                                {
                                    logger.LogWarning("Role {Role} does not exist when trying to assign to user {Email}", user.Role, user.Email);
                                }
                            }

                            // get saved user
                            savedUser = await userManager.FindByEmailAsync(user.Email);
                        }
                        else
                        {
                            logger.LogWarning("Failed to create user {Email}: {Errors}", user.Email, string.Join(", ", result.Errors.Select(e => e.Description)));
                        }
                    }


                    if (savedUser != null) 
                    { 
                        // get claims for this role
                        var identityRole = await roleManager.FindByNameAsync(user.Role);
                        var roleClaims = await roleManager.GetClaimsAsync(identityRole);

                        // add claims to users
                        if (roleClaims.Count() > 0)
                        {
                            var userClaims = await userManager.GetClaimsAsync(savedUser);

                            foreach (var claim in roleClaims)
                            {
                                var isUserClaimExists = userClaims.Any(c => c.Type == claim.Type);
                                if (isUserClaimExists == false)
                                {
                                    await userManager.AddClaimAsync(savedUser, claim);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError($"An error occurred seeding the database with user {user.Email}. Error: {ex.Message}");
                }
            }
        }

    }

    public class UserDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string UserName { get; set; }
    }
}
