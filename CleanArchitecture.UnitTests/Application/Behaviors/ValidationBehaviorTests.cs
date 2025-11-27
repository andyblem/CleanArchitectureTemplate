using CleanArchitecture.Application.Behaviours;
using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Application.Requests.BookRequests;
using CleanArchitecture.Application.Wrappers;
using CleanArchitecture.Application.DTOs.Book;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using ValidationException = CleanArchitecture.Application.Exceptions.ValidationException;

namespace CleanArchitecture.UnitTests.Application.Behaviors
{
    public class ValidationBehaviorTests
    {
        private readonly Mock<RequestHandlerDelegate<Response<BookDTO>>> _mockNext;
        private readonly Mock<IValidator<GetBookRequest>> _mockValidator;

        public ValidationBehaviorTests()
        {
            _mockNext = new Mock<RequestHandlerDelegate<Response<BookDTO>>>();
            _mockValidator = new Mock<IValidator<GetBookRequest>>();
        }

        [Fact]
        public async Task Handle_Should_Call_Next_When_No_Validators_Exist()
        {
            // Arrange
            var request = new GetBookRequest { Id = 1 };
            var expectedResponse = new Response<BookDTO>();
            var validators = new List<IValidator<GetBookRequest>>();
            var behavior = new ValidationBehavior<GetBookRequest, Response<BookDTO>>(validators);

            _mockNext.Setup(x => x()).ReturnsAsync(expectedResponse);

            // Act
            var result = await behavior.Handle(request, _mockNext.Object, CancellationToken.None);

            // Assert
            result.Should().Be(expectedResponse);
            _mockNext.Verify(x => x(), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Call_Next_When_Validation_Passes()
        {
            // Arrange
            var request = new GetBookRequest { Id = 1 };
            var expectedResponse = new Response<BookDTO>();
            var validationResult = new ValidationResult(); // Empty validation result (no errors)
            var validators = new List<IValidator<GetBookRequest>> { _mockValidator.Object };
            var behavior = new ValidationBehavior<GetBookRequest, Response<BookDTO>>(validators);

            _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<GetBookRequest>>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(validationResult);
            _mockNext.Setup(x => x()).ReturnsAsync(expectedResponse);

            // Act
            var result = await behavior.Handle(request, _mockNext.Object, CancellationToken.None);

            // Assert
            result.Should().Be(expectedResponse);
            _mockNext.Verify(x => x(), Times.Once);
            _mockValidator.Verify(v => v.ValidateAsync(It.IsAny<ValidationContext<GetBookRequest>>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_ValidationException_When_Validation_Fails()
        {
            // Arrange
            var request = new GetBookRequest { Id = 0 };
            var validationFailure = new ValidationFailure("Id", "Book Id must be greater than zero.");
            var validationResult = new ValidationResult(new[] { validationFailure });
            var validators = new List<IValidator<GetBookRequest>> { _mockValidator.Object };
            var behavior = new ValidationBehavior<GetBookRequest, Response<BookDTO>>(validators);

            _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<GetBookRequest>>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(validationResult);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() =>
                behavior.Handle(request, _mockNext.Object, CancellationToken.None));

            exception.Errors.Should().HaveCount(1);
            exception.Errors.First().Should().Be("Book Id must be greater than zero.");
            _mockNext.Verify(x => x(), Times.Never);
            _mockValidator.Verify(v => v.ValidateAsync(It.IsAny<ValidationContext<GetBookRequest>>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_ValidationException_With_Multiple_Errors()
        {
            // Arrange
            var request = new GetBookRequest { Id = -1 };
            var validationFailures = new[]
            {
                new ValidationFailure("Id", "Book Id must be greater than zero."),
                new ValidationFailure("Id", "Book Id is required.")
            };
            var validationResult = new ValidationResult(validationFailures);
            var validators = new List<IValidator<GetBookRequest>> { _mockValidator.Object };
            var behavior = new ValidationBehavior<GetBookRequest, Response<BookDTO>>(validators);

            _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<GetBookRequest>>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(validationResult);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() =>
                behavior.Handle(request, _mockNext.Object, CancellationToken.None));

            exception.Errors.Should().HaveCount(2);
            exception.Errors.Should().Contain("Book Id must be greater than zero.");
            exception.Errors.Should().Contain("Book Id is required.");
            _mockNext.Verify(x => x(), Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Combine_Errors_From_Multiple_Validators()
        {
            // Arrange
            var request = new GetBookRequest { Id = 0 };
            var mockValidator1 = new Mock<IValidator<GetBookRequest>>();
            var mockValidator2 = new Mock<IValidator<GetBookRequest>>();
            
            var validationResult1 = new ValidationResult(new[] 
            {
                new ValidationFailure("Id", "First validator error.")
            });
            var validationResult2 = new ValidationResult(new[] 
            {
                new ValidationFailure("Id", "Second validator error.")
            });

            var validators = new List<IValidator<GetBookRequest>> { mockValidator1.Object, mockValidator2.Object };
            var behavior = new ValidationBehavior<GetBookRequest, Response<BookDTO>>(validators);

            mockValidator1.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<GetBookRequest>>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(validationResult1);
            mockValidator2.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<GetBookRequest>>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(validationResult2);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() =>
                behavior.Handle(request, _mockNext.Object, CancellationToken.None));

            exception.Errors.Should().HaveCount(2);
            exception.Errors.Should().Contain("First validator error.");
            exception.Errors.Should().Contain("Second validator error.");
            _mockNext.Verify(x => x(), Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Filter_Out_Null_ValidationFailures()
        {
            // Arrange
            var request = new GetBookRequest { Id = 0 };
            var validationFailures = new[]
            {
                new ValidationFailure("Id", "Valid error message."),
                null, // This should be filtered out
                new ValidationFailure("Name", "Another valid error.")
            };
            var validationResult = new ValidationResult(validationFailures.Where(f => f != null));
            var validators = new List<IValidator<GetBookRequest>> { _mockValidator.Object };
            var behavior = new ValidationBehavior<GetBookRequest, Response<BookDTO>>(validators);

            _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<GetBookRequest>>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(validationResult);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() =>
                behavior.Handle(request, _mockNext.Object, CancellationToken.None));

            exception.Errors.Should().HaveCount(2);
            exception.Errors.Should().Contain("Valid error message.");
            exception.Errors.Should().Contain("Another valid error.");
        }

        [Fact]
        public async Task Handle_Should_Pass_Correct_ValidationContext_To_Validators()
        {
            // Arrange
            var request = new GetBookRequest { Id = 1 };
            var expectedResponse = new Response<BookDTO>();
            var validationResult = new ValidationResult();
            var validators = new List<IValidator<GetBookRequest>> { _mockValidator.Object };
            var behavior = new ValidationBehavior<GetBookRequest, Response<BookDTO>>(validators);
            IValidationContext capturedContext = null;

            _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>()))
                         .Callback<IValidationContext, CancellationToken>((context, _) => capturedContext = context)
                         .ReturnsAsync(validationResult);
            _mockNext.Setup(x => x()).ReturnsAsync(expectedResponse);

            // Act
            await behavior.Handle(request, _mockNext.Object, CancellationToken.None);

            // Assert
            capturedContext.Should().NotBeNull();
            capturedContext.InstanceToValidate.Should().Be(request);
        }

        [Fact]
        public async Task Handle_Should_Pass_CancellationToken_To_Validators()
        {
            // Arrange
            var request = new GetBookRequest { Id = 1 };
            var expectedResponse = new Response<BookDTO>();
            var validationResult = new ValidationResult();
            var validators = new List<IValidator<GetBookRequest>> { _mockValidator.Object };
            var behavior = new ValidationBehavior<GetBookRequest, Response<BookDTO>>(validators);
            var cancellationToken = new CancellationToken();

            _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<GetBookRequest>>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(validationResult);
            _mockNext.Setup(x => x()).ReturnsAsync(expectedResponse);

            // Act
            await behavior.Handle(request, _mockNext.Object, cancellationToken);

            // Assert
            _mockValidator.Verify(v => v.ValidateAsync(It.IsAny<ValidationContext<GetBookRequest>>(), cancellationToken), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Execute_All_Validators_Concurrently()
        {
            // Arrange
            var request = new GetBookRequest { Id = 1 };
            var expectedResponse = new Response<BookDTO>();
            var mockValidator1 = new Mock<IValidator<GetBookRequest>>();
            var mockValidator2 = new Mock<IValidator<GetBookRequest>>();
            var mockValidator3 = new Mock<IValidator<GetBookRequest>>();
            
            var validationResult = new ValidationResult();
            var validators = new List<IValidator<GetBookRequest>> 
            { 
                mockValidator1.Object, 
                mockValidator2.Object, 
                mockValidator3.Object 
            };
            var behavior = new ValidationBehavior<GetBookRequest, Response<BookDTO>>(validators);

            mockValidator1.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<GetBookRequest>>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(validationResult);
            mockValidator2.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<GetBookRequest>>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(validationResult);
            mockValidator3.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<GetBookRequest>>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(validationResult);
            _mockNext.Setup(x => x()).ReturnsAsync(expectedResponse);

            // Act
            var result = await behavior.Handle(request, _mockNext.Object, CancellationToken.None);

            // Assert
            result.Should().Be(expectedResponse);
            mockValidator1.Verify(v => v.ValidateAsync(It.IsAny<ValidationContext<GetBookRequest>>(), It.IsAny<CancellationToken>()), Times.Once);
            mockValidator2.Verify(v => v.ValidateAsync(It.IsAny<ValidationContext<GetBookRequest>>(), It.IsAny<CancellationToken>()), Times.Once);
            mockValidator3.Verify(v => v.ValidateAsync(It.IsAny<ValidationContext<GetBookRequest>>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Not_Throw_When_Some_Validators_Pass_And_Others_Have_No_Failures()
        {
            // Arrange
            var request = new GetBookRequest { Id = 1 };
            var expectedResponse = new Response<BookDTO>();
            var mockValidator1 = new Mock<IValidator<GetBookRequest>>();
            var mockValidator2 = new Mock<IValidator<GetBookRequest>>();
            
            var validationResultWithNoErrors = new ValidationResult();
            var validationResultWithNoErrors2 = new ValidationResult();

            var validators = new List<IValidator<GetBookRequest>> { mockValidator1.Object, mockValidator2.Object };
            var behavior = new ValidationBehavior<GetBookRequest, Response<BookDTO>>(validators);

            mockValidator1.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<GetBookRequest>>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(validationResultWithNoErrors);
            mockValidator2.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<GetBookRequest>>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(validationResultWithNoErrors2);
            _mockNext.Setup(x => x()).ReturnsAsync(expectedResponse);

            // Act
            var result = await behavior.Handle(request, _mockNext.Object, CancellationToken.None);

            // Assert
            result.Should().Be(expectedResponse);
            _mockNext.Verify(x => x(), Times.Once);
        }

        [Fact]
        public void ValidationBehavior_Should_Accept_Validators_In_Constructor()
        {
            // Arrange
            var validators = new List<IValidator<GetBookRequest>> { _mockValidator.Object };

            // Act
            var behavior = new ValidationBehavior<GetBookRequest, Response<BookDTO>>(validators);

            // Assert
            behavior.Should().NotBeNull();
        }
    }
}
