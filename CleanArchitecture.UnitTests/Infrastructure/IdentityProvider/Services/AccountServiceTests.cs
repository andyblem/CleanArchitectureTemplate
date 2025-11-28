using CleanArchitecture.Application.DTOs.Account;
using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Application.Wrappers;
using CleanArchitecture.Infrastructure.IdentityProvider.DTOs;
using CleanArchitecture.Infrastructure.IdentityProvider.Services;
using CleanArchitecture.Infrastructure.Shared.DTOs;
using CleanArchitecture.Infrastructure.Shared.Identity.Managers;
using CleanArchitecture.Infrastructure.Shared.Identity.Models;
using CleanArchitecture.Infrastructure.Shared.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CleanArchitecture.UnitTests.Infrastructure.IdentityProvider.Services
{
    public class AccountServiceTests
    {
        private readonly Mock<CustomUserManager<CustomIdentityUser>> _mockUserManager;
        private readonly Mock<IFileService> _mockFileService;
        private readonly Mock<ILogger<AccountService>> _mockLogger;
        private readonly Mock<IOptions<FileSettingsDTO>> _mockFileOptions;
        private readonly Mock<IOptions<JwtSettingsDTO>> _mockJwtOptions;
        private readonly AccountService _accountService;

        public AccountServiceTests()
        {
            _mockUserManager = new Mock<CustomUserManager<CustomIdentityUser>>(
                Mock.Of<IUserStore<CustomIdentityUser>>(), null, null, null, null, null, null, null, null);
            _mockFileService = new Mock<IFileService>();
            _mockLogger = new Mock<ILogger<AccountService>>();
            _mockFileOptions = new Mock<IOptions<FileSettingsDTO>>();
            _mockJwtOptions = new Mock<IOptions<JwtSettingsDTO>>();

            // Setup default options
            _mockFileOptions.Setup(x => x.Value).Returns(new FileSettingsDTO { ProfilePicturesLocation = "uploads/profile-pictures" });
            _mockJwtOptions.Setup(x => x.Value).Returns(new JwtSettingsDTO 
            {
                Key = "this-is-a-super-secret-key-that-is-long-enough-for-hmac-sha512-algorithm-and-should-be-at-least-64-characters-long-to-meet-the-minimum-requirements",
                Issuer = "test-issuer",
                Audience = "test-audience",
                DurationInMinutes = "60"
            });

            _accountService = new AccountService(
                _mockFileOptions.Object,
                _mockJwtOptions.Object,
                _mockUserManager.Object,
                _mockFileService.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task AuthenticateAsync_Should_Return_Success_When_Valid_Credentials()
        {
            // Arrange
            var request = new AuthenticationRequestDTO 
            { 
                Email = "test@test.com", 
                Password = "Password123!" 
            };
            var user = new CustomIdentityUser 
            { 
                Id = "user-id", 
                UserName = "test@test.com", 
                Email = "test@test.com" 
            };

            _mockUserManager.Setup(x => x.FindByEmailAsync(request.Email))
                           .ReturnsAsync(user);
            _mockUserManager.Setup(x => x.CheckPasswordAsync(user, request.Password))
                           .ReturnsAsync(true);
            _mockUserManager.Setup(x => x.GetClaimsAsync(user))
                           .ReturnsAsync(new List<System.Security.Claims.Claim>());

            // Act
            var result = await _accountService.AuthenticateAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.AccessToken.Should().NotBeNullOrEmpty();
            result.Data.UserId.Should().Be(user.Id);
        }

        [Fact]
        public async Task AuthenticateAsync_Should_Return_Failure_When_User_Not_Found()
        {
            // Arrange
            var request = new AuthenticationRequestDTO 
            { 
                Email = "notfound@test.com", 
                Password = "Password123!" 
            };

            _mockUserManager.Setup(x => x.FindByEmailAsync(request.Email))
                           .ReturnsAsync((CustomIdentityUser)null);

            // Act
            var result = await _accountService.AuthenticateAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be("User not found");
        }

        [Fact]
        public async Task UpdateProfileAsync_Should_Return_Success_When_Valid_Data()
        {
            // Arrange
            var userId = "user-id";
            var userProfileDTO = new UpdateUserProfileDTO 
            { 
                UserName = "newusername",
                Email = "new@email.com" 
            };
            var existingUser = new CustomIdentityUser 
            { 
                Id = userId, 
                UserName = "oldusername",
                Email = "old@email.com" 
            };

            _mockUserManager.Setup(x => x.FindByIdAsync(userId))
                           .ReturnsAsync(existingUser);
            _mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<CustomIdentityUser>()))
                           .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _accountService.UpdateProfileAsync(userId, userProfileDTO);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Message.Should().Be("User profile updated successfully");
        }

        [Fact]
        public async Task UpdateSecurityInformationAsync_Should_Return_Success_When_Valid_Passwords()
        {
            // Arrange
            var userId = "user-id";
            var securityDTO = new UpdateUserSecurityDTO 
            { 
                CurrentPassword = "OldPassword123!",
                NewPassword = "NewPassword123!",
                ConfirmPassword = "NewPassword123!"
            };
            var existingUser = new CustomIdentityUser { Id = userId };

            _mockUserManager.Setup(x => x.FindByIdAsync(userId))
                           .ReturnsAsync(existingUser);
            _mockUserManager.Setup(x => x.ChangePasswordAsync(existingUser, securityDTO.CurrentPassword, securityDTO.NewPassword))
                           .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _accountService.UpdateSecurityInformationAsync(userId, securityDTO);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Message.Should().Be("User password changed successfully");
        }

        [Fact]
        public async Task UploadProfilePictureAsync_Should_Return_Success_When_Valid_File()
        {
            // Arrange
            var userId = "user-id";
            var formFile = CreateValidTestFormFile("test.jpg", "image/jpeg", 50000); // 50KB file
            var uploadDTO = new UploadProfilePictureDTO { ProfilePicture = formFile };
            var existingUser = new CustomIdentityUser { Id = userId };
            var testImageBytes = new byte[] { 1, 2, 3, 4 };

            _mockUserManager.Setup(x => x.FindByIdAsync(userId))
                           .ReturnsAsync(existingUser);
    
            // Add missing GeneratePathToLocation mock
            _mockFileService.Setup(x => x.GeneratePathToLocation(It.IsAny<string>()))
                           .Returns("/test/path");
    
            _mockFileService.Setup(x => x.SaveFileAsync(It.IsAny<string>(), It.IsAny<string>(), formFile))
                           .Returns(Task.CompletedTask);
    
            _mockFileService.Setup(x => x.GetFileAsByteArrayAsync(It.IsAny<string>(), It.IsAny<string>()))
                           .ReturnsAsync(testImageBytes);
    
            _mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<CustomIdentityUser>()))
                           .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _accountService.UploadProfilePictureAsync(userId, uploadDTO);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNullOrEmpty();
            result.Message.Should().Be("Profile picture uploaded successfully");
        }

        [Fact]
        public async Task UploadProfilePictureAsync_Should_Return_Failure_When_File_Too_Large()
        {
            // Arrange
            var userId = "user-id";
            var formFile = CreateValidTestFormFile("test.jpg", "image/jpeg", 10 * 1024 * 1024); // 10MB file - too large
            var uploadDTO = new UploadProfilePictureDTO { ProfilePicture = formFile };

            // Act
            var result = await _accountService.UploadProfilePictureAsync(userId, uploadDTO);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be("Validation failed");
            result.Errors.Should().Contain(error => error.Contains("5MB"));
        }

        [Fact]
        public async Task UploadProfilePictureAsync_Should_Return_Failure_When_Invalid_File_Type()
        {
            // Arrange
            var userId = "user-id";
            var formFile = CreateValidTestFormFile("test.exe", "application/exe", 50000);
            var uploadDTO = new UploadProfilePictureDTO { ProfilePicture = formFile };

            // Act
            var result = await _accountService.UploadProfilePictureAsync(userId, uploadDTO);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be("Validation failed");
            result.Errors.Should().Contain(error => error.Contains("extension"));
        }

        [Fact]
        public async Task UploadProfilePictureAsync_Should_Return_Failure_When_User_Not_Found()
        {
            // Arrange
            var userId = "nonexistent-user";
            var formFile = CreateValidTestFormFile("test.jpg", "image/jpeg", 50000);
            var uploadDTO = new UploadProfilePictureDTO { ProfilePicture = formFile };

            _mockUserManager.Setup(x => x.FindByIdAsync(userId))
                           .ReturnsAsync((CustomIdentityUser)null);

            // Act
            var result = await _accountService.UploadProfilePictureAsync(userId, uploadDTO);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be("User not found");
        }

        [Fact]
        public async Task UploadProfilePictureAsync_Should_Return_Failure_When_User_Update_Fails()
        {
            // Arrange
            var userId = "user-id";
            var formFile = CreateValidTestFormFile("test.jpg", "image/jpeg", 50000);
            var uploadDTO = new UploadProfilePictureDTO { ProfilePicture = formFile };
            var existingUser = new CustomIdentityUser { Id = userId };
            var testImageBytes = new byte[] { 1, 2, 3, 4 };

            var identityError = new IdentityError { Description = "Update failed" };
            var failureResult = IdentityResult.Failed(identityError);

            _mockUserManager.Setup(x => x.FindByIdAsync(userId))
                           .ReturnsAsync(existingUser);
    
            _mockFileService.Setup(x => x.GeneratePathToLocation(It.IsAny<string>()))
                           .Returns("/test/path");
    
            _mockFileService.Setup(x => x.SaveFileAsync(It.IsAny<string>(), It.IsAny<string>(), formFile))
                           .Returns(Task.CompletedTask);
    
            _mockFileService.Setup(x => x.GetFileAsByteArrayAsync(It.IsAny<string>(), It.IsAny<string>()))
                           .ReturnsAsync(testImageBytes);
    
            _mockFileService.Setup(x => x.DeleteFileAsync(It.IsAny<string>(), It.IsAny<string>()))
                           .Returns(Task.CompletedTask);
    
            _mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<CustomIdentityUser>()))
                           .ReturnsAsync(failureResult);

            // Act
            var result = await _accountService.UploadProfilePictureAsync(userId, uploadDTO);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be("Failed to save profile picture reference");
            result.Errors.Should().Contain("Update failed");
    
            // Verify cleanup was attempted
            _mockFileService.Verify(x => x.DeleteFileAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        // Update the helper method to create files that pass validation
        private IFormFile CreateValidTestFormFile(string fileName, string contentType, long fileSize = 50000)
        {
            // Create content that matches the specified file size
            var content = new byte[fileSize];
            for (int i = 0; i < fileSize; i++)
            {
                content[i] = (byte)(i % 256); // Fill with some data
            }
    
            var stream = new MemoryStream(content);
            var formFile = new FormFile(stream, 0, stream.Length, "ProfilePicture", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };
            return formFile;
        }

        private IFormFile CreateTestFormFile(string fileName, string contentType)
        {
            return CreateValidTestFormFile(fileName, contentType, 50000); // Default to valid size
        }
    }
}