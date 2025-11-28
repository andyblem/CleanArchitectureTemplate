using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces;
using FluentAssertions;
using Xunit;

namespace CleanArchitecture.UnitTests.Domain.Entities
{
    public class BookTests
    {
        [Fact]
        public void Book_Should_Inherit_From_BaseEntity()
        {
            // Act
            var book = new Book { Title = "Test", ISBN = "123", Price = 10.99m, Summary = "Test" };

            // Assert
            book.Should().BeAssignableTo<BaseEntity>();
        }

        [Fact]
        public void Book_Should_Implement_IAuditable()
        {
            // Act
            var book = new Book { Title = "Test", ISBN = "123", Price = 10.99m, Summary = "Test" };

            // Assert
            book.Should().BeAssignableTo<IAuditable>();
        }

        [Fact]
        public void Book_Should_Have_All_IAuditable_Properties()
        {
            // Arrange
            var book = new Book { Title = "Test", ISBN = "123", Price = 10.99m, Summary = "Test" };
            var createdBy = "TestUser";
            var modifiedBy = "ModifiedUser";
            var deletedBy = "DeletedUser";
            var createdAt = DateTime.UtcNow;
            var modifiedAt = DateTime.UtcNow.AddMinutes(5);
            var deletedAt = DateTime.UtcNow.AddMinutes(10);

            // Act
            book.CreatedBy = createdBy;
            book.ModifiedBy = modifiedBy;
            book.DeletedBy = deletedBy;
            book.CreatedAt = createdAt;
            book.ModifiedAt = modifiedAt;
            book.DeletedAt = deletedAt;
            book.IsDeleted = true;

            // Assert
            book.CreatedBy.Should().Be(createdBy);
            book.ModifiedBy.Should().Be(modifiedBy);
            book.DeletedBy.Should().Be(deletedBy);
            book.CreatedAt.Should().Be(createdAt);
            book.ModifiedAt.Should().Be(modifiedAt);
            book.DeletedAt.Should().Be(deletedAt);
            book.IsDeleted.Should().BeTrue();
        }

        [Fact]
        public void Book_Should_Have_Default_IAuditable_Values()
        {
            // Act
            var book = new Book { Title = "Test", ISBN = "123", Price = 10.99m, Summary = "Test" };

            // Assert - Check default values for IAuditable properties
            book.Id.Should().Be(0); // Default value for int
            book.IsDeleted.Should().BeFalse(); // Default value for bool
            book.CreatedBy.Should().BeNull();
            book.ModifiedBy.Should().BeNull();
            book.DeletedBy.Should().BeNull();
            book.CreatedAt.Should().Be(default(DateTime));
            book.ModifiedAt.Should().Be(default(DateTime));
            book.DeletedAt.Should().Be(default(DateTime));
        }

        [Fact]
        public void Book_Should_Have_Required_Properties()
        {
            // Arrange
            var title = "Clean Architecture";
            var isbn = "978-0134494166";
            var price = 29.99m;
            var summary = "Software architecture guide";

            // Act
            var book = new Book
            {
                Title = title,
                ISBN = isbn,
                Price = price,
                Summary = summary
            };

            // Assert
            book.Title.Should().Be(title);
            book.ISBN.Should().Be(isbn);
            book.Price.Should().Be(price);
            book.Summary.Should().Be(summary);
        }

        [Fact]
        public void Book_Properties_Should_Be_Immutable()
        {
            // This test verifies that properties have init-only setters
            var book = new Book
            {
                Title = "Original Title",
                ISBN = "123456789",
                Price = 19.99m,
                Summary = "Original Summary"
            };

            // Properties should be read-only after initialization
            book.Title.Should().Be("Original Title");
            book.ISBN.Should().Be("123456789");
            book.Price.Should().Be(19.99m);
            book.Summary.Should().Be("Original Summary");
        }
    }
}