using CleanArchitecture.Application.Requests.BookRequests;
using CleanArchitecture.Application.Validators.BooksValidators;
using FluentAssertions;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.UnitTests.Application.Validators.Book
{
    public class GetBookValidatorTests
    {
        private readonly GetBookValidator _validator;

        public GetBookValidatorTests()
        {
            _validator = new GetBookValidator();
        }

        [Fact]
        public void Validator_Should_Have_Error_When_Id_Is_Zero()
        {
            var request = new GetBookRequest { Id = 0 };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.Id)
                .WithErrorMessage("Book Id must be greater than zero.");
        }

        [Fact]
        public void Validator_Should_Have_Error_When_Id_Is_Negative()
        {
            var request = new GetBookRequest { Id = -1 };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.Id)
                .WithErrorMessage("Book Id must be greater than zero.");
        }

        [Fact]
        public void Validator_Should_Pass_When_Id_Is_Positive()
        {
            var request = new GetBookRequest { Id = 1 };
            var result = _validator.TestValidate(request);
            result.ShouldNotHaveValidationErrorFor(r => r.Id);
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(int.MaxValue)]
        public void Validator_Should_Pass_For_Valid_Positive_Ids(int id)
        {
            var request = new GetBookRequest { Id = id };
            var result = _validator.TestValidate(request);
            result.ShouldNotHaveValidationErrorFor(r => r.Id);
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10)]
        [InlineData(-100)]
        [InlineData(-1000)]
        [InlineData(int.MinValue)]
        public void Validator_Should_Have_Error_For_Invalid_Ids(int id)
        {
            var request = new GetBookRequest { Id = id };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.Id)
                .WithErrorMessage("Book Id must be greater than zero.");
        }

        [Fact]
        public void Validator_Should_Only_Validate_Id_Property()
        {
            var request = new GetBookRequest { Id = 1 };
            var result = _validator.TestValidate(request);

            // Should only validate the Id property, no other validation errors
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Validator_Should_Have_Single_Error_For_Invalid_Id()
        {
            var request = new GetBookRequest { Id = 0 };
            var result = _validator.TestValidate(request);

            // Should have exactly one validation error
            result.Errors.Should().HaveCount(1);
            result.Errors[0].PropertyName.Should().Be("Id");
            result.Errors[0].ErrorMessage.Should().Be("Book Id must be greater than zero.");
        }

        [Fact]
        public void Validator_Should_Pass_For_Minimum_Valid_Id()
        {
            var request = new GetBookRequest { Id = 1 };
            var result = _validator.TestValidate(request);
            result.ShouldNotHaveValidationErrorFor(r => r.Id);
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validator_Should_Pass_For_Maximum_Valid_Id()
        {
            var request = new GetBookRequest { Id = int.MaxValue };
            var result = _validator.TestValidate(request);
            result.ShouldNotHaveValidationErrorFor(r => r.Id);
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validator_Should_Have_Error_For_Minimum_Invalid_Id()
        {
            var request = new GetBookRequest { Id = int.MinValue };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.Id)
                .WithErrorMessage("Book Id must be greater than zero.");
        }
    }
}
