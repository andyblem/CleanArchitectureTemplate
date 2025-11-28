using AutoMapper;
using CleanArchitecture.Application.DTOs.Book;
using CleanArchitecture.Application.Features.CQRS.Books.Commands;
using CleanArchitecture.Application.Features.CQRS.Books.Queries;
using CleanArchitecture.Application.Features.Requests.BookRequests;
using CleanArchitecture.Application.Wrappers;
using CleanArchitecture.Domain.Entities;
using FluentAssertions;
using MediatR;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CleanArchitecture.UnitTests.Application.Features.Requests.BookRequests
{
    public class CreateBookRequestHandlerTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IMediator> _mockMediator;
        private readonly CreateBookRequestHandler _handler;

        public CreateBookRequestHandlerTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockMediator = new Mock<IMediator>();
            _handler = new CreateBookRequestHandler(_mockMapper.Object, _mockMediator.Object);
        }

        [Fact]
        public async Task Handle_Should_Create_Book_When_ISBN_Is_Unique()
        {
            // Arrange
            var createBookDto = new CreateBookDTO
            {
                Title = "Clean Architecture",
                ISBN = "978-0134494166",
                Summary = "Software architecture guide",
                Price = 29.99m
            };

            var request = new CreateBookRequest { Book = createBookDto };
            var book = new Book { Title = createBookDto.Title, ISBN = createBookDto.ISBN, Summary = createBookDto.Summary, Price = createBookDto.Price };

            _mockMediator.Setup(x => x.Send(It.IsAny<IsUniqueISBNQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(Response<bool>.Success(true, "ISBN is unique."));

            _mockMapper.Setup(x => x.Map<Book>(createBookDto)).Returns(book);

            _mockMediator.Setup(x => x.Send(It.IsAny<CreateBookCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(Response<int>.Success(1));

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().Be(1);

            // Verify ISBN uniqueness check was called
            _mockMediator.Verify(x => x.Send(It.Is<IsUniqueISBNQuery>(q => q.ISBN == createBookDto.ISBN), It.IsAny<CancellationToken>()), Times.Once);

            // Verify mapping was called
            _mockMapper.Verify(x => x.Map<Book>(createBookDto), Times.Once);

            // Verify create command was called
            _mockMediator.Verify(x => x.Send(It.IsAny<CreateBookCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_Failure_When_ISBN_Is_Not_Unique()
        {
            // Arrange
            var createBookDto = new CreateBookDTO
            {
                Title = "Test Book",
                ISBN = "978-0123456789",
                Summary = "Test summary",
                Price = 19.99m
            };

            var request = new CreateBookRequest { Book = createBookDto };

            _mockMediator.Setup(x => x.Send(It.IsAny<IsUniqueISBNQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(Response<bool>.Success(false, "ISBN already exists."));

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be("ISBN already exists.");

            // Verify ISBN check was called but mapping and create were not
            _mockMediator.Verify(x => x.Send(It.IsAny<IsUniqueISBNQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockMapper.Verify(x => x.Map<Book>(It.IsAny<CreateBookDTO>()), Times.Never);
            _mockMediator.Verify(x => x.Send(It.IsAny<CreateBookCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Return_Failure_When_ISBN_Check_Fails()
        {
            // Arrange
            var createBookDto = new CreateBookDTO
            {
                Title = "Test Book",
                ISBN = "978-0123456789"
            };

            var request = new CreateBookRequest { Book = createBookDto };

            _mockMediator.Setup(x => x.Send(It.IsAny<IsUniqueISBNQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(Response<bool>.Failure("Database error"));

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be("Database error");

            // Verify only ISBN check was called
            _mockMediator.Verify(x => x.Send(It.IsAny<IsUniqueISBNQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockMapper.Verify(x => x.Map<Book>(It.IsAny<CreateBookDTO>()), Times.Never);
            _mockMediator.Verify(x => x.Send(It.IsAny<CreateBookCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
