namespace CleanArchitecture.Application.DTOs.Account
{
    public class UpdateUserSecurityDTO
    {
        public string? CurrentPassword { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? Id { get; set; }
        public string? NewPassword { get; set; }
    }
}
