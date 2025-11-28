using CleanArchitecture.Application.DTOs.Account;
using FluentValidation;

namespace CleanArchitecture.Application.Features.Validators.AccountValidators
{
    public class AuthenticationRequestValidator : AbstractValidator<AuthenticationRequestDTO>
    {
        public AuthenticationRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Please enter a valid email address.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        }
    }
}
