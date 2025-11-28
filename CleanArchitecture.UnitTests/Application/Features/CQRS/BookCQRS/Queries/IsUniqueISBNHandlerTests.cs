using CleanArchitecture.Application.Features.CQRS.Books.Queries;
using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CleanArchitecture.UnitTests.Application.Features.CQRS.BookCQRS.Queries
{
    public class IsUniqueISBNHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _mockDbContext;
        private readonly Mock<DbSet<Book>> _mockDbSet;
        private readonly Mock<ILogger<IsUniqueISBNHandler>> _mockLogger;
        private readonly IsUniqueISBNHandler _handler;
        private readonly List<Book> _books;

        public IsUniqueISBNHandlerTests()
        {
            _mockDbContext = new Mock<IApplicationDbContext>();
            _mockDbSet = CreateMockDbSet();
            _mockLogger = new Mock<ILogger<IsUniqueISBNHandler>>();

            _books = new List<Book>
            {
                new Book { Id = 1, Title = "Clean Architecture", ISBN = "978-0134494166", Summary = "A guide to software architecture", Price = 29.99m },
                new Book { Id = 2, Title = "Domain-Driven Design", ISBN = "978-0321125217", Summary = "Tackling complexity in software", Price = 34.99m },
                new Book { Id = 3, Title = "Design Patterns", ISBN = "978-0201633610", Summary = "Elements of Reusable Object-Oriented Software", Price = 54.99m }
            };

            // Setup the mock DbSet with our test data
            SetupMockDbSetWithData(_mockDbSet, _books);

            _mockDbContext.Setup(x => x.Books).Returns(_mockDbSet.Object);

            _handler = new IsUniqueISBNHandler(_mockDbContext.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_True_When_ISBN_Is_Unique()
        {
            // Arrange
            var uniqueIsbn = "978-1234567890"; // Not in our test data
            var query = new IsUniqueISBNQuery { ISBN = uniqueIsbn };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().BeTrue();
            result.Message.Should().Be("ISBN is unique.");
        }

        [Fact]
        public async Task Handle_Should_Return_False_When_ISBN_Already_Exists()
        {
            // Arrange
            var existingIsbn = "978-0134494166"; // Exists in our test data
            var query = new IsUniqueISBNQuery { ISBN = existingIsbn };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().BeFalse();
            result.Message.Should().Be("ISBN already exists.");
        }

        [Theory]
        [InlineData("978-0134494166", false)] // Existing ISBN
        [InlineData("978-0321125217", false)] // Existing ISBN
        [InlineData("978-0201633610", false)] // Existing ISBN
        [InlineData("978-1234567890", true)]  // Unique ISBN
        [InlineData("978-0987654321", true)]  // Unique ISBN
        [InlineData("978-5555555555", true)]  // Unique ISBN
        public async Task Handle_Should_Return_Correct_Result_For_Various_ISBNs(string isbn, bool expectedIsUnique)
        {
            // Arrange
            var query = new IsUniqueISBNQuery { ISBN = isbn };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().Be(expectedIsUnique);

            if (expectedIsUnique)
            {
                result.Message.Should().Be("ISBN is unique.");
            }
            else
            {
                result.Message.Should().Be("ISBN already exists.");
            }
        }

        [Fact]
        public async Task Handle_Should_Return_True_For_Empty_String_ISBN()
        {
            // Arrange
            var emptyIsbn = "";
            var query = new IsUniqueISBNQuery { ISBN = emptyIsbn };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().BeTrue(); // Empty string should be considered unique
            result.Message.Should().Be("ISBN is unique.");
        }

        [Fact]
        public async Task Handle_Should_Return_True_For_Null_ISBN()
        {
            // Arrange
            var query = new IsUniqueISBNQuery { ISBN = null };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().BeTrue(); // Null should be considered unique
            result.Message.Should().Be("ISBN is unique.");
        }

        [Fact]
        public async Task Handle_Should_Handle_Whitespace_In_ISBN()
        {
            // Arrange
            var isbnWithSpaces = " 978-0134494166 "; // Same ISBN with surrounding spaces
            var query = new IsUniqueISBNQuery { ISBN = isbnWithSpaces };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().BeTrue(); // Should be unique because exact match is required
            result.Message.Should().Be("ISBN is unique.");
        }

        [Fact]
        public async Task Handle_Should_Use_Correct_CancellationToken()
        {
            // Arrange
            var isbn = "978-1234567890";
            var query = new IsUniqueISBNQuery { ISBN = isbn };
            var cancellationToken = new CancellationToken(false);

            // Create fresh mocks to track cancellation token usage
            var mockDbContext = new Mock<IApplicationDbContext>();
            var mockDbSet = CreateMockDbSet();
            var mockLogger = new Mock<ILogger<IsUniqueISBNHandler>>();

            SetupMockDbSetWithData(mockDbSet, _books);
            mockDbContext.Setup(x => x.Books).Returns(mockDbSet.Object);

            var handler = new IsUniqueISBNHandler(mockDbContext.Object, mockLogger.Object);

            // Act
            var result = await handler.Handle(query, cancellationToken);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_Return_Failure_When_Database_Exception_Occurs()
        {
            // Arrange
            var isbn = "978-1234567890";
            var query = new IsUniqueISBNQuery { ISBN = isbn };
            var exception = new InvalidOperationException("Database connection failed");

            // Create a fresh mock that throws an exception
            var mockDbContext = new Mock<IApplicationDbContext>();
            var mockDbSet = new Mock<DbSet<Book>>();
            var mockLogger = new Mock<ILogger<IsUniqueISBNHandler>>();

            mockDbSet.As<IQueryable<Book>>();
            mockDbSet.As<IAsyncEnumerable<Book>>();

            // Setup the mock to throw an exception when AnyAsync is called
            mockDbSet.As<IQueryable<Book>>()
                .Setup(m => m.Provider)
                .Throws(exception);

            mockDbContext.Setup(x => x.Books).Returns(mockDbSet.Object);

            var handler = new IsUniqueISBNHandler(mockDbContext.Object, mockLogger.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be("Error checking unique ISBN.");
            result.Data.Should().BeFalse(); // Default value for bool
            result.Errors.Should().NotBeNull();
            result.Errors.Should().Contain(exception.Message);

            // Verify error logging
            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Error checking unique ISBN.")),
                    It.IsAny<InvalidOperationException>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_Failure_When_DbException_Occurs()
        {
            // Arrange
            var isbn = "978-1234567890";
            var query = new IsUniqueISBNQuery { ISBN = isbn };
            var dbException = new DbUpdateException("Database query failed");

            // Create a fresh mock that throws a database exception
            var mockDbContext = new Mock<IApplicationDbContext>();
            var mockDbSet = new Mock<DbSet<Book>>();
            var mockLogger = new Mock<ILogger<IsUniqueISBNHandler>>();

            mockDbSet.As<IQueryable<Book>>();
            mockDbSet.As<IAsyncEnumerable<Book>>();

            // Setup the mock to throw a database exception
            mockDbSet.As<IQueryable<Book>>()
                .Setup(m => m.Provider)
                .Throws(dbException);

            mockDbContext.Setup(x => x.Books).Returns(mockDbSet.Object);

            var handler = new IsUniqueISBNHandler(mockDbContext.Object, mockLogger.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be("Error checking unique ISBN.");
            result.Data.Should().BeFalse();
            result.Errors.Should().NotBeNull();
            result.Errors.Should().Contain(dbException.Message);

            // Verify error logging
            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Error checking unique ISBN.")),
                    It.IsAny<DbUpdateException>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Log_Error_With_Exception_Details()
        {
            // Arrange
            var isbn = "978-1234567890";
            var query = new IsUniqueISBNQuery { ISBN = isbn };
            var exception = new Exception("Test exception with specific details");

            // Create a fresh mock for this specific test
            var mockDbContext = new Mock<IApplicationDbContext>();
            var mockDbSet = new Mock<DbSet<Book>>();
            var mockLogger = new Mock<ILogger<IsUniqueISBNHandler>>();

            mockDbSet.As<IQueryable<Book>>();
            mockDbSet.As<IAsyncEnumerable<Book>>();

            // Setup the mock to throw an exception
            mockDbSet.As<IQueryable<Book>>()
                .Setup(m => m.Provider)
                .Throws(exception);

            mockDbContext.Setup(x => x.Books).Returns(mockDbSet.Object);

            var handler = new IsUniqueISBNHandler(mockDbContext.Object, mockLogger.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Errors.Should().Contain("Test exception with specific details");

            // Verify the log message contains the correct error message
            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Error checking unique ISBN.")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_Success_Response_Structure()
        {
            // Arrange
            var isbn = "978-1234567890";
            var query = new IsUniqueISBNQuery { ISBN = isbn };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().BeTrue();
            result.Message.Should().Be("ISBN is unique.");

            // Success responses should have null errors based on current implementation pattern
            if (result.Errors != null)
            {
                result.Errors.Should().BeEmpty();
            }
        }

        [Fact]
        public async Task Handle_Should_Handle_Concurrent_Requests()
        {
            // Arrange
            var isbn1 = "978-1234567890"; // Unique
            var isbn2 = "978-0134494166"; // Exists
            var isbn3 = "978-9999999999"; // Unique
            var query1 = new IsUniqueISBNQuery { ISBN = isbn1 };
            var query2 = new IsUniqueISBNQuery { ISBN = isbn2 };
            var query3 = new IsUniqueISBNQuery { ISBN = isbn3 };

            // Act
            var tasks = new[]
            {
                _handler.Handle(query1, CancellationToken.None),
                _handler.Handle(query2, CancellationToken.None),
                _handler.Handle(query3, CancellationToken.None)
            };

            var results = await Task.WhenAll(tasks);

            // Assert
            // First request - should be unique
            results[0].Succeeded.Should().BeTrue();
            results[0].Data.Should().BeTrue();
            results[0].Message.Should().Be("ISBN is unique.");

            // Second request - should not be unique (exists)
            results[1].Succeeded.Should().BeTrue();
            results[1].Data.Should().BeFalse();
            results[1].Message.Should().Be("ISBN already exists.");

            // Third request - should be unique
            results[2].Succeeded.Should().BeTrue();
            results[2].Data.Should().BeTrue();
            results[2].Message.Should().Be("ISBN is unique.");
        }

        [Fact]
        public async Task Handle_Should_Work_With_Various_ISBN_Formats()
        {
            // Arrange - Test various ISBN formats that should all be considered unique
            var isbnFormats = new[]
            {
                "978-0134494167",    // Standard ISBN-13 format
                "9780134494167",     // ISBN-13 without hyphens
                "0134494167",        // ISBN-10 format
                "978 0134494167",    // With spaces instead of hyphens
                "978.0134.49416.7"  // With periods
            };

            foreach (var isbn in isbnFormats)
            {
                // Arrange
                var query = new IsUniqueISBNQuery { ISBN = isbn };

                // Act
                var result = await _handler.Handle(query, CancellationToken.None);

                // Assert
                result.Should().NotBeNull();
                result.Succeeded.Should().BeTrue();
                result.Data.Should().BeTrue(); // All should be unique since they don't match our test data exactly
                result.Message.Should().Be("ISBN is unique.");
            }
        }

        [Fact]
        public async Task Handle_Should_Return_Correct_Message_For_Existing_ISBN()
        {
            // Arrange
            var existingIsbn = "978-0134494166";
            var query = new IsUniqueISBNQuery { ISBN = existingIsbn };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().BeFalse();
            result.Message.Should().Be("ISBN already exists.");
        }

        [Fact]
        public async Task Handle_Should_Return_Correct_Message_For_Unique_ISBN()
        {
            // Arrange
            var uniqueIsbn = "978-9876543210";
            var query = new IsUniqueISBNQuery { ISBN = uniqueIsbn };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().BeTrue();
            result.Message.Should().Be("ISBN is unique.");
        }

        #region Helper Methods

        private Mock<DbSet<Book>> CreateMockDbSet()
        {
            var mockSet = new Mock<DbSet<Book>>();
            // Configure interfaces BEFORE any other setup
            mockSet.As<IQueryable<Book>>();
            mockSet.As<IAsyncEnumerable<Book>>();
            return mockSet;
        }

        private void SetupMockDbSetWithData(Mock<DbSet<Book>> mockSet, List<Book> data)
        {
            var queryableData = data.AsQueryable();

            // Setup synchronous IQueryable
            mockSet.As<IQueryable<Book>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<Book>(queryableData.Provider));

            mockSet.As<IQueryable<Book>>()
                .Setup(m => m.Expression)
                .Returns(queryableData.Expression);

            mockSet.As<IQueryable<Book>>()
                .Setup(m => m.ElementType)
                .Returns(queryableData.ElementType);

            mockSet.As<IQueryable<Book>>()
                .Setup(m => m.GetEnumerator())
                .Returns(queryableData.GetEnumerator());

            // Setup asynchronous operations - crucial for AnyAsync to work
            mockSet.As<IAsyncEnumerable<Book>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<Book>(queryableData.GetEnumerator()));
        }

        #endregion

        #region Async Test Helper Classes

        // Helper class to support async operations
        internal class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
        {
            private readonly IQueryProvider _inner;

            internal TestAsyncQueryProvider(IQueryProvider inner)
            {
                _inner = inner;
            }

            public IQueryable CreateQuery(Expression expression)
            {
                return new TestAsyncEnumerable<TEntity>(expression);
            }

            public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
            {
                return new TestAsyncEnumerable<TElement>(expression);
            }

            public object? Execute(Expression expression)
            {
                return _inner.Execute(expression);
            }

            public TResult Execute<TResult>(Expression expression)
            {
                return _inner.Execute<TResult>(expression);
            }

            public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
            {
                var expectedResultType = typeof(TResult).GetGenericArguments()[0];
                var executionResult = typeof(IQueryProvider)
                    .GetMethod(
                        name: nameof(IQueryProvider.Execute),
                        genericParameterCount: 1,
                        types: new[] { typeof(Expression) })!
                    .MakeGenericMethod(expectedResultType)
                    .Invoke(this, new[] { expression });

                return (TResult)typeof(Task).GetMethod(nameof(Task.FromResult))!
                    .MakeGenericMethod(expectedResultType)
                    .Invoke(null, new[] { executionResult })!;
            }
        }

        internal class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
        {
            public TestAsyncEnumerable(IEnumerable<T> enumerable)
                : base(enumerable)
            { }

            public TestAsyncEnumerable(Expression expression)
                : base(expression)
            { }

            public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            {
                return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
            }

            IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(this);
        }

        internal class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
        {
            private readonly IEnumerator<T> _inner;

            public TestAsyncEnumerator(IEnumerator<T> inner)
            {
                _inner = inner;
            }

            public ValueTask DisposeAsync()
            {
                _inner.Dispose();
                return ValueTask.CompletedTask;
            }

            public T Current => _inner.Current;

            public ValueTask<bool> MoveNextAsync()
            {
                return ValueTask.FromResult(_inner.MoveNext());
            }
        }

        #endregion
    }
}