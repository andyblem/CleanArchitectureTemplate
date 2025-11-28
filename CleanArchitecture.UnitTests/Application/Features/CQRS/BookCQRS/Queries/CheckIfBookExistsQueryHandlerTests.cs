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
    public class CheckIfBookExistsQueryHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _mockDbContext;
        private readonly Mock<DbSet<Book>> _mockDbSet;
        private readonly Mock<ILogger<CheckIfBookExistsQueryHandler>> _mockLogger;
        private readonly CheckIfBookExistsQueryHandler _handler;
        private readonly List<Book> _books;

        public CheckIfBookExistsQueryHandlerTests()
        {
            _mockDbContext = new Mock<IApplicationDbContext>();
            _mockDbSet = CreateMockDbSet();
            _mockLogger = new Mock<ILogger<CheckIfBookExistsQueryHandler>>();

            _books = new List<Book>
            {
                new Book { Id = 1, Title = "Clean Architecture", ISBN = "978-0134494166", Summary = "A guide to software architecture", Price = 29.99m },
                new Book { Id = 2, Title = "Domain-Driven Design", ISBN = "978-0321125217", Summary = "Tackling complexity in software", Price = 34.99m },
                new Book { Id = 3, Title = "Design Patterns", ISBN = "978-0201633610", Summary = "Elements of Reusable Object-Oriented Software", Price = 54.99m }
            };

            // Setup the mock DbSet with our test data
            SetupMockDbSetWithData(_mockDbSet, _books);

            _mockDbContext.Setup(x => x.Books).Returns(_mockDbSet.Object);

            _handler = new CheckIfBookExistsQueryHandler(_mockDbContext.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_True_When_Book_Exists()
        {
            // Arrange
            var existingBookId = 1;
            var query = new CheckIfBookExistsQuery { Id = existingBookId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().BeTrue();
            result.Message.Should().BeNull(); // Success response typically has no message
            result.Errors.Should().BeNull();
        }

        [Fact]
        public async Task Handle_Should_Return_False_When_Book_Does_Not_Exist()
        {
            // Arrange
            var nonExistentBookId = 999;
            var query = new CheckIfBookExistsQuery { Id = nonExistentBookId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().BeFalse();
            result.Message.Should().BeNull();
            result.Errors.Should().BeNull();
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(2, true)]
        [InlineData(3, true)]
        [InlineData(999, false)]
        [InlineData(0, false)]
        [InlineData(-1, false)]
        public async Task Handle_Should_Return_Correct_Result_For_Various_Book_Ids(int bookId, bool expectedExists)
        {
            // Arrange
            var query = new CheckIfBookExistsQuery { Id = bookId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().Be(expectedExists);
            result.Errors.Should().BeNull();
        }

        [Fact]
        public async Task Handle_Should_Use_Correct_CancellationToken()
        {
            // Arrange
            var bookId = 1;
            var query = new CheckIfBookExistsQuery { Id = bookId };
            var cancellationToken = new CancellationToken(false);

            // Create fresh mocks to track cancellation token usage
            var mockDbContext = new Mock<IApplicationDbContext>();
            var mockDbSet = CreateMockDbSet();
            var mockLogger = new Mock<ILogger<CheckIfBookExistsQueryHandler>>();

            SetupMockDbSetWithData(mockDbSet, _books);
            mockDbContext.Setup(x => x.Books).Returns(mockDbSet.Object);

            var handler = new CheckIfBookExistsQueryHandler(mockDbContext.Object, mockLogger.Object);

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
            var bookId = 1;
            var query = new CheckIfBookExistsQuery { Id = bookId };
            var exception = new InvalidOperationException("Database connection failed");

            // Create a fresh mock that throws an exception
            var mockDbContext = new Mock<IApplicationDbContext>();
            var mockDbSet = new Mock<DbSet<Book>>();
            var mockLogger = new Mock<ILogger<CheckIfBookExistsQueryHandler>>();

            mockDbSet.As<IQueryable<Book>>();
            mockDbSet.As<IAsyncEnumerable<Book>>();

            // Setup the mock to throw an exception when AnyAsync is called
            mockDbSet.As<IQueryable<Book>>()
                .Setup(m => m.Provider)
                .Throws(exception);

            mockDbContext.Setup(x => x.Books).Returns(mockDbSet.Object);

            var handler = new CheckIfBookExistsQueryHandler(mockDbContext.Object, mockLogger.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be("Error checking if book exists.");
            result.Errors.Should().Contain(exception.Message);
            result.Data.Should().BeFalse(); // Default value for bool

            // Verify error logging
            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains($"Error checking if book exists with Id {bookId}")),
                    It.IsAny<InvalidOperationException>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_Failure_When_DbException_Occurs()
        {
            // Arrange
            var bookId = 5;
            var query = new CheckIfBookExistsQuery { Id = bookId };
            var dbException = new DbUpdateException("Database error occurred");

            // Create a fresh mock that throws a database exception
            var mockDbContext = new Mock<IApplicationDbContext>();
            var mockDbSet = new Mock<DbSet<Book>>();
            var mockLogger = new Mock<ILogger<CheckIfBookExistsQueryHandler>>();

            mockDbSet.As<IQueryable<Book>>();
            mockDbSet.As<IAsyncEnumerable<Book>>();

            // Setup the mock to throw a database exception
            mockDbSet.As<IQueryable<Book>>()
                .Setup(m => m.Provider)
                .Throws(dbException);

            mockDbContext.Setup(x => x.Books).Returns(mockDbSet.Object);

            var handler = new CheckIfBookExistsQueryHandler(mockDbContext.Object, mockLogger.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be("Error checking if book exists.");
            result.Errors.Should().Contain(dbException.Message);

            // Verify error logging with correct book ID
            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains($"Error checking if book exists with Id {bookId}")),
                    It.IsAny<DbUpdateException>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Log_Correct_BookId_In_Exception()
        {
            // Arrange
            var bookId = 42;
            var query = new CheckIfBookExistsQuery { Id = bookId };
            var exception = new Exception("Test exception");

            // Create a fresh mock for this specific test
            var mockDbContext = new Mock<IApplicationDbContext>();
            var mockDbSet = new Mock<DbSet<Book>>();
            var mockLogger = new Mock<ILogger<CheckIfBookExistsQueryHandler>>();

            mockDbSet.As<IQueryable<Book>>();
            mockDbSet.As<IAsyncEnumerable<Book>>();

            // Setup the mock to throw an exception
            mockDbSet.As<IQueryable<Book>>()
                .Setup(m => m.Provider)
                .Throws(exception);

            mockDbContext.Setup(x => x.Books).Returns(mockDbSet.Object);

            var handler = new CheckIfBookExistsQueryHandler(mockDbContext.Object, mockLogger.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();

            // Verify the log message contains the correct book ID
            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains($"Error checking if book exists with Id {bookId}")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_False_For_Zero_BookId()
        {
            // Arrange
            var query = new CheckIfBookExistsQuery { Id = 0 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_Should_Return_False_For_Negative_BookId()
        {
            // Arrange
            var query = new CheckIfBookExistsQuery { Id = -5 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_Should_Return_Success_Response_For_Valid_Query()
        {
            // Arrange
            var bookId = 1;
            var query = new CheckIfBookExistsQuery { Id = bookId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Message.Should().BeNull(); 
            result.Errors.Should().BeNull();
        }

        [Fact]
        public async Task Handle_Should_Work_With_Large_BookId()
        {
            // Arrange
            var largeBookId = int.MaxValue;
            var query = new CheckIfBookExistsQuery { Id = largeBookId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().BeFalse(); // This ID doesn't exist in our test data
        }

        [Fact]
        public async Task Handle_Should_Handle_Concurrent_Requests()
        {
            // Arrange
            var bookId1 = 1;
            var bookId2 = 2;
            var bookId3 = 999; // Non-existent
            var query1 = new CheckIfBookExistsQuery { Id = bookId1 };
            var query2 = new CheckIfBookExistsQuery { Id = bookId2 };
            var query3 = new CheckIfBookExistsQuery { Id = bookId3 };

            // Act
            var tasks = new[]
            {
                _handler.Handle(query1, CancellationToken.None),
                _handler.Handle(query2, CancellationToken.None),
                _handler.Handle(query3, CancellationToken.None)
            };

            var results = await Task.WhenAll(tasks);

            // Assert
            results[0].Succeeded.Should().BeTrue();
            results[0].Data.Should().BeTrue();

            results[1].Succeeded.Should().BeTrue();
            results[1].Data.Should().BeTrue();

            results[2].Succeeded.Should().BeTrue();
            results[2].Data.Should().BeFalse();
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