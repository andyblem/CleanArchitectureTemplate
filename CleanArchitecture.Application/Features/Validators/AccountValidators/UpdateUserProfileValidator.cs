using CleanArchitecture.Application.DTOs.Account;
using FluentValidation;

namespace CleanArchitecture.Application.Features.Validators.AccountValidators
{
    public class UpdateUserProfileValidator : AbstractValidator<UpdateUserProfileDTO>
    {
        public UpdateUserProfileValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters.")
                .MaximumLength(50).WithMessage("Username must not exceed 50 characters.")
                .Matches("^[a-zA-Z0-9._-]+$").WithMessage("Username can only contain letters, numbers, dots, underscores, and hyphens.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.")
                .MaximumLength(100).WithMessage("Email must not exceed 100 characters.");

        }
    }
}
