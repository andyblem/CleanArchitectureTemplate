using CleanArchitecture.Application.Features.Validators.BooksValidators;
using CleanArchitecture.Application.Requests.BookRequests;
using FluentAssertions;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.UnitTests.Application.Features.Validators.Book
{
    public class DeleteBookRequestValidatorTests
    {
        private readonly DeleteBookRequestValidator _validator;

        public DeleteBookRequestValidatorTests()
        {
            _validator = new DeleteBookRequestValidator();
        }

        [Fact]
        public void Validator_Should_Have_Error_When_Id_Is_Zero()
        {
            var request = new DeleteBookRequest { Id = 0 };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.Id)
                .WithErrorMessage("Book Id must be greater than zero.");
        }

        [Fact]
        public void Validator_Should_Have_Error_When_Id_Is_Negative()
        {
            var request = new DeleteBookRequest { Id = -1 };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.Id)
                .WithErrorMessage("Book Id must be greater than zero.");
        }

        [Fact]
        public void Validator_Should_Pass_When_Id_Is_Positive()
        {
            var request = new DeleteBookRequest { Id = 1 };
            var result = _validator.TestValidate(request);
            result.ShouldNotHaveValidationErrorFor(r => r.Id);
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(999999)]
        [InlineData(int.MaxValue)]
        public void Validator_Should_Pass_For_Valid_Positive_Ids(int id)
        {
            var request = new DeleteBookRequest { Id = id };
            var result = _validator.TestValidate(request);
            result.ShouldNotHaveValidationErrorFor(r => r.Id);
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10)]
        [InlineData(-100)]
        [InlineData(-999999)]
        [InlineData(int.MinValue)]
        public void Validator_Should_Have_Error_For_Invalid_Ids(int id)
        {
            var request = new DeleteBookRequest { Id = id };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.Id)
                .WithErrorMessage("Book Id must be greater than zero.");
        }

        [Fact]
        public void Validator_Should_Only_Validate_Id_Property()
        {
            var request = new DeleteBookRequest { Id = 1 };
            var result = _validator.TestValidate(request);

            // Should only validate the Id property, no other validation errors
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Validator_Should_Have_Single_Error_For_Invalid_Id()
        {
            var request = new DeleteBookRequest { Id = 0 };
            var result = _validator.TestValidate(request);

            // Should have exactly one validation error
            result.Errors.Should().HaveCount(1);
            result.Errors[0].PropertyName.Should().Be("Id");
            result.Errors[0].ErrorMessage.Should().Be("Book Id must be greater than zero.");
        }
    }
}
