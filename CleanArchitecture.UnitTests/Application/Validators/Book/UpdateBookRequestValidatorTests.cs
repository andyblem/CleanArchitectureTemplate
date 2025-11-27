using CleanArchitecture.Application.DTOs.Book;
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
    public class UpdateBookRequestValidatorTests
    {
        private readonly UpdateBookRequestValidator _validator;

        public UpdateBookRequestValidatorTests()
        {
            _validator = new UpdateBookRequestValidator();
        }

        [Fact]
        public void Validator_Should_Have_Error_When_Book_Is_Null()
        {
            var request = new UpdateBookRequest { Id = 1, Book = null! };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.Book)
                .WithErrorMessage("Book data must be provided.");
        }

        [Fact]
        public void Validator_Should_Have_Error_When_Route_Id_Does_Not_Match_Book_Id()
        {
            var request = new UpdateBookRequest
            {
                Id = 1,
                Book = new UpdateBookDTO
                {
                    Id = 2,
                    Title = "Valid Title",
                    Summary = "Valid Summary",
                    Price = 10.99m
                }
            };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.Book.Id)
                .WithErrorMessage("Route id must match Book.Id in payload.");
        }

        [Fact]
        public void Validator_Should_Have_Error_When_Book_Id_Is_Zero()
        {
            var request = new UpdateBookRequest
            {
                Id = 0,
                Book = new UpdateBookDTO
                {
                    Id = 0,
                    Title = "Valid Title",
                    Summary = "Valid Summary",
                    Price = 10.99m
                }
            };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.Book.Id)
                .WithErrorMessage("Book ID must be greater than zero.");
        }

        [Fact]
        public void Validator_Should_Have_Error_When_Book_Id_Is_Negative()
        {
            var request = new UpdateBookRequest
            {
                Id = -1,
                Book = new UpdateBookDTO
                {
                    Id = -1,
                    Title = "Valid Title",
                    Summary = "Valid Summary",
                    Price = 10.99m
                }
            };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.Book.Id)
                .WithErrorMessage("Book ID must be greater than zero.");
        }

        [Fact]
        public void Validator_Should_Have_Error_When_Title_Is_Empty()
        {
            var request = new UpdateBookRequest
            {
                Id = 1,
                Book = new UpdateBookDTO
                {
                    Id = 1,
                    Title = "",
                    Summary = "Valid Summary",
                    Price = 10.99m
                }
            };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.Book.Title)
                .WithErrorMessage("Book title is required.");
        }

        [Fact]
        public void Validator_Should_Have_Error_When_Title_Is_Null()
        {
            var request = new UpdateBookRequest
            {
                Id = 1,
                Book = new UpdateBookDTO
                {
                    Id = 1,
                    Title = null!,
                    Summary = "Valid Summary",
                    Price = 10.99m
                }
            };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.Book.Title)
                .WithErrorMessage("Book title is required.");
        }

        [Fact]
        public void Validator_Should_Have_Error_When_Title_Exceeds_Maximum_Length()
        {
            var longTitle = new string('A', 201); // 201 characters
            var request = new UpdateBookRequest
            {
                Id = 1,
                Book = new UpdateBookDTO
                {
                    Id = 1,
                    Title = longTitle,
                    Summary = "Valid Summary",
                    Price = 10.99m
                }
            };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.Book.Title)
                .WithErrorMessage("Book title must not exceed 200 characters.");
        }

        [Fact]
        public void Validator_Should_Pass_When_Title_Is_At_Maximum_Length()
        {
            var titleAtMaxLength = new string('A', 200); // Exactly 200 characters
            var request = new UpdateBookRequest
            {
                Id = 1,
                Book = new UpdateBookDTO
                {
                    Id = 1,
                    Title = titleAtMaxLength,
                    Summary = "Valid Summary",
                    Price = 10.99m
                }
            };
            var result = _validator.TestValidate(request);
            result.ShouldNotHaveValidationErrorFor(r => r.Book.Title);
        }

        [Fact]
        public void Validator_Should_Have_Error_When_Summary_Is_Empty()
        {
            var request = new UpdateBookRequest
            {
                Id = 1,
                Book = new UpdateBookDTO
                {
                    Id = 1,
                    Title = "Valid Title",
                    Summary = "",
                    Price = 10.99m
                }
            };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.Book.Summary)
                .WithErrorMessage("Book summary is required.");
        }

        [Fact]
        public void Validator_Should_Have_Error_When_Summary_Is_Null()
        {
            var request = new UpdateBookRequest
            {
                Id = 1,
                Book = new UpdateBookDTO
                {
                    Id = 1,
                    Title = "Valid Title",
                    Summary = null!,
                    Price = 10.99m
                }
            };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.Book.Summary)
                .WithErrorMessage("Book summary is required.");
        }

        [Fact]
        public void Validator_Should_Have_Error_When_Summary_Exceeds_Maximum_Length()
        {
            var longSummary = new string('A', 1001); // 1001 characters
            var request = new UpdateBookRequest
            {
                Id = 1,
                Book = new UpdateBookDTO
                {
                    Id = 1,
                    Title = "Valid Title",
                    Summary = longSummary,
                    Price = 10.99m
                }
            };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.Book.Summary)
                .WithErrorMessage("Book summary must not exceed 1000 characters.");
        }

        [Fact]
        public void Validator_Should_Pass_When_Summary_Is_At_Maximum_Length()
        {
            var summaryAtMaxLength = new string('A', 1000); // Exactly 1000 characters
            var request = new UpdateBookRequest
            {
                Id = 1,
                Book = new UpdateBookDTO
                {
                    Id = 1,
                    Title = "Valid Title",
                    Summary = summaryAtMaxLength,
                    Price = 10.99m
                }
            };
            var result = _validator.TestValidate(request);
            result.ShouldNotHaveValidationErrorFor(r => r.Book.Summary);
        }

        [Fact]
        public void Validator_Should_Have_Error_When_Price_Is_Zero()
        {
            var request = new UpdateBookRequest
            {
                Id = 1,
                Book = new UpdateBookDTO
                {
                    Id = 1,
                    Title = "Valid Title",
                    Summary = "Valid Summary",
                    Price = 0m
                }
            };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.Book.Price)
                .WithErrorMessage("Book price must be greater than zero.");
        }

        [Fact]
        public void Validator_Should_Have_Error_When_Price_Is_Negative()
        {
            var request = new UpdateBookRequest
            {
                Id = 1,
                Book = new UpdateBookDTO
                {
                    Id = 1,
                    Title = "Valid Title",
                    Summary = "Valid Summary",
                    Price = -10.99m
                }
            };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.Book.Price)
                .WithErrorMessage("Book price must be greater than zero.");
        }

        [Fact]
        public void Validator_Should_Pass_For_Valid_Update_Request()
        {
            var request = new UpdateBookRequest
            {
                Id = 1,
                Book = new UpdateBookDTO
                {
                    Id = 1,
                    Title = "Valid Book Title",
                    Summary = "This is a valid book summary that provides good information about the book.",
                    Price = 19.99m
                }
            };
            var result = _validator.TestValidate(request);
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validator_Should_Not_Validate_Book_Properties_When_Book_Is_Null()
        {
            var request = new UpdateBookRequest { Id = 1, Book = null! };
            var result = _validator.TestValidate(request);

            // Should only have error for Book being null, not for individual properties
            result.ShouldHaveValidationErrorFor(r => r.Book);
            result.ShouldNotHaveValidationErrorFor(r => r.Book!.Id);
            result.ShouldNotHaveValidationErrorFor(r => r.Book!.Title);
            result.ShouldNotHaveValidationErrorFor(r => r.Book!.Summary);
            result.ShouldNotHaveValidationErrorFor(r => r.Book!.Price);
        }

        [Theory]
        [InlineData(0.01)]
        [InlineData(1)]
        [InlineData(99.99)]
        [InlineData(999.99)]
        public void Validator_Should_Pass_For_Valid_Prices(decimal price)
        {
            var request = new UpdateBookRequest
            {
                Id = 1,
                Book = new UpdateBookDTO
                {
                    Id = 1,
                    Title = "Valid Title",
                    Summary = "Valid Summary",
                    Price = price
                }
            };
            var result = _validator.TestValidate(request);
            result.ShouldNotHaveValidationErrorFor(r => r.Book.Price);
        }

        [Theory]
        [InlineData("A")]
        [InlineData("Valid Title")]
        [InlineData("This is a longer but still valid title that should pass validation")]
        public void Validator_Should_Pass_For_Valid_Titles(string title)
        {
            var request = new UpdateBookRequest
            {
                Id = 1,
                Book = new UpdateBookDTO
                {
                    Id = 1,
                    Title = title,
                    Summary = "Valid Summary",
                    Price = 10.99m
                }
            };
            var result = _validator.TestValidate(request);
            result.ShouldNotHaveValidationErrorFor(r => r.Book.Title);
        }
    }
}
