using CleanArchitecture.Application.DTOs.Account;
using FluentValidation;

namespace CleanArchitecture.Application.Features.Validators.AccountValidators
{
    public class UpdateUserSecurityValidator : AbstractValidator<UpdateUserSecurityDTO>
    {
        public UpdateUserSecurityValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("Current password is required.")
                .MinimumLength(6).WithMessage("Current password must be at least 6 characters long.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password is required.")
                .MinimumLength(8).WithMessage("New password must be at least 8 characters long.")
                .MaximumLength(100).WithMessage("New password must not exceed 100 characters.")
                .Matches(@"[A-Z]+").WithMessage("New password must contain at least one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("New password must contain at least one lowercase letter.")
                .Matches(@"[0-9]+").WithMessage("New password must contain at least one number.")
                .Matches(@"[\!\?\*\.\@\#\$\%\^\&\(\)\-\+\=]+").WithMessage("New password must contain at least one special character (!?*.@#$%^&()+-=).");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Password confirmation is required.")
                .Equal(x => x.NewPassword).WithMessage("Password confirmation must match the new password.");

            // Custom rule to ensure new password is different from current password
            RuleFor(x => x)
                .Must(x => x.NewPassword != x.CurrentPassword)
                .WithMessage("New password must be different from the current password.")
                .When(x => !string.IsNullOrEmpty(x.CurrentPassword) && !string.IsNullOrEmpty(x.NewPassword));
        }
    }
}
