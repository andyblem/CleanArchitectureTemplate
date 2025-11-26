using CleanArchitecture.Application.DTOs.Account;
using CleanArchitecture.Application.Interfaces;
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
                return new Response<AuthenticationResponseDTO>("User not found");
            }
            else
            {
                // check user password
                var userPasswordValid = await _customUserManager.CheckPasswordAsync(user, request.Password);
                if (userPasswordValid == false)
                    return new Response<AuthenticationResponseDTO>("User password invalid");

                // validate JWT settings
                if (_jwtSettings == null)
                    return new Response<AuthenticationResponseDTO>("JWT settings not configured");

                if (string.IsNullOrWhiteSpace(_jwtSettings.Key))
                    return new Response<AuthenticationResponseDTO>("JWT signing key is not configured");

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
                return new Response<AuthenticationResponseDTO>(response, string.Empty);
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
                return new Response<UserProfileDTO>("User not found");
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
                return new Response<UserProfileDTO>(userProfile);
            }
        }

        //public async Task<Response<string>> UpdateProfileAsync(string userId)
        //{

        //    // check if user exists first
        //    bool isUserAvailable = await _mediator.Send(new CheckIfUserExistsByIdQuery()
        //    {
        //        UserId = request.UserId,
        //    });

        //    if (isUserAvailable == true)
        //    {
        //        // make user id field is set
        //        request.UserProfileInformation.Id = request.UserId;

        //        // map dto to model
        //        var user = _mapper.Map<CustomIdentityUser>(request.UserProfileInformation);

        //        // update data to db
        //        var updateUserResult = await _mediator.Send(new UpdateUserProfileInformationCommand()
        //        {
        //            User = user,
        //        });

        //        // return result based on update result
        //        if (updateUserResult == true)
        //        {
        //            return new Response<string>("User updated successfully", string.Empty);
        //        }
        //        else
        //        {
        //            // return result
        //            return new Response<string>("Updating user failed");
        //        }
        //    }
        //    else
        //    {
        //        // return result
        //        return new Response<string>("User not found");
        //    }

        //}

        //public class UpdateSecurityInformationRequestHandler : IRequestHandler<UpdateSecurityInformationRequest, Response<string>>
        //{
        //    private readonly IMediator _mediator;
        //    private readonly CustomUserManager<CustomIdentityUser> _customUserManager;

        //    public UpdateSecurityInformationRequestHandler(IMediator mediator, CustomUserManager<CustomIdentityUser> customUserManager)
        //    {
        //        _mediator = mediator;
        //        _customUserManager = customUserManager;
        //    }

        //    public async Task<Response<string>> Handle(UpdateSecurityInformationRequest request, CancellationToken cancellationToken)
        //    {
        //        // check if user exists first
        //        bool isUserAvailable = await _mediator.Send(new CheckIfUserExistsByIdQuery()
        //        {
        //            UserId = request.UserId,
        //        });

        //        if (isUserAvailable == true)
        //        {
        //            // map dto to model
        //            var user = await _customUserManager.FindByIdAsync(request.UserId);

        //            // update data to db
        //            var updateUserResult = await _customUserManager.ChangePasswordAsync(
        //                user,
        //                request.UserSecurityInformation.CurrentPassword,
        //                request.UserSecurityInformation.NewPassword);

        //            // return result based on update result
        //            if (updateUserResult.Succeeded == true)
        //            {
        //                return new Response<string>("User updated successfully", string.Empty);
        //            }
        //            else
        //            {
        //                // return result
        //                return new Response<string>(updateUserResult.Errors.ToString());
        //            }
        //        }
        //        else
        //        {
        //            // return result
        //            return new Response<string>("User not found");
        //        }
        //    }
        //}

        //public async Task<Response<string>> Handle(UploadProfilePictureRequest request, CancellationToken cancellationToken)
        //{
        //    // create path to directory
        //    string pathToDirectory = FileService.GeneratePathToLocation(_fileSettings.ProfilePicturesLocation);

        //    // create file name
        //    string fileName = string.Format("P{0}{1}",
        //        request.ProfilePicture.Id,
        //        Path.GetExtension(request.ProfilePicture.ProfilePicture.FileName).ToLowerInvariant());

        //    // save file to storage
        //    await FileService.SaveFileToPathAsync(fileName, pathToDirectory, request.ProfilePicture.ProfilePicture);

        //    // get saved file
        //    var imageBytes = await FileService.GetFileAsByteArray(fileName, pathToDirectory);

        //    // convert image to base 64
        //    string imageString = Convert.ToBase64String(imageBytes);

        //    // save record for user
        //    var user = await _userManager.FindByIdAsync(request.ProfilePicture.Id);
        //    user.ProfilePicture = fileName;
        //    var saveProfilePictureNameResult = await _userManager.UpdateAsync(user);

        //    // return result based on identity result
        //    if (saveProfilePictureNameResult.Succeeded)
        //    {
        //        // return result
        //        return new Response<string>(imageString, "Image saved successfuly");
        //    }
        //    else
        //    {
        //        // return result
        //        return new Response<string>("Uploading image failed");
        //    }
        //}
    }
}
