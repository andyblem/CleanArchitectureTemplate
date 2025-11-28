using CleanArchitecture.Application.Features.CQRS.Books.Commands;
using CleanArchitecture.Application.Interfaces;
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

namespace CleanArchitecture.UnitTests.Application.Features.CQRS.BookCQRS.Commands
{
    public class CreateBookCommandHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _mockDbContext;
        private readonly Mock<DbSet<Book>> _mockDbSet;
        private readonly Mock<ILogger<CreateBookCommandHandler>> _mockLogger;
        private readonly CreateBookCommandHandler _handler;

        public CreateBookCommandHandlerTests()
        {
            _mockDbContext = new Mock<IApplicationDbContext>();
            _mockDbSet = new Mock<DbSet<Book>>();
            _mockLogger = new Mock<ILogger<CreateBookCommandHandler>>();
            
            // Setup the DbSet mock
            _mockDbContext.Setup(x => x.Books).Returns(_mockDbSet.Object);
            
            _handler = new CreateBookCommandHandler(_mockDbContext.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_Should_Create_Book_Successfully()
        {
            // Arrange
            var book = new Book
            {
                Id = 1,
                Title = "Clean Architecture",
                ISBN = "978-0134494166",
                Summary = "A guide to software architecture",
                Price = 29.99m
            };

            var command = new CreateBookCommand { Book = book };
            
            _mockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                         .ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().Be(book.Id);
            
            // Verify database operations
            _mockDbSet.Verify(x => x.AddAsync(book, It.IsAny<CancellationToken>()), Times.Once);
            _mockDbContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_Failure_When_DbUpdateException_Occurs()
        {
            // Arrange
            var book = new Book { Title = "Test", ISBN = "978-0123456789", Summary = "Test", Price = 10.99m };
            var command = new CreateBookCommand { Book = book };
            
            _mockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                         .ThrowsAsync(new DbUpdateException("ISBN already exists"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be("ISBN already exists.");
            
            // Verify logging
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Database update failed")),
                    It.IsAny<DbUpdateException>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_Failure_When_Exception_Occurs()
        {
            // Arrange
            var book = new Book { Title = "Test", ISBN = "123", Summary = "Test", Price = 10.99m };
            var command = new CreateBookCommand { Book = book };
            var exception = new InvalidOperationException("Database error");
            
            _mockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                         .ThrowsAsync(exception);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be("Error creating book");
            result.Errors.Should().Contain(exception.Message);
            
            // Verify error logging
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error creating book")),
                    It.IsAny<InvalidOperationException>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_Failure_When_Book_Is_Null()
        {
            // Arrange
            var command = new CreateBookCommand { Book = null };

            // Mock AddAsync to throw ArgumentNullException when null is passed
            _mockDbSet.Setup(x => x.AddAsync(null, It.IsAny<CancellationToken>()))
                      .ThrowsAsync(new ArgumentNullException("book"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be("Error creating book");
            result.Errors.First().Should().Contain("book");

            // Verify that AddAsync was called with null
            _mockDbSet.Verify(x => x.AddAsync(null, It.IsAny<CancellationToken>()), Times.Once);

            // Verify error was logged
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error creating book")),
                    It.IsAny<ArgumentNullException>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Theory]
        [InlineData("", "Test ISBN", "Test Summary", 10.99)]
        [InlineData("Test Title", "", "Test Summary", 10.99)]
        [InlineData("Test Title", "Test ISBN", "", 10.99)]
        [InlineData("Test Title", "Test ISBN", "Test Summary", 0)]
        public async Task Handle_Should_Create_Book_With_Various_Properties(string title, string isbn, string summary, decimal price)
        {
            // Arrange
            var book = new Book { Title = title, ISBN = isbn, Summary = summary, Price = price };
            var command = new CreateBookCommand { Book = book };
            
            _mockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                         .ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            
            // Verify the book was added
            _mockDbSet.Verify(x => x.AddAsync(book, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_Success_With_Correct_Message()
        {
            // Arrange
            var book = new Book 
            { 
                Id = 5, 
                Title = "Test", 
                ISBN = "123", 
                Summary = "Test", 
                Price = 15.99m 
            };
            var command = new CreateBookCommand { Book = book };
            
            _mockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                         .ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().Be(book.Id);
            result.Message.Should().BeNull(); // The Success factory method sets message to null by default
        }

        [Fact]
        public async Task Handle_Should_Call_SaveChangesAsync_After_AddAsync()
        {
            // Arrange
            var book = new Book { Title = "Test", ISBN = "123", Summary = "Test", Price = 10.99m };
            var command = new CreateBookCommand { Book = book };
            
            var sequence = new MockSequence();
            _mockDbSet.InSequence(sequence).Setup(x => x.AddAsync(book, It.IsAny<CancellationToken>()));
            _mockDbContext.InSequence(sequence).Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            
            // Verify the sequence was called correctly
            _mockDbSet.Verify(x => x.AddAsync(book, It.IsAny<CancellationToken>()), Times.Once);
            _mockDbContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}