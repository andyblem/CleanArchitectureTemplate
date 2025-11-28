using CleanArchitecture.Application.Features.Requests.BookRequests;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Features.Validators.BooksValidators
{
    public class GetBooksListRequestValidator : AbstractValidator<GetBooksListRequest>
    {
        public GetBooksListRequestValidator()
        {
            RuleFor(x => x.BookParameters).NotNull().WithMessage("Book parameters must be provided.");

            When(x => x.BookParameters != null, () =>
            {
                RuleFor(x => x.BookParameters.PageNumber)
                    .GreaterThan(0).WithMessage("Page number must be greater than 0.");
                RuleFor(x => x.BookParameters.PageSize)
                    .GreaterThan(0).WithMessage("Page size must be greater than 0.")
                    .LessThanOrEqualTo(100).WithMessage("Page size must not exceed 100.");
            });
        }
    }
}
