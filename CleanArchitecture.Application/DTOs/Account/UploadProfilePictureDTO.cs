using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Application.DTOs.Account
{
    public class UploadProfilePictureDTO
    {
        public string? Id { get; set; }

        public IFormFile? ProfilePicture { get; set; }
    }
}
