using CleanArchitecture.Application.Features.Books.Commands;
using CleanArchitecture.Application.Features.Books.Queries;
using CleanArchitecture.Application.Requests.BookRequests;
using CleanArchitecture.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Validators.BooksValidators
{
    public class CreateBookRequestValidator : AbstractValidator<CreateBookRequest>
    {

        public CreateBookRequestValidator()
        {
            RuleFor(b => b.Book)
                .NotNull().WithMessage("{PropertyName} is required.");

            When(x => x.Book != null, () =>
            {
                RuleFor(b => b.Book.ISBN)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

                RuleFor(p => p.Book.Title)
                    .NotEmpty().WithMessage("{PropertyName} is required.")
                    .NotNull()
                    .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");
            });
        }
    }
}
