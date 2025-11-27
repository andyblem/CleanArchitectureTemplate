using AutoMapper;
using CleanArchitecture.Application.DTOs.Book;
using CleanArchitecture.Application.Mappings;
using CleanArchitecture.Domain.Entities;
using FluentAssertions;
using System;
using Xunit;

namespace CleanArchitecture.UnitTests.Application.Mappings
{
    public class BookProfilesTests
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configuration;

        public BookProfilesTests()
        {
            _configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<BookProfiles>();
            });

            _mapper = _configuration.CreateMapper();
        }

        [Fact]
        public void BookProfiles_Should_Have_Valid_Configuration()
        {
            // Assert
            _configuration.AssertConfigurationIsValid();
        }

        [Fact]
        public void Should_Map_CreateBookDTO_To_Book()
        {
            // Arrange
            var createBookDto = new CreateBookDTO
            {
                Title = "Clean Architecture",
                ISBN = "978-0134494166",
                Summary = "A comprehensive guide to clean architecture principles and practices.",
                Price = 39.99m
            };

            // Act
            var book = _mapper.Map<Book>(createBookDto);

            // Assert
            book.Should().NotBeNull();
            book.Title.Should().Be(createBookDto.Title);
            book.ISBN.Should().Be(createBookDto.ISBN);
            book.Summary.Should().Be(createBookDto.Summary);
            book.Price.Should().Be(createBookDto.Price);
        }

        [Fact]
        public void Should_Map_UpdateBookDTO_To_Book()
        {
            // Arrange
            var updateBookDto = new UpdateBookDTO
            {
                Id = 1,
                Title = "Clean Code",
                Summary = "A handbook of agile software craftsmanship.",
                Price = 34.99m
            };

            // Act
            var book = _mapper.Map<Book>(updateBookDto);

            // Assert
            book.Should().NotBeNull();
            book.Id.Should().Be(updateBookDto.Id);
            book.Title.Should().Be(updateBookDto.Title);
            book.Summary.Should().Be(updateBookDto.Summary);
            book.Price.Should().Be(updateBookDto.Price);
        }

        [Fact]
        public void Should_Map_CreateBookDTO_To_Book_With_Null_Values()
        {
            // Arrange
            var createBookDto = new CreateBookDTO
            {
                Title = null,
                ISBN = null,
                Summary = null,
                Price = 0m
            };

            // Act
            var book = _mapper.Map<Book>(createBookDto);

            // Assert
            book.Should().NotBeNull();
            book.Title.Should().BeNull();
            book.ISBN.Should().BeNull();
            book.Summary.Should().BeNull();
            book.Price.Should().Be(0m);
        }

        [Fact]
        public void Should_Map_UpdateBookDTO_To_Book_With_Null_Values()
        {
            // Arrange
            var updateBookDto = new UpdateBookDTO
            {
                Id = 0,
                Title = null,
                Summary = null,
                Price = 0m
            };

            // Act
            var book = _mapper.Map<Book>(updateBookDto);

            // Assert
            book.Should().NotBeNull();
            book.Id.Should().Be(0);
            book.Title.Should().BeNull();
            book.Summary.Should().BeNull();
            book.Price.Should().Be(0m);
        }

        [Fact]
        public void Should_Map_CreateBookDTO_To_Book_With_Empty_Strings()
        {
            // Arrange
            var createBookDto = new CreateBookDTO
            {
                Title = "",
                ISBN = "",
                Summary = "",
                Price = 19.99m
            };

            // Act
            var book = _mapper.Map<Book>(createBookDto);

            // Assert
            book.Should().NotBeNull();
            book.Title.Should().Be("");
            book.ISBN.Should().Be("");
            book.Summary.Should().Be("");
            book.Price.Should().Be(19.99m);
        }

        [Fact]
        public void Should_Map_UpdateBookDTO_To_Book_With_Empty_Strings()
        {
            // Arrange
            var updateBookDto = new UpdateBookDTO
            {
                Id = 5,
                Title = "",
                Summary = "",
                Price = 25.50m
            };

            // Act
            var book = _mapper.Map<Book>(updateBookDto);

            // Assert
            book.Should().NotBeNull();
            book.Id.Should().Be(5);
            book.Title.Should().Be("");
            book.Summary.Should().Be("");
            book.Price.Should().Be(25.50m);
        }

        [Fact]
        public void Should_Map_CreateBookDTO_To_Book_With_Long_Strings()
        {
            // Arrange
            var longTitle = new string('A', 500);
            var longISBN = new string('1', 50);
            var longSummary = new string('S', 2000);

            var createBookDto = new CreateBookDTO
            {
                Title = longTitle,
                ISBN = longISBN,
                Summary = longSummary,
                Price = 99.99m
            };

            // Act
            var book = _mapper.Map<Book>(createBookDto);

            // Assert
            book.Should().NotBeNull();
            book.Title.Should().Be(longTitle);
            book.ISBN.Should().Be(longISBN);
            book.Summary.Should().Be(longSummary);
            book.Price.Should().Be(99.99m);
        }

        [Fact]
        public void Should_Map_CreateBookDTO_To_Book_With_Special_Characters()
        {
            // Arrange
            var createBookDto = new CreateBookDTO
            {
                Title = "C# & .NET: Special Characters!@#$%^&*()",
                ISBN = "ISBN-123-456-789-X",
                Summary = "A book about special chars: <>{}[]|\\/:;\"'",
                Price = 15.75m
            };

            // Act
            var book = _mapper.Map<Book>(createBookDto);

            // Assert
            book.Should().NotBeNull();
            book.Title.Should().Be(createBookDto.Title);
            book.ISBN.Should().Be(createBookDto.ISBN);
            book.Summary.Should().Be(createBookDto.Summary);
            book.Price.Should().Be(createBookDto.Price);
        }

        [Fact]
        public void Should_Map_UpdateBookDTO_To_Book_With_Maximum_Decimal_Value()
        {
            // Arrange
            var updateBookDto = new UpdateBookDTO
            {
                Id = int.MaxValue,
                Title = "Expensive Book",
                Summary = "A very expensive book",
                Price = decimal.MaxValue
            };

            // Act
            var book = _mapper.Map<Book>(updateBookDto);

            // Assert
            book.Should().NotBeNull();
            book.Id.Should().Be(int.MaxValue);
            book.Title.Should().Be(updateBookDto.Title);
            book.Summary.Should().Be(updateBookDto.Summary);
            book.Price.Should().Be(decimal.MaxValue);
        }

        [Fact]
        public void Should_Map_CreateBookDTO_To_Book_With_Minimum_Decimal_Value()
        {
            // Arrange
            var createBookDto = new CreateBookDTO
            {
                Title = "Free Book",
                ISBN = "000-000-000-0",
                Summary = "A free book",
                Price = decimal.MinValue
            };

            // Act
            var book = _mapper.Map<Book>(createBookDto);

            // Assert
            book.Should().NotBeNull();
            book.Title.Should().Be(createBookDto.Title);
            book.ISBN.Should().Be(createBookDto.ISBN);
            book.Summary.Should().Be(createBookDto.Summary);
            book.Price.Should().Be(decimal.MinValue);
        }

        [Fact]
        public void Should_Not_Map_BaseEntity_Properties_From_CreateBookDTO()
        {
            // Arrange
            var createBookDto = new CreateBookDTO
            {
                Title = "Test Book",
                ISBN = "123456789",
                Summary = "Test Summary",
                Price = 10.00m
            };

            // Act
            var book = _mapper.Map<Book>(createBookDto);

            // Assert
            book.Should().NotBeNull();
            // BaseEntity properties should have default values since they're not mapped
            book.Id.Should().Be(0); // Default value for int
            book.CreatedAt.Should().Be(default(DateTime));
            book.ModifiedAt.Should().Be(default(DateTime));
            book.DeletedAt.Should().Be(default(DateTime));
            book.CreatedBy.Should().BeNull();
            book.ModifiedBy.Should().BeNull();
            book.DeletedBy.Should().BeNull();
            book.IsDeleted.Should().BeFalse();
        }

        [Fact]
        public void Should_Map_Id_From_UpdateBookDTO_But_Not_Other_BaseEntity_Properties()
        {
            // Arrange
            var updateBookDto = new UpdateBookDTO
            {
                Id = 42,
                Title = "Updated Book",
                Summary = "Updated Summary",
                Price = 29.99m
            };

            // Act
            var book = _mapper.Map<Book>(updateBookDto);

            // Assert
            book.Should().NotBeNull();
            book.Id.Should().Be(42); // This should be mapped
            // Other BaseEntity properties should have default values
            book.CreatedAt.Should().Be(default(DateTime));
            book.ModifiedAt.Should().Be(default(DateTime));
            book.DeletedAt.Should().Be(default(DateTime));
            book.CreatedBy.Should().BeNull();
            book.ModifiedBy.Should().BeNull();
            book.DeletedBy.Should().BeNull();
            book.IsDeleted.Should().BeFalse();
        }

        [Theory]
        [InlineData(0.01)]
        [InlineData(9.99)]
        [InlineData(100.00)]
        [InlineData(999.99)]
        [InlineData(1000.50)]
        public void Should_Map_CreateBookDTO_To_Book_With_Various_Price_Values(decimal price)
        {
            // Arrange
            var createBookDto = new CreateBookDTO
            {
                Title = "Price Test Book",
                ISBN = "123-456-789",
                Summary = "Testing various price values",
                Price = price
            };

            // Act
            var book = _mapper.Map<Book>(createBookDto);

            // Assert
            book.Should().NotBeNull();
            book.Price.Should().Be(price);
            book.Title.Should().Be(createBookDto.Title);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(int.MaxValue)]
        public void Should_Map_UpdateBookDTO_To_Book_With_Various_Id_Values(int id)
        {
            // Arrange
            var updateBookDto = new UpdateBookDTO
            {
                Id = id,
                Title = "ID Test Book",
                Summary = "Testing various ID values",
                Price = 15.99m
            };

            // Act
            var book = _mapper.Map<Book>(updateBookDto);

            // Assert
            book.Should().NotBeNull();
            book.Id.Should().Be(id);
            book.Title.Should().Be(updateBookDto.Title);
        }
    }
}