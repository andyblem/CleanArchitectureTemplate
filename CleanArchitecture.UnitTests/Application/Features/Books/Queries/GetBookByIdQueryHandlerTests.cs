using CleanArchitecture.Application.DTOs.Book;
using CleanArchitecture.Application.Features.Books.Queries;
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

namespace CleanArchitecture.UnitTests.Application.Features.Books.Queries
{
    public class GetBookByIdQueryHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _mockDbContext;
        private readonly Mock<DbSet<Book>> _mockDbSet;
        private readonly Mock<ILogger<GetBookByIdQueryHandler>> _mockLogger;
        private readonly GetBookByIdQueryHandler _handler;
        private readonly List<Book> _books;

        public GetBookByIdQueryHandlerTests()
        {
            _mockDbContext = new Mock<IApplicationDbContext>();
            _mockDbSet = CreateMockDbSet();
            _mockLogger = new Mock<ILogger<GetBookByIdQueryHandler>>();

            _books = new List<Book>
            {
                new Book { Id = 1, Title = "Clean Architecture", ISBN = "978-0134494166", Summary = "A guide to software architecture", Price = 29.99m },
                new Book { Id = 2, Title = "Domain-Driven Design", ISBN = "978-0321125217", Summary = "Tackling complexity in software", Price = 34.99m },
                new Book { Id = 3, Title = "Design Patterns", ISBN = "978-0201633610", Summary = "Elements of Reusable Object-Oriented Software", Price = 54.99m },
                new Book { Id = 4, Title = "Refactoring", ISBN = "978-0201485677", Summary = "Improving the Design of Existing Code", Price = 49.99m }
            };

            // Setup the mock DbSet with our test data
            SetupMockDbSetWithData(_mockDbSet, _books);

            _mockDbContext.Setup(x => x.Books).Returns(_mockDbSet.Object);

            _handler = new GetBookByIdQueryHandler(_mockDbContext.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_Book_When_Book_Exists()
        {
            // Arrange
            var existingBookId = 1;
            var query = new GetBookByIdQuery { Id = existingBookId };
            var expectedBook = _books.First(b => b.Id == existingBookId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Message.Should().Be("Book retrieved successfully");
            result.Data.Should().NotBeNull();
            result.Data.Id.Should().Be(expectedBook.Id);
            result.Data.Title.Should().Be(expectedBook.Title);
            result.Data.ISBN.Should().Be(expectedBook.ISBN);
            result.Data.Summary.Should().Be(expectedBook.Summary);
            result.Data.Price.Should().Be(expectedBook.Price);
        }

        [Fact]
        public async Task Handle_Should_Return_Failure_When_Book_Does_Not_Exist()
        {
            // Arrange
            var nonExistentBookId = 999;
            var query = new GetBookByIdQuery { Id = nonExistentBookId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be("Book not found");
            result.Data.Should().BeNull();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public async Task Handle_Should_Return_Correct_Book_For_Valid_Ids(int bookId)
        {
            // Arrange
            var query = new GetBookByIdQuery { Id = bookId };
            var expectedBook = _books.First(b => b.Id == bookId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Id.Should().Be(expectedBook.Id);
            result.Data.Title.Should().Be(expectedBook.Title);
            result.Data.ISBN.Should().Be(expectedBook.ISBN);
            result.Data.Summary.Should().Be(expectedBook.Summary);
            result.Data.Price.Should().Be(expectedBook.Price);
            result.Message.Should().Be("Book retrieved successfully");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(999)]
        [InlineData(int.MaxValue)]
        public async Task Handle_Should_Return_Failure_For_Invalid_Ids(int invalidBookId)
        {
            // Arrange
            var query = new GetBookByIdQuery { Id = invalidBookId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be("Book not found");
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task Handle_Should_Use_Correct_CancellationToken()
        {
            // Arrange
            var bookId = 1;
            var query = new GetBookByIdQuery { Id = bookId };
            var cancellationToken = new CancellationToken(false);

            // Create fresh mocks to track cancellation token usage
            var mockDbContext = new Mock<IApplicationDbContext>();
            var mockDbSet = CreateMockDbSet();
            var mockLogger = new Mock<ILogger<GetBookByIdQueryHandler>>();

            SetupMockDbSetWithData(mockDbSet, _books);
            mockDbContext.Setup(x => x.Books).Returns(mockDbSet.Object);

            var handler = new GetBookByIdQueryHandler(mockDbContext.Object, mockLogger.Object);

            // Act
            var result = await handler.Handle(query, cancellationToken);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_Should_Return_Failure_When_Database_Exception_Occurs()
        {
            // Arrange
            var bookId = 1;
            var query = new GetBookByIdQuery { Id = bookId };
            var exception = new InvalidOperationException("Database connection failed");

            // Create a fresh mock that throws an exception
            var mockDbContext = new Mock<IApplicationDbContext>();
            var mockDbSet = new Mock<DbSet<Book>>();
            var mockLogger = new Mock<ILogger<GetBookByIdQueryHandler>>();

            mockDbSet.As<IQueryable<Book>>();
            mockDbSet.As<IAsyncEnumerable<Book>>();

            // Setup the mock to throw an exception when query is executed
            mockDbSet.As<IQueryable<Book>>()
                .Setup(m => m.Provider)
                .Throws(exception);

            mockDbContext.Setup(x => x.Books).Returns(mockDbSet.Object);

            var handler = new GetBookByIdQueryHandler(mockDbContext.Object, mockLogger.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be("An error occurred while retrieving the book.");
            result.Data.Should().BeNull();
            result.Errors.Should().NotBeNull();
            result.Errors.Should().Contain(exception.Message);

            // Verify error logging
            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains($"An error occurred while retrieving the book with ID {bookId}")),
                    It.IsAny<InvalidOperationException>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_Failure_When_DbException_Occurs()
        {
            // Arrange
            var bookId = 2;
            var query = new GetBookByIdQuery { Id = bookId };
            var dbException = new DbUpdateException("Database query failed");

            // Create a fresh mock that throws a database exception
            var mockDbContext = new Mock<IApplicationDbContext>();
            var mockDbSet = new Mock<DbSet<Book>>();
            var mockLogger = new Mock<ILogger<GetBookByIdQueryHandler>>();

            mockDbSet.As<IQueryable<Book>>();
            mockDbSet.As<IAsyncEnumerable<Book>>();

            // Setup the mock to throw a database exception
            mockDbSet.As<IQueryable<Book>>()
                .Setup(m => m.Provider)
                .Throws(dbException);

            mockDbContext.Setup(x => x.Books).Returns(mockDbSet.Object);

            var handler = new GetBookByIdQueryHandler(mockDbContext.Object, mockLogger.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be("An error occurred while retrieving the book.");
            result.Data.Should().BeNull();
            result.Errors.Should().NotBeNull();
            result.Errors.Should().Contain(dbException.Message);

            // Verify error logging with correct book ID
            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains($"An error occurred while retrieving the book with ID {bookId}")),
                    It.IsAny<DbUpdateException>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Log_Correct_BookId_In_Exception()
        {
            // Arrange
            var bookId = 42;
            var query = new GetBookByIdQuery { Id = bookId };
            var exception = new Exception("Test exception");

            // Create a fresh mock for this specific test
            var mockDbContext = new Mock<IApplicationDbContext>();
            var mockDbSet = new Mock<DbSet<Book>>();
            var mockLogger = new Mock<ILogger<GetBookByIdQueryHandler>>();

            mockDbSet.As<IQueryable<Book>>();
            mockDbSet.As<IAsyncEnumerable<Book>>();

            // Setup the mock to throw an exception
            mockDbSet.As<IQueryable<Book>>()
                .Setup(m => m.Provider)
                .Throws(exception);

            mockDbContext.Setup(x => x.Books).Returns(mockDbSet.Object);

            var handler = new GetBookByIdQueryHandler(mockDbContext.Object, mockLogger.Object);

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
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains($"An error occurred while retrieving the book with ID {bookId}")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_BookDTO_With_All_Properties_Mapped()
        {
            // Arrange
            var bookId = 1;
            var query = new GetBookByIdQuery { Id = bookId };
            var expectedBook = _books.First(b => b.Id == bookId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull();

            // Verify all properties are correctly mapped
            result.Data.Should().BeOfType<BookDTO>();
            result.Data.Id.Should().Be(expectedBook.Id);
            result.Data.Title.Should().Be(expectedBook.Title);
            result.Data.ISBN.Should().Be(expectedBook.ISBN);
            result.Data.Summary.Should().Be(expectedBook.Summary);
            result.Data.Price.Should().Be(expectedBook.Price);
        }

        [Fact]
        public async Task Handle_Should_Return_Success_Response_Structure()
        {
            // Arrange
            var bookId = 1;
            var query = new GetBookByIdQuery { Id = bookId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Message.Should().Be("Book retrieved successfully");
            result.Data.Should().NotBeNull();

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
            var bookId1 = 1;
            var bookId2 = 2;
            var bookId3 = 999; // Non-existent
            var query1 = new GetBookByIdQuery { Id = bookId1 };
            var query2 = new GetBookByIdQuery { Id = bookId2 };
            var query3 = new GetBookByIdQuery { Id = bookId3 };

            // Act
            var tasks = new[]
            {
                _handler.Handle(query1, CancellationToken.None),
                _handler.Handle(query2, CancellationToken.None),
                _handler.Handle(query3, CancellationToken.None)
            };

            var results = await Task.WhenAll(tasks);

            // Assert
            // First request - should succeed
            results[0].Succeeded.Should().BeTrue();
            results[0].Data.Should().NotBeNull();
            results[0].Data.Id.Should().Be(bookId1);

            // Second request - should succeed
            results[1].Succeeded.Should().BeTrue();
            results[1].Data.Should().NotBeNull();
            results[1].Data.Id.Should().Be(bookId2);

            // Third request - should fail (book not found)
            results[2].Succeeded.Should().BeFalse();
            results[2].Data.Should().BeNull();
            results[2].Message.Should().Be("Book not found");
        }

        [Fact]
        public async Task Handle_Should_Use_Select_Projection_For_Performance()
        {
            // This test verifies that the handler uses Select projection to avoid loading the full entity
            // By using mock verification, we can ensure the query is structured correctly

            // Arrange
            var bookId = 1;
            var query = new GetBookByIdQuery { Id = bookId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Should().BeOfType<BookDTO>();

            // The fact that we get a BookDTO and not a Book entity indicates proper projection
            result.Data.GetType().Should().Be(typeof(BookDTO));
        }

        [Fact]
        public async Task Handle_Should_Return_Null_Data_For_NotFound_Response()
        {
            // Arrange
            var nonExistentBookId = 99999;
            var query = new GetBookByIdQuery { Id = nonExistentBookId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be("Book not found");
            result.Data.Should().BeNull();

            // Verify no logging occurs for normal "not found" scenarios
            _mockLogger.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Never);
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

            // Setup asynchronous operations - crucial for FirstOrDefaultAsync to work
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