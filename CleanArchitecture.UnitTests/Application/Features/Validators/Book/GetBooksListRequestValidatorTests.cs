using CleanArchitecture.Application.Features.Parameters.Book;
using CleanArchitecture.Application.Features.Requests.BookRequests;
using FluentValidation.TestHelper;
using Xunit;
using FluentAssertions;
using CleanArchitecture.Application.Features.Validators.BooksValidators;

namespace CleanArchitecture.UnitTests.Application.Features.Validators.Book
{
    public class GetBooksListRequestValidatorTests
    {
        private readonly GetBooksListRequestValidator _validator;

        public GetBooksListRequestValidatorTests()
        {
            _validator = new GetBooksListRequestValidator();
        }

        [Fact]
        public void Validator_Should_Have_Error_When_BookParameters_Is_Null()
        {
            var request = new GetBooksListRequest { BookParameters = null! };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.BookParameters)
                .WithErrorMessage("Book parameters must be provided.");
        }

        [Fact]
        public void Validator_Should_Have_Error_When_PageNumber_Is_Zero()
        {
            var request = new GetBooksListRequest
            {
                BookParameters = new GetBooksListParameter { PageNumber = 0, PageSize = 10 }
            };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.BookParameters.PageNumber)
                .WithErrorMessage("Page number must be greater than 0.");
        }

        [Fact]
        public void Validator_Should_Have_Error_When_PageNumber_Is_Negative()
        {
            var request = new GetBooksListRequest
            {
                BookParameters = new GetBooksListParameter { PageNumber = -1, PageSize = 10 }
            };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.BookParameters.PageNumber)
                .WithErrorMessage("Page number must be greater than 0.");
        }

        [Fact]
        public void Validator_Should_Have_Error_When_PageSize_Is_Zero()
        {
            var request = new GetBooksListRequest
            {
                BookParameters = new GetBooksListParameter { PageNumber = 1, PageSize = 0 }
            };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.BookParameters.PageSize)
                .WithErrorMessage("Page size must be greater than 0.");
        }

        [Fact]
        public void Validator_Should_Have_Error_When_PageSize_Is_Negative()
        {
            var request = new GetBooksListRequest
            {
                BookParameters = new GetBooksListParameter { PageNumber = 1, PageSize = -5 }
            };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.BookParameters.PageSize)
                .WithErrorMessage("Page size must be greater than 0.");
        }

        [Fact]
        public void Validator_Should_Have_Error_When_PageSize_Exceeds_Maximum()
        {
            var request = new GetBooksListRequest
            {
                BookParameters = new GetBooksListParameter { PageNumber = 1, PageSize = 101 }
            };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.BookParameters.PageSize)
                .WithErrorMessage("Page size must not exceed 100.");
        }

        [Fact]
        public void Validator_Should_Have_Error_When_PageSize_Is_Much_Larger_Than_Maximum()
        {
            var request = new GetBooksListRequest
            {
                BookParameters = new GetBooksListParameter { PageNumber = 1, PageSize = 500 }
            };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.BookParameters.PageSize)
                .WithErrorMessage("Page size must not exceed 100.");
        }

        [Fact]
        public void Validator_Should_Pass_When_PageSize_Is_At_Maximum()
        {
            var request = new GetBooksListRequest
            {
                BookParameters = new GetBooksListParameter { PageNumber = 1, PageSize = 100 }
            };
            var result = _validator.TestValidate(request);
            result.ShouldNotHaveValidationErrorFor(r => r.BookParameters.PageSize);
        }

        [Fact]
        public void Validator_Should_Pass_For_Valid_Parameters()
        {
            var request = new GetBooksListRequest
            {
                BookParameters = new GetBooksListParameter { PageNumber = 1, PageSize = 10 }
            };
            var result = _validator.TestValidate(request);
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validator_Should_Pass_For_Valid_Parameters_With_Different_Values()
        {
            var request = new GetBooksListRequest
            {
                BookParameters = new GetBooksListParameter { PageNumber = 5, PageSize = 50 }
            };
            var result = _validator.TestValidate(request);
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validator_Should_Have_Multiple_Errors_For_Invalid_Parameters()
        {
            var request = new GetBooksListRequest
            {
                BookParameters = new GetBooksListParameter { PageNumber = 0, PageSize = 0 }
            };
            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(r => r.BookParameters.PageNumber)
                .WithErrorMessage("Page number must be greater than 0.");
            result.ShouldHaveValidationErrorFor(r => r.BookParameters.PageSize)
                .WithErrorMessage("Page size must be greater than 0.");
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Validator_Should_Have_Multiple_Errors_For_PageSize_Violations()
        {
            var request = new GetBooksListRequest
            {
                BookParameters = new GetBooksListParameter { PageNumber = 1, PageSize = -5 }
            };
            var result = _validator.TestValidate(request);

            // Should have error for page size being less than or equal to 0
            result.ShouldHaveValidationErrorFor(r => r.BookParameters.PageSize)
                .WithErrorMessage("Page size must be greater than 0.");
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Validator_Should_Not_Validate_PageParameters_When_BookParameters_Is_Null()
        {
            var request = new GetBooksListRequest { BookParameters = null! };
            var result = _validator.TestValidate(request);

            // Should only have error for BookParameters being null
            result.ShouldHaveValidationErrorFor(r => r.BookParameters);
            result.ShouldNotHaveValidationErrorFor(r => r.BookParameters!.PageNumber);
            result.ShouldNotHaveValidationErrorFor(r => r.BookParameters!.PageSize);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 10)]
        [InlineData(1, 25)]
        [InlineData(1, 50)]
        [InlineData(1, 100)]
        [InlineData(5, 20)]
        [InlineData(10, 100)]
        [InlineData(100, 1)]
        public void Validator_Should_Pass_For_Valid_PageNumber_And_PageSize_Combinations(int pageNumber, int pageSize)
        {
            var request = new GetBooksListRequest
            {
                BookParameters = new GetBooksListParameter { PageNumber = pageNumber, PageSize = pageSize }
            };
            var result = _validator.TestValidate(request);
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(-1, 10)]
        [InlineData(-5, 10)]
        [InlineData(-100, 10)]
        public void Validator_Should_Have_Error_For_Invalid_PageNumbers(int pageNumber, int pageSize)
        {
            var request = new GetBooksListRequest
            {
                BookParameters = new GetBooksListParameter { PageNumber = pageNumber, PageSize = pageSize }
            };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.BookParameters.PageNumber)
                .WithErrorMessage("Page number must be greater than 0.");
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(1, -1)]
        [InlineData(1, -5)]
        [InlineData(1, -100)]
        public void Validator_Should_Have_Error_For_Invalid_PageSizes_LessThanOrEqualToZero(int pageNumber, int pageSize)
        {
            var request = new GetBooksListRequest
            {
                BookParameters = new GetBooksListParameter { PageNumber = pageNumber, PageSize = pageSize }
            };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.BookParameters.PageSize)
                .WithErrorMessage("Page size must be greater than 0.");
        }

        [Theory]
        [InlineData(1, 101)]
        [InlineData(1, 150)]
        [InlineData(1, 200)]
        [InlineData(1, 1000)]
        [InlineData(1, int.MaxValue)]
        public void Validator_Should_Have_Error_For_PageSizes_Exceeding_Maximum(int pageNumber, int pageSize)
        {
            var request = new GetBooksListRequest
            {
                BookParameters = new GetBooksListParameter { PageNumber = pageNumber, PageSize = pageSize }
            };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.BookParameters.PageSize)
                .WithErrorMessage("Page size must not exceed 100.");
        }

        [Fact]
        public void Validator_Should_Pass_When_Using_Default_Constructor_Values()
        {
            var request = new GetBooksListRequest
            {
                BookParameters = new GetBooksListParameter() // Uses default values: PageNumber = 1, PageSize = 10
            };
            var result = _validator.TestValidate(request);
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validator_Should_Pass_When_Using_Parameterized_Constructor_With_Valid_Values()
        {
            var request = new GetBooksListRequest
            {
                BookParameters = new GetBooksListParameter(2, 20)
            };
            var result = _validator.TestValidate(request);
            result.IsValid.Should().BeTrue();
        }
    }
}
