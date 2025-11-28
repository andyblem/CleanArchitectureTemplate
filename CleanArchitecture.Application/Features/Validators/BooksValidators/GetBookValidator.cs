using CleanArchitecture.Application.Features.Requests.BookRequests;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Features.Validators.BooksValidators
{
    public class GetBookValidator : AbstractValidator<GetBookRequest>
    {
        public GetBookValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Book Id must be greater than zero.");
        }
    }
}
