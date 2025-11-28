using CleanArchitecture.Application.DTOs.Account;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;

namespace CleanArchitecture.Application.Features.Validators.AccountValidators
{
    public class UploadProfilePictureValidator : AbstractValidator<UploadProfilePictureDTO>
    {
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
        private readonly string[] _allowedContentTypes = { 
            "image/jpeg", 
            "image/jpg", 
            "image/png", 
            "image/gif", 
            "image/bmp", 
            "image/webp" 
        };
        private const long MaxFileSize = 5 * 1024 * 1024; // 5MB
        private const long MinFileSize = 1024; // 1KB

        public UploadProfilePictureValidator()
        {
            RuleFor(x => x)
                .NotNull().WithMessage("UploadProfilePictureDTO cannot be null.");

            RuleFor(x => x.ProfilePicture)
                .NotNull().WithMessage("Profile picture is required.")
                .Must(HaveValidFileSize).WithMessage($"Profile picture must be between {MinFileSize / 1024}KB and {MaxFileSize / (1024 * 1024)}MB.")
                .Must(HaveValidFileExtension).WithMessage($"Profile picture must have one of the following extensions: {string.Join(", ", _allowedExtensions)}.")
                .Must(HaveValidContentType).WithMessage($"Profile picture must be a valid image file.")
                .Must(HaveValidFileName).WithMessage("Profile picture must have a valid filename.")
                .Must(NotBeEmpty).WithMessage("Profile picture file cannot be empty.");

        }

        private bool HaveValidFileSize(IFormFile file)
        {
            if (file == null) return false;
            return file.Length > MinFileSize && file.Length <= MaxFileSize;
        }

        private bool HaveValidFileExtension(IFormFile file)
        {
            if (file?.FileName == null) return false;
            
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return _allowedExtensions.Contains(extension);
        }

        private bool HaveValidContentType(IFormFile file)
        {
            if (file?.ContentType == null) return false;
            return _allowedContentTypes.Contains(file.ContentType.ToLowerInvariant());
        }

        private bool HaveValidFileName(IFormFile file)
        {
            if (file?.FileName == null) return false;
            
            // Check for valid filename (no path traversal, valid characters)
            var fileName = file.FileName;
            
            // Check for path traversal attempts
            if (fileName.Contains("..") || fileName.Contains("/") || fileName.Contains("\\"))
                return false;
            
            // Check for invalid characters
            var invalidChars = Path.GetInvalidFileNameChars();
            if (fileName.Any(c => invalidChars.Contains(c)))
                return false;
            
            // Check filename length
            if (fileName.Length > 255)
                return false;
                
            return true;
        }

        private bool NotBeEmpty(IFormFile file)
        {
            return file != null && file.Length > 0;
        }
    }
}
