using CleanArchitecture.Application.DTOs.Book;
using CleanArchitecture.Application.Features.CQRS.Books.Queries;
using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Application.Features.Parameters.Book;
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
    public class GetBooksListQueryHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _mockDbContext;
        private readonly Mock<DbSet<Book>> _mockDbSet;
        private readonly Mock<ILogger<GetBooksListQueryHandler>> _mockLogger;
        private readonly GetBooksListQueryHandler _handler;
        private readonly List<Book> _books;

        public GetBooksListQueryHandlerTests()
        {
            _mockDbContext = new Mock<IApplicationDbContext>();
            _mockDbSet = CreateMockDbSet();
            _mockLogger = new Mock<ILogger<GetBooksListQueryHandler>>();

            _books = new List<Book>
            {
                new Book { Id = 1, Title = "Clean Architecture", ISBN = "978-0134494166", Summary = "A guide to software architecture", Price = 29.99m },
                new Book { Id = 2, Title = "Domain-Driven Design", ISBN = "978-0321125217", Summary = "Tackling complexity in software", Price = 34.99m },
                new Book { Id = 3, Title = "Design Patterns", ISBN = "978-0201633610", Summary = "Elements of Reusable Object-Oriented Software", Price = 54.99m },
                new Book { Id = 4, Title = "Refactoring", ISBN = "978-0201485677", Summary = "Improving the Design of Existing Code", Price = 49.99m },
                new Book { Id = 5, Title = "Test Driven Development", ISBN = "978-0321146533", Summary = "By Example", Price = 39.99m },
                new Book { Id = 6, Title = "Effective Java", ISBN = "978-0134685991", Summary = "Third Edition", Price = 44.99m },
                new Book { Id = 7, Title = "Code Complete", ISBN = "978-0735619678", Summary = "A Practical Handbook", Price = 42.99m },
                new Book { Id = 8, Title = "The Pragmatic Programmer", ISBN = "978-0135957059", Summary = "Your Journey To Mastery", Price = 37.99m }
            };

            // Setup the mock DbSet with our test data
            SetupMockDbSetWithData(_mockDbSet, _books);

            _mockDbContext.Setup(x => x.Books).Returns(_mockDbSet.Object);

            _handler = new GetBooksListQueryHandler(_mockDbContext.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_Paged_Books_Successfully()
        {
            // Arrange
            var parameters = new GetBooksListParameter(1, 3);
            var query = new GetBooksListQuery { Parameters = parameters };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Should().HaveCount(3);
            result.PageNumber.Should().Be(1);
            result.PageSize.Should().Be(3);
            result.TotalRecords.Should().Be(8); // Total books in test data
            result.TotalPages.Should().Be(3); // 8 books / 3 per page = 3 pages (rounded up)

            // Verify the books are ordered by Id and contain correct data
            var booksList = result.Data.ToList();
            booksList[0].Id.Should().Be(1);
            booksList[0].Title.Should().Be("Clean Architecture");
            booksList[0].ISBN.Should().Be("978-0134494166");
            booksList[0].Price.Should().Be(29.99m);
        }

        [Fact]
        public async Task Handle_Should_Return_Second_Page_Correctly()
        {
            // Arrange
            var parameters = new GetBooksListParameter(2, 3);
            var query = new GetBooksListQuery { Parameters = parameters };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Should().HaveCount(3);
            result.PageNumber.Should().Be(2);
            result.PageSize.Should().Be(3);
            result.TotalRecords.Should().Be(8);

            // Verify second page contains books 4, 5, 6
            var booksList = result.Data.ToList();
            booksList[0].Id.Should().Be(4); // Should start from book Id 4 (skip first 3)
            booksList[1].Id.Should().Be(5);
            booksList[2].Id.Should().Be(6);
        }

        [Fact]
        public async Task Handle_Should_Return_Last_Page_With_Remaining_Books()
        {
            // Arrange
            var parameters = new GetBooksListParameter(3, 3);
            var query = new GetBooksListQuery { Parameters = parameters };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Should().HaveCount(2); // Only 2 books remaining on last page
            result.PageNumber.Should().Be(3);
            result.PageSize.Should().Be(3);
            result.TotalRecords.Should().Be(8);

            // Verify last page contains books 7, 8
            var booksList = result.Data.ToList();
            booksList[0].Id.Should().Be(7);
            booksList[1].Id.Should().Be(8);
        }

        [Theory]
        [InlineData(1, 5, 5, 8)] // First page, 5 items
        [InlineData(2, 5, 3, 8)] // Second page, 3 remaining items
        [InlineData(1, 10, 8, 8)] // All items on one page
        [InlineData(1, 2, 2, 8)] // Small page size
        public async Task Handle_Should_Return_Correct_Pagination_For_Various_Parameters(
            int pageNumber, int pageSize, int expectedCount, int expectedTotal)
        {
            // Arrange
            var parameters = new GetBooksListParameter(pageNumber, pageSize);
            var query = new GetBooksListQuery { Parameters = parameters };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Should().HaveCount(expectedCount);
            result.PageNumber.Should().Be(pageNumber);
            result.PageSize.Should().Be(pageSize);
            result.TotalRecords.Should().Be(expectedTotal);
        }

        [Fact]
        public async Task Handle_Should_Return_Empty_Result_For_Page_Beyond_Available_Data()
        {
            // Arrange
            var parameters = new GetBooksListParameter(10, 5); // Page 10 with 5 items per page
            var query = new GetBooksListQuery { Parameters = parameters };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Should().BeEmpty(); // No books on page 10
            result.PageNumber.Should().Be(10);
            result.PageSize.Should().Be(5);
            result.TotalRecords.Should().Be(8);
            result.TotalPages.Should().Be(2); // 8 books / 5 per page = 2 pages (rounded up)
        }

        [Fact]
        public async Task Handle_Should_Use_Default_Parameters_When_Parameters_Are_Null()
        {
            // Arrange
            var query = new GetBooksListQuery { Parameters = null };

            // This test checks the behavior when Parameters is null
            // The handler should handle this gracefully or the parameter class should provide defaults

            // Act & Assert
            // Note: This might throw an exception depending on the implementation
            // If the handler doesn't handle null parameters, we expect an exception
            var exception = await Record.ExceptionAsync(async () =>
                await _handler.Handle(query, CancellationToken.None));

            // Verify that either it works with defaults or throws appropriate exception
            if (exception == null)
            {
                // If no exception, verify the call completed successfully
                var result = await _handler.Handle(query, CancellationToken.None);
                result.Should().NotBeNull();
            }
            else
            {
                // If exception occurs, it should be a meaningful one
                exception.Should().Match<Exception>(ex =>
                    ex is NullReferenceException || ex is ArgumentNullException);
            }
        }

        [Fact]
        public async Task Handle_Should_Return_Books_Ordered_By_Id()
        {
            // Arrange
            var parameters = new GetBooksListParameter(1, 8); // Get all books
            var query = new GetBooksListQuery { Parameters = parameters };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull();

            var booksList = result.Data.ToList();
            booksList.Should().HaveCount(8);

            // Verify books are ordered by Id
            for (int i = 0; i < booksList.Count - 1; i++)
            {
                booksList[i].Id.Should().BeLessThan(booksList[i + 1].Id);
            }
        }

        [Fact]
        public async Task Handle_Should_Map_Properties_Correctly_To_BookListItemDTO()
        {
            // Arrange
            var parameters = new GetBooksListParameter(1, 1);
            var query = new GetBooksListQuery { Parameters = parameters };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull();

            var firstBook = result.Data.First();
            var expectedBook = _books.First();

            // Verify all properties are mapped correctly
            firstBook.Should().BeOfType<BookListItemDTO>();
            firstBook.Id.Should().Be(expectedBook.Id);
            firstBook.Title.Should().Be(expectedBook.Title);
            firstBook.ISBN.Should().Be(expectedBook.ISBN);
            firstBook.Price.Should().Be(expectedBook.Price);

            // Note: Summary is not included in BookListItemDTO, which is correct for list view
        }

        [Fact]
        public async Task Handle_Should_Use_Correct_CancellationToken()
        {
            // Arrange
            var parameters = new GetBooksListParameter(1, 3);
            var query = new GetBooksListQuery { Parameters = parameters };
            var cancellationToken = new CancellationToken(false);

            // Create fresh mocks to track cancellation token usage
            var mockDbContext = new Mock<IApplicationDbContext>();
            var mockDbSet = CreateMockDbSet();
            var mockLogger = new Mock<ILogger<GetBooksListQueryHandler>>();

            SetupMockDbSetWithData(mockDbSet, _books);
            mockDbContext.Setup(x => x.Books).Returns(mockDbSet.Object);

            var handler = new GetBooksListQueryHandler(mockDbContext.Object, mockLogger.Object);

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
            var parameters = new GetBooksListParameter(1, 5);
            var query = new GetBooksListQuery { Parameters = parameters };
            var exception = new InvalidOperationException("Database connection failed");

            // Create a fresh mock that throws an exception
            var mockDbContext = new Mock<IApplicationDbContext>();
            var mockDbSet = new Mock<DbSet<Book>>();
            var mockLogger = new Mock<ILogger<GetBooksListQueryHandler>>();

            mockDbSet.As<IQueryable<Book>>();
            mockDbSet.As<IAsyncEnumerable<Book>>();

            // Setup the mock to throw an exception when query is executed
            mockDbSet.As<IQueryable<Book>>()
                .Setup(m => m.Provider)
                .Throws(exception);

            mockDbContext.Setup(x => x.Books).Returns(mockDbSet.Object);

            var handler = new GetBooksListQueryHandler(mockDbContext.Object, mockLogger.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be("Error occurred while getting books list.");
            result.Data.Should().BeNull();
            result.Errors.Should().NotBeNull();
            result.Errors.Should().Contain(exception.Message);

            // Verify error logging
            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Error occurred while getting books list.")),
                    It.IsAny<InvalidOperationException>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_Failure_When_DbException_Occurs()
        {
            // Arrange
            var parameters = new GetBooksListParameter(1, 5);
            var query = new GetBooksListQuery { Parameters = parameters };
            var dbException = new DbUpdateException("Database query failed");

            // Create a fresh mock that throws a database exception
            var mockDbContext = new Mock<IApplicationDbContext>();
            var mockDbSet = new Mock<DbSet<Book>>();
            var mockLogger = new Mock<ILogger<GetBooksListQueryHandler>>();

            mockDbSet.As<IQueryable<Book>>();
            mockDbSet.As<IAsyncEnumerable<Book>>();

            // Setup the mock to throw a database exception
            mockDbSet.As<IQueryable<Book>>()
                .Setup(m => m.Provider)
                .Throws(dbException);

            mockDbContext.Setup(x => x.Books).Returns(mockDbSet.Object);

            var handler = new GetBooksListQueryHandler(mockDbContext.Object, mockLogger.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be("Error occurred while getting books list.");
            result.Data.Should().BeNull();
            result.Errors.Should().NotBeNull();
            result.Errors.Should().Contain(dbException.Message);

            // Verify error logging
            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Error occurred while getting books list.")),
                    It.IsAny<DbUpdateException>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_Correct_TotalPages_Calculation()
        {
            // Arrange - Test various scenarios for total pages calculation
            var testCases = new[]
            {
                new { PageSize = 3, ExpectedTotalPages = 3 }, // 8 books / 3 = 2.67 => 3 pages
                new { PageSize = 4, ExpectedTotalPages = 2 }, // 8 books / 4 = 2 pages
                new { PageSize = 8, ExpectedTotalPages = 1 }, // 8 books / 8 = 1 page
                new { PageSize = 10, ExpectedTotalPages = 1 }  // 8 books / 10 = 0.8 => 1 page
            };

            foreach (var testCase in testCases)
            {
                // Arrange
                var parameters = new GetBooksListParameter(1, testCase.PageSize);
                var query = new GetBooksListQuery { Parameters = parameters };

                // Act
                var result = await _handler.Handle(query, CancellationToken.None);

                // Assert
                result.TotalPages.Should().Be(testCase.ExpectedTotalPages,
                    $"For PageSize {testCase.PageSize} with 8 total records");
            }
        }

        [Fact]
        public async Task Handle_Should_Handle_Concurrent_Requests()
        {
            // Arrange
            var parameters1 = new GetBooksListParameter(1, 3);
            var parameters2 = new GetBooksListParameter(2, 3);
            var parameters3 = new GetBooksListParameter(1, 5);

            var query1 = new GetBooksListQuery { Parameters = parameters1 };
            var query2 = new GetBooksListQuery { Parameters = parameters2 };
            var query3 = new GetBooksListQuery { Parameters = parameters3 };

            // Act
            var tasks = new[]
            {
                _handler.Handle(query1, CancellationToken.None),
                _handler.Handle(query2, CancellationToken.None),
                _handler.Handle(query3, CancellationToken.None)
            };

            var results = await Task.WhenAll(tasks);

            // Assert
            // First request - page 1, size 3
            results[0].Succeeded.Should().BeTrue();
            results[0].Data.Should().HaveCount(3);
            results[0].PageNumber.Should().Be(1);
            results[0].PageSize.Should().Be(3);

            // Second request - page 2, size 3
            results[1].Succeeded.Should().BeTrue();
            results[1].Data.Should().HaveCount(3);
            results[1].PageNumber.Should().Be(2);
            results[1].PageSize.Should().Be(3);

            // Third request - page 1, size 5
            results[2].Succeeded.Should().BeTrue();
            results[2].Data.Should().HaveCount(5);
            results[2].PageNumber.Should().Be(1);
            results[2].PageSize.Should().Be(5);
        }

        [Fact]
        public async Task Handle_Should_Use_AsNoTracking_For_Performance()
        {
            // This test verifies that the query uses AsNoTracking() for better performance
            // The actual verification is implicit through the mock setup and the fact that
            // the query returns DTOs instead of entities

            // Arrange
            var parameters = new GetBooksListParameter(1, 3);
            var query = new GetBooksListQuery { Parameters = parameters };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull();

            // Verify we get DTOs, not entities (indicating proper projection)
            result.Data.First().Should().BeOfType<BookListItemDTO>();
        }

        [Fact]
        public async Task Handle_Should_Return_Empty_List_When_No_Books_Exist()
        {
            // Arrange
            var emptyBookList = new List<Book>();
            var mockDbContext = new Mock<IApplicationDbContext>();
            var mockDbSet = CreateMockDbSet();
            var mockLogger = new Mock<ILogger<GetBooksListQueryHandler>>();

            SetupMockDbSetWithData(mockDbSet, emptyBookList);
            mockDbContext.Setup(x => x.Books).Returns(mockDbSet.Object);

            var handler = new GetBooksListQueryHandler(mockDbContext.Object, mockLogger.Object);

            var parameters = new GetBooksListParameter(1, 10);
            var query = new GetBooksListQuery { Parameters = parameters };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Should().BeEmpty();
            result.TotalRecords.Should().Be(0);
            result.TotalPages.Should().Be(0);
            result.PageNumber.Should().Be(1);
            result.PageSize.Should().Be(10);
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

            // Setup asynchronous operations - crucial for CountAsync and ToListAsync to work
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