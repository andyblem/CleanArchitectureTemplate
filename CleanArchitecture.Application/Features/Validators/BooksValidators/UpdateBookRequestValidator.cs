using CleanArchitecture.Application.Features.Requests.BookRequests;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Features.Validators.BooksValidators
{
    public class UpdateBookRequestValidator : AbstractValidator<UpdateBookRequest>
    {
        public UpdateBookRequestValidator()
        {

            RuleFor(x => x.Book)
                .NotNull().WithMessage("Book data must be provided.");


            When(x => x.Book != null, () =>
            {
                RuleFor(x => x.Book.Id)
                    .Equal(x => x.Id)
                    .WithMessage("Route id must match Book.Id in payload.");

                RuleFor(x => x.Book.Id)
                    .GreaterThan(0).WithMessage("Book ID must be greater than zero.");

                RuleFor(x => x.Book.Title)
                    .NotEmpty().WithMessage("Book title is required.")
                    .MaximumLength(200).WithMessage("Book title must not exceed 200 characters.");

                RuleFor(x => x.Book.Summary)
                    .NotEmpty().WithMessage("Book summary is required.")
                    .MaximumLength(1000).WithMessage("Book summary must not exceed 1000 characters.");

                RuleFor(x => x.Book.Price)
                    .GreaterThan(0).WithMessage("Book price must be greater than zero.");
            });
        }
    }
}
