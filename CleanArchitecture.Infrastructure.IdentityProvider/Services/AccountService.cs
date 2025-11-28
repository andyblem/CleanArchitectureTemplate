using AutoMapper;
using CleanArchitecture.Application.DTOs.Account;
using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Application.Features.Validators.AccountValidators;
using CleanArchitecture.Application.Wrappers;
using CleanArchitecture.Infrastructure.IdentityProvider.DTOs;
using CleanArchitecture.Infrastructure.Shared.DTOs;
using CleanArchitecture.Infrastructure.Shared.Identity.Managers;
using CleanArchitecture.Infrastructure.Shared.Identity.Models;
using CleanArchitecture.Infrastructure.Shared.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.IdentityProvider.Services
{
    public class AccountService : IAccountService
    {
        private readonly FileSettingsDTO _fileSettings;
        private readonly JwtSettingsDTO _jwtSettings;
        private readonly IFileService _fileService;

        private readonly CustomUserManager<CustomIdentityUser> _customUserManager;
    
        private readonly ILogger<AccountService> _logger;


        public AccountService(IOptions<FileSettingsDTO> options, IOptions<JwtSettingsDTO> jwtSettings, CustomUserManager<CustomIdentityUser> userManager, IFileService fileService, ILogger<AccountService> logger)
        {
            _fileSettings = options.Value;
            _jwtSettings = jwtSettings.Value;
            _customUserManager = userManager;
            _fileService = fileService;
            _logger = logger;
        }

        public async Task<Response<AuthenticationResponseDTO>> AuthenticateAsync(AuthenticationRequestDTO request)
        {
            //check the user
            var user = await _customUserManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                // return result
                return Response<AuthenticationResponseDTO>.Failure("User not found");
            }
            else
            {
                // check user password
                var userPasswordValid = await _customUserManager.CheckPasswordAsync(user, request.Password);
                if (userPasswordValid == false)
                    return Response<AuthenticationResponseDTO>.Failure("User password invalid");

                // validate JWT settings
                if (_jwtSettings == null)
                    return Response<AuthenticationResponseDTO>.Failure("JWT settings not configured");

                if (string.IsNullOrWhiteSpace(_jwtSettings.Key))
                    return Response<AuthenticationResponseDTO>.Failure("JWT signing key is not configured");

                // parse duration safely
                if (!int.TryParse(_jwtSettings.DurationInMinutes, out var durationInMinutes))
                {
                    durationInMinutes = 60; // fallback default
                }


                // get profile picture
                string profilePicture = string.Empty;
                try
                {
                    // get imageand convert to byte array
                    string pathToDirectory = _fileService.GeneratePathToLocation(_fileSettings.ProfilePicturesLocation);
                    byte[] imageBytes = await _fileService.GetFileAsByteArrayAsync(user.ProfilePicture, pathToDirectory);

                    // convert to base64 string and save to
                    profilePicture = Convert.ToBase64String(imageBytes);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error loading profile picture for user {UserId} during AuthenticateAsync", user?.Id);
                }

                // prepare token configs
                var audience = _jwtSettings.Audience;
                var issuer = _jwtSettings.Issuer;
                var issuerKeyBytes = Encoding.ASCII.GetBytes(_jwtSettings.Key);
                var signInCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(issuerKeyBytes),
                    SecurityAlgorithms.HmacSha512Signature
                );

                // create claims for user
                var jti = Guid.NewGuid().ToString();
                var subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),                              //user id
                    new Claim(JwtRegisteredClaimNames.Email, user.UserName ?? string.Empty),    //holds user email
                    new Claim(JwtRegisteredClaimNames.Jti, jti),                                //unique identifier for our jwt
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName ?? string.Empty),      //holds user identity
                    new Claim("LoggedOn", DateTime.UtcNow.ToString()),                          //user log in moment
                });

                //get user claims and them to subject
                var claims = await _customUserManager.GetClaimsAsync(user);
                foreach (Claim claim in claims)
                    subject.AddClaim(claim);

                // calculate token expiry time
                var expires = DateTime.UtcNow.AddMinutes(durationInMinutes);

                // create token descriptor
                var tokenDiscriptor = new SecurityTokenDescriptor()
                {
                    Audience = audience,
                    Expires = expires,
                    Issuer = issuer,
                    SigningCredentials = signInCredentials,
                    Subject = subject,
                };

                // create token
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDiscriptor);
                var jwtToken = tokenHandler.WriteToken(token);

                // create response
                var response = new AuthenticationResponseDTO()
                {
                    ExpiresAt = token.ValidTo,
                    ExpiresIn = durationInMinutes,

                    AccessToken = jwtToken,
                    Email = user.UserName ?? string.Empty,
                    UserId = user.Id,
                    UserName = user.UserName,
                    ProfilePicture = profilePicture,
                    TokenId = jti,

                };

                // return result
                return Response<AuthenticationResponseDTO>.Success(response, string.Empty);
            }
        }

        public async Task<Response<UserProfileDTO>> GetUserProfileAsync(string userId)
        {
            // get user
            var userProfile = await _customUserManager.Users
                .Where(u => u.Id == userId)
                .Select(u => new UserProfileDTO()
                {
                    Email = u.Email,
                    ProfilePicture = u.ProfilePicture,
                    UserName = u.UserName
                })
                .FirstOrDefaultAsync();

            if (userProfile == null)
            {
                // return result
                return Response<UserProfileDTO>.Failure("User not found");
            }
            else
            {
                // load profile picture from storage
                if (userProfile.ProfilePicture != null)
                {
                    try
                    {
                        // create path to directory
                        string pathToDirectory = _fileService.GeneratePathToLocation(_fileSettings.ProfilePicturesLocation);
                        byte[] imageBytes = await _fileService.GetFileAsByteArrayAsync(userProfile.ProfilePicture, pathToDirectory);

                        // convert to base64 string and save to
                        userProfile.ProfilePicture = Convert.ToBase64String(imageBytes);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error loading profile picture for user {UserId}", userId);
                    }
                }

                // return result
                return Response<UserProfileDTO>.Success(userProfile);
            }
        }

        public async Task<Response<string>> UpdateProfileAsync(string userId, UpdateUserProfileDTO userProfileDTO)
        {
            try
            {
                // Validate input using FluentValidation
                var validator = new UpdateUserProfileValidator();
                var validationResult = await validator.ValidateAsync(userProfileDTO);

                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    return Response<string>.Failure("Validation failed", errors);
                }

                // Validate user ID
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return Response<string>.Failure("User ID is required");
                }

                // Get the existing user
                var existingUser = await _customUserManager.FindByIdAsync(userId);

                if (existingUser == null)
                {
                    return Response<string>.Failure("User not found");
                }

                // Update user properties
                existingUser.UserName = userProfileDTO.UserName ?? existingUser.UserName;
                existingUser.Email = userProfileDTO.Email ?? existingUser.Email;
                existingUser.NormalizedUserName = userProfileDTO.UserName?.ToUpper() ?? existingUser.NormalizedUserName;
                existingUser.NormalizedEmail = userProfileDTO.Email?.ToUpper() ?? existingUser.NormalizedEmail;

                // Update the user using CustomUserManager
                var updateResult = await _customUserManager.UpdateAsync(existingUser);

                if (updateResult.Succeeded)
                {
                    _logger.LogInformation("Profile updated successfully for user {UserId}", userId);
                    return Response<string>.Success("Profile updated successfully", "User profile updated successfully");
                }
                else
                {
                    var errors = updateResult.Errors.Select(e => e.Description).ToList();
                    _logger.LogWarning("Failed to update profile for user {UserId}. Errors: {Errors}", userId, string.Join(", ", errors));

                    return Response<string>.Failure("Failed to update profile", errors);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating profile for user {UserId}", userId);
                return Response<string>.Failure("An error occurred while updating the profile");
            }
        }

        public async Task<Response<string>> UpdateSecurityInformationAsync(string userId, UpdateUserSecurityDTO userSecurityDTO)
        {
            try
            {
                // Validate input using FluentValidation
                var validator = new UpdateUserSecurityValidator();
                var validationResult = await validator.ValidateAsync(userSecurityDTO);

                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    return Response<string>.Failure("Validation failed", errors);
                }

                // Validate user ID
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return Response<string>.Failure("User ID is required");
                }

                // Get the existing user directly using CustomUserManager
                var existingUser = await _customUserManager.FindByIdAsync(userId);

                if (existingUser == null)
                {
                    return Response<string>.Failure("User not found");
                }

                // Update user password using CustomUserManager
                var changePasswordResult = await _customUserManager.ChangePasswordAsync(
                    existingUser,
                    userSecurityDTO.CurrentPassword,
                    userSecurityDTO.NewPassword);

                if (changePasswordResult.Succeeded)
                {
                    _logger.LogInformation("Password changed successfully for user {UserId}", userId);
                    return Response<string>.Success("Password updated successfully", "User password changed successfully");
                }
                else
                {
                    var errors = changePasswordResult.Errors.Select(e => e.Description).ToList();
                    _logger.LogWarning("Failed to change password for user {UserId}. Errors: {Errors}", userId, string.Join(", ", errors));

                    return Response<string>.Failure("Failed to change password", errors);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing password for user {UserId}", userId);
                return Response<string>.Failure("An error occurred while changing the password");
            }
        }

        public async Task<Response<string>> UploadProfilePictureAsync(string userId, UploadProfilePictureDTO uploadProfilePictureDTO)
        {
            try
            {
                // Validate input using FluentValidation
                var validator = new UploadProfilePictureValidator();
                var validationResult = await validator.ValidateAsync(uploadProfilePictureDTO);

                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    return Response<string>.Failure("Validation failed", errors);
                }

                // Validate user ID
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return Response<string>.Failure("User ID is required");
                }

                // Get the existing user
                var existingUser = await _customUserManager.FindByIdAsync(userId);
                if (existingUser == null)
                {
                    return Response<string>.Failure("User not found");
                }

                // Create path to directory
                string pathToDirectory = _fileService.GeneratePathToLocation(_fileSettings.ProfilePicturesLocation);

                // Create unique file name to prevent conflicts
                string fileExtension = Path.GetExtension(uploadProfilePictureDTO.ProfilePicture.FileName).ToLowerInvariant();
                string fileName = string.Format("P{0}{1}", userId, fileExtension);

                // Save file to storage
                await _fileService.SaveFileAsync(fileName, pathToDirectory, uploadProfilePictureDTO.ProfilePicture);

                // Get saved file and convert to base64
                var imageBytes = await _fileService.GetFileAsByteArrayAsync(fileName, pathToDirectory);
                string imageString = Convert.ToBase64String(imageBytes);

                // Update user's profile picture reference
                existingUser.ProfilePicture = fileName;
                var updateResult = await _customUserManager.UpdateAsync(existingUser);

                if (updateResult.Succeeded)
                {
                    _logger.LogInformation("Profile picture uploaded successfully for user {UserId}", userId);
                    return Response<string>.Success(imageString, "Profile picture uploaded successfully");
                }
                else
                {
                    // If user update failed, try to clean up the uploaded file
                    try
                    {
                        await _fileService.DeleteFileAsync(fileName, pathToDirectory);
                    }
                    catch (Exception cleanupEx)
                    {
                        _logger.LogError(cleanupEx, "Failed to cleanup uploaded file {FileName} after user update failure for user {UserId}", fileName, userId);
                    }

                    var errors = updateResult.Errors.Select(e => e.Description).ToList();
                    _logger.LogWarning("Failed to update user profile picture reference for user {UserId}. Errors: {Errors}", userId, string.Join(", ", errors));

                    return Response<string>.Failure("Failed to save profile picture reference", errors);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading profile picture for user {UserId}", userId);
                return Response<string>.Failure("An error occurred while uploading the profile picture");
            }
        }
    }
}
