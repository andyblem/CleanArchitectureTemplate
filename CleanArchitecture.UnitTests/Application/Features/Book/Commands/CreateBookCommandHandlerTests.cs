using CleanArchitecture.Application.Features.Books.Commands;
using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Application.Wrappers;
using CleanArchitecture.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CleanArchitecture.UnitTests.Application.Features.Book.Commands
{
    public class CreateBookCommandHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _mockDbContext;
        private readonly Mock<ILogger<CreateBookCommandHandler>> _mockLogger;
        private readonly Mock<DbSet<Domain.Entities.Book>> _mockDbSet;
        private readonly CreateBookCommandHandler _handler;

        public CreateBookCommandHandlerTests()
        {
            _mockDbContext = new Mock<IApplicationDbContext>();
            _mockLogger = new Mock<ILogger<CreateBookCommandHandler>>();
            _mockDbSet = new Mock<DbSet<Domain.Entities.Book>>();

            _mockDbContext.Setup(x => x.Books).Returns(_mockDbSet.Object);

            _handler = new CreateBookCommandHandler(_mockDbContext.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_Should_Create_Book_Successfully_And_Return_Success_Response()
        {
            // Arrange
            var book = new Domain.Entities.Book
            {
                Id = 1,
                Title = "Clean Architecture",
                ISBN = "978-0134494166",
                Summary = "A comprehensive guide to software architecture",
                Price = 39.99m
            };

            var command = new CreateBookCommand { Book = book };
            var cancellationToken = CancellationToken.None;

            _mockDbContext.Setup(x => x.SaveChangesAsync(cancellationToken))
                         .ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().Be(book.Id);
            result.Message.Should().BeNull();
            result.Errors.Should().BeNull();

            _mockDbSet.Verify(x => x.AddAsync(book, cancellationToken), Times.Once);
            _mockDbContext.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Handle_DbUpdateException_And_Return_ISBN_Exists_Error()
        {
            // Arrange
            var book = new Domain.Entities.Book
            {
                Title = "Duplicate Book",
                ISBN = "978-0134494166", // Duplicate ISBN
                Summary = "Test summary",
                Price = 29.99m
            };

            var command = new CreateBookCommand { Book = book };
            var cancellationToken = CancellationToken.None;
            var dbUpdateException = new DbUpdateException("Duplicate entry");

            _mockDbContext.Setup(x => x.SaveChangesAsync(cancellationToken))
                         .ThrowsAsync(dbUpdateException);

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be("ISBN already exists.");
            result.Errors.Should().HaveCount(1);
            result.Errors.Should().Contain("ISBN already exists.");
            result.Data.Should().Be(0); // Default value for int

            _mockDbSet.Verify(x => x.AddAsync(book, cancellationToken), Times.Once);
            _mockDbContext.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);

            // Verify logging
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Database update failed")),
                    dbUpdateException,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Handle_General_Exception_And_Return_Error_Response()
        {
            // Arrange
            var book = new Domain.Entities.Book
            {
                Title = "Test Book",
                ISBN = "978-0123456789",
                Summary = "Test summary",
                Price = 19.99m
            };

            var command = new CreateBookCommand { Book = book };
            var cancellationToken = CancellationToken.None;
            var exception = new InvalidOperationException("Database connection failed");

            _mockDbContext.Setup(x => x.SaveChangesAsync(cancellationToken))
                         .ThrowsAsync(exception);

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be("Database connection failed");
            result.Errors.Should().HaveCount(1);
            result.Errors.Should().Contain("Database connection failed");
            result.Data.Should().Be(0); // Default value for int

            _mockDbSet.Verify(x => x.AddAsync(book, cancellationToken), Times.Once);
            _mockDbContext.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);

            // Verify logging
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error creating book")),
                    exception,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Pass_Cancellation_Token_To_DbContext_Methods()
        {
            // Arrange
            var book = new Domain.Entities.Book
            {
                Title = "Token Test Book",
                ISBN = "978-0999999999",
                Summary = "Testing cancellation token",
                Price = 15.99m
            };

            var command = new CreateBookCommand { Book = book };
            var cancellationToken = new CancellationToken();

            _mockDbContext.Setup(x => x.SaveChangesAsync(cancellationToken))
                         .ReturnsAsync(1);

            // Act
            await _handler.Handle(command, cancellationToken);

            // Assert
            _mockDbSet.Verify(x => x.AddAsync(book, cancellationToken), Times.Once);
            _mockDbContext.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Create_Book_With_All_Properties_Set()
        {
            // Arrange
            var book = new Domain.Entities.Book
            {
                Id = 42,
                Title = "Advanced C# Programming",
                ISBN = "978-1234567890",
                Summary = "Learn advanced C# concepts and patterns for modern software development.",
                Price = 59.99m
            };

            var command = new CreateBookCommand { Book = book };
            var cancellationToken = CancellationToken.None;

            _mockDbContext.Setup(x => x.SaveChangesAsync(cancellationToken))
                         .ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().Be(42);

            _mockDbSet.Verify(x => x.AddAsync(
                It.Is<Domain.Entities.Book>(b =>
                    b.Title == "Advanced C# Programming" &&
                    b.ISBN == "978-1234567890" &&
                    b.Summary == "Learn advanced C# concepts and patterns for modern software development." &&
                    b.Price == 59.99m),
                cancellationToken), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Work_With_Minimum_Required_Book_Properties()
        {
            // Arrange
            var book = new Domain.Entities.Book
            {
                Title = "Minimal Book",
                ISBN = "MIN-001",
                Summary = "Short",
                Price = 0.01m
            };

            var command = new CreateBookCommand { Book = book };
            var cancellationToken = CancellationToken.None;

            _mockDbContext.Setup(x => x.SaveChangesAsync(cancellationToken))
                         .ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().Be(book.Id);
        }

        [Fact]
        public async Task Handle_Should_Handle_Book_With_Special_Characters()
        {
            // Arrange
            var book = new Domain.Entities.Book
            {
                Title = "C# & .NET: Special Characters!@#$%^&*()",
                ISBN = "SPEC-123-456",
                Summary = "A book with special chars: <>{}[]|\\/:;\"'",
                Price = 45.50m
            };

            var command = new CreateBookCommand { Book = book };
            var cancellationToken = CancellationToken.None;

            _mockDbContext.Setup(x => x.SaveChangesAsync(cancellationToken))
                         .ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().Be(book.Id);
        }

        [Fact]
        public async Task Handle_Should_Handle_DbUpdateException_With_Inner_Exception()
        {
            // Arrange
            var book = new Domain.Entities.Book
            {
                Title = "Test Book",
                ISBN = "978-0000000000",
                Summary = "Test",
                Price = 10.00m
            };

            var command = new CreateBookCommand { Book = book };
            var cancellationToken = CancellationToken.None;

            var innerException = new Exception("Constraint violation");
            var dbUpdateException = new DbUpdateException("Database constraint failed", innerException);

            _mockDbContext.Setup(x => x.SaveChangesAsync(cancellationToken))
                         .ThrowsAsync(dbUpdateException);

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be("ISBN already exists.");
            result.Errors.Should().Contain("ISBN already exists.");
        }

        [Fact]
        public async Task Handle_Should_Return_Book_Id_Zero_When_Book_Id_Is_Not_Set()
        {
            // Arrange
            var book = new Domain.Entities.Book
            {
                // Id is not set, should default to 0
                Title = "No ID Book",
                ISBN = "NO-ID-001",
                Summary = "Book without explicit ID",
                Price = 25.00m
            };

            var command = new CreateBookCommand { Book = book };
            var cancellationToken = CancellationToken.None;

            _mockDbContext.Setup(x => x.SaveChangesAsync(cancellationToken))
                         .ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().Be(0); // Default value when Id is not set
        }

        [Theory]
        [InlineData(0.01)]
        [InlineData(9.99)]
        [InlineData(100.00)]
        [InlineData(999.99)]
        [InlineData(1000000.00)]
        public async Task Handle_Should_Accept_Various_Price_Values(decimal price)
        {
            // Arrange
            var book = new Domain.Entities.Book
            {
                Title = "Price Test Book",
                ISBN = $"PRICE-{price:F2}",
                Summary = "Testing price values",
                Price = price
            };

            var command = new CreateBookCommand { Book = book };
            var cancellationToken = CancellationToken.None;

            _mockDbContext.Setup(x => x.SaveChangesAsync(cancellationToken))
                         .ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_Handle_ArgumentNullException()
        {
            // Arrange
            var book = new Domain.Entities.Book
            {
                Title = "Exception Test",
                ISBN = "EXC-001",
                Summary = "Testing exceptions",
                Price = 20.00m
            };

            var command = new CreateBookCommand { Book = book };
            var cancellationToken = CancellationToken.None;
            var argumentNullException = new ArgumentNullException("connectionString", "Connection string cannot be null");

            _mockDbContext.Setup(x => x.SaveChangesAsync(cancellationToken))
                         .ThrowsAsync(argumentNullException);

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Message.Should().Contain(argumentNullException.Message);
            result.Errors.Should().Contain(argumentNullException.Message);
        }
    }
}