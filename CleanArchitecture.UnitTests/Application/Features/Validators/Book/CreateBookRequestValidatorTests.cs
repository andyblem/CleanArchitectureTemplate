using CleanArchitecture.Application.Requests.BookRequests;
using CleanArchitecture.Application.DTOs.Book;
using FluentValidation.TestHelper;
using Xunit;
using FluentAssertions;
using CleanArchitecture.Application.Features.Validators.BooksValidators;

namespace CleanArchitecture.UnitTests.Application.Features.Validators.Book
{
    public class CreateBookRequestValidatorTests
    {
        private readonly CreateBookRequestValidator _validator;

        public CreateBookRequestValidatorTests()
        {
            _validator = new CreateBookRequestValidator();
        }

        [Fact]
        public void Validator_Should_Have_Error_When_Book_Is_Null()
        {
            var request = new CreateBookRequest { Book = null! };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.Book);
        }

        [Fact]
        public void Validator_Should_Have_Error_When_ISBN_Is_Empty()
        {
            var request = new CreateBookRequest { Book = new CreateBookDTO { ISBN = "", Title = "Title" } };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.Book!.ISBN);
        }

        [Fact]
        public void Validator_Should_Pass_For_Valid_Book()
        {
            var request = new CreateBookRequest { Book = new CreateBookDTO { ISBN = "123-ABC", Title = "Valid Title" } };
            var result = _validator.TestValidate(request);
            result.IsValid.Should().BeTrue();
        }
    }
}