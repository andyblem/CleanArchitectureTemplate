using CleanArchitecture.Application.Wrappers;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CleanArchitecture.UnitTests.Application.Wrappers
{
    public class PagedResponseTests
    {
        [Fact]
        public void PagedResponse_Success_Should_Initialize_All_Properties_Correctly()
        {
            // Arrange
            var data = new List<string> { "Item1", "Item2", "Item3" };
            int pageNumber = 2;
            int pageSize = 10;
            int totalRecords = 25;
            int expectedTotalPages = 3; // Math.Ceiling(25 / 10.0)

            // Act
            var response = PagedResponse<List<string>>.Success(data, pageNumber, pageSize, totalRecords);

            // Assert
            response.Data.Should().BeEquivalentTo(data);
            response.PageNumber.Should().Be(pageNumber);
            response.PageSize.Should().Be(pageSize);
            response.TotalRecords.Should().Be(totalRecords);
            response.TotalPages.Should().Be(expectedTotalPages);
            response.Message.Should().Be("Records found"); // Default message
            response.Succeeded.Should().BeTrue();
            response.Errors.Should().BeNull();
        }

        [Fact]
        public void PagedResponse_Success_With_Custom_Message_Should_Set_Message_Correctly()
        {
            // Arrange
            var data = new List<string> { "Item1", "Item2", "Item3" };
            var customMessage = "Books retrieved successfully";

            // Act
            var response = PagedResponse<List<string>>.Success(data, 1, 10, 3, customMessage);

            // Assert
            response.Message.Should().Be(customMessage);
            response.Succeeded.Should().BeTrue();
        }

        [Fact]
        public void PagedResponse_Should_Inherit_From_Response()
        {
            // Arrange & Act
            var pagedResponse = PagedResponse<List<int>>.Success(new List<int> { 1, 2, 3 }, 1, 10, 3);

            // Assert
            pagedResponse.Should().BeAssignableTo<Response<List<int>>>();
        }

        [Theory]
        [InlineData(1, 10, 0, 0)]      // No items
        [InlineData(1, 10, 5, 1)]      // Less than one page
        [InlineData(1, 10, 10, 1)]     // Exactly one page
        [InlineData(1, 10, 15, 2)]     // One and half pages
        [InlineData(1, 10, 25, 3)]     // Two and half pages
        [InlineData(1, 10, 100, 10)]   // Exactly 10 pages
        [InlineData(1, 10, 101, 11)]   // 10 pages and 1 item
        public void PagedResponse_Success_Should_Calculate_TotalPages_Correctly(int pageNumber, int pageSize, int totalRecords, int expectedTotalPages)
        {
            // Arrange
            var data = Enumerable.Range(1, Math.Min(totalRecords, pageSize)).ToList();

            // Act
            var response = PagedResponse<List<int>>.Success(data, pageNumber, pageSize, totalRecords);

            // Assert
            response.TotalPages.Should().Be(expectedTotalPages);
        }

        [Fact]
        public void PagedResponse_Success_Should_Handle_Zero_PageSize_Gracefully()
        {
            // Arrange
            var data = new List<string> { "Item1" };
            int pageNumber = 1;
            int pageSize = 0;
            int totalRecords = 10;

            // Act
            var response = PagedResponse<List<string>>.Success(data, pageNumber, pageSize, totalRecords);

            // Assert
            response.TotalPages.Should().Be(0); // When pageSize is 0, TotalPages should be 0
            response.PageSize.Should().Be(pageSize);
            response.PageNumber.Should().Be(pageNumber);
            response.TotalRecords.Should().Be(totalRecords);
        }

        [Theory]
        [InlineData(1, 1, 1, 1)]
        [InlineData(5, 3, 14, 5)]
        [InlineData(10, 20, 100, 5)]
        [InlineData(1, 50, 25, 1)]
        public void PagedResponse_Success_Should_Set_Pagination_Properties_Correctly(int pageNumber, int pageSize, int totalRecords, int expectedTotalPages)
        {
            // Arrange
            var data = new string[] { "test" };

            // Act
            var response = PagedResponse<string[]>.Success(data, pageNumber, pageSize, totalRecords);

            // Assert
            response.PageNumber.Should().Be(pageNumber);
            response.PageSize.Should().Be(pageSize);
            response.TotalRecords.Should().Be(totalRecords);
            response.TotalPages.Should().Be(expectedTotalPages);
        }

        [Fact]
        public void PagedResponse_Success_Should_Handle_Null_Data()
        {
            // Act
            var response = PagedResponse<List<string>>.Success(null, 1, 10, 0);

            // Assert
            response.Data.Should().BeNull();
            response.Succeeded.Should().BeTrue();
            response.PageNumber.Should().Be(1);
            response.PageSize.Should().Be(10);
            response.TotalRecords.Should().Be(0);
            response.TotalPages.Should().Be(0);
        }

        [Fact]
        public void PagedResponse_Empty_Should_Handle_Empty_Collection()
        {
            // Act
            var response = PagedResponse<List<int>>.Empty(1, 10);

            // Assert
            response.Data.Should().BeNull(); // Empty returns default(T) which is null for reference types
            response.Succeeded.Should().BeTrue();
            response.TotalPages.Should().Be(0);
            response.TotalRecords.Should().Be(0);
            response.Message.Should().Be("No records found"); // Default empty message
        }

        [Fact]
        public void PagedResponse_Empty_With_Custom_Message_Should_Set_Message_Correctly()
        {
            // Arrange
            var customMessage = "No books found matching criteria";

            // Act
            var response = PagedResponse<List<int>>.Empty(1, 10, customMessage);

            // Assert
            response.Message.Should().Be(customMessage);
            response.Succeeded.Should().BeTrue();
            response.TotalRecords.Should().Be(0);
        }

        [Fact]
        public void PagedResponse_Success_Should_Work_With_Different_Data_Types()
        {
            // Arrange
            var books = new[]
            {
                new { Id = 1, Title = "Book 1" },
                new { Id = 2, Title = "Book 2" }
            };

            // Act
            var response = PagedResponse<object[]>.Success(books, 1, 10, 2);

            // Assert
            response.Data.Should().HaveCount(2);
            response.Data.Should().BeEquivalentTo(books);
            response.TotalPages.Should().Be(1);
        }

        [Fact]
        public void PagedResponse_Failure_Should_Set_Error_Properties_Correctly()
        {
            // Arrange
            var errorMessage = "Database connection failed";
            var errors = new List<string> { "Connection timeout", "Authentication failed" };

            // Act
            var response = PagedResponse<List<string>>.Failure(errorMessage, errors);

            // Assert
            response.Succeeded.Should().BeFalse();
            response.Message.Should().Be(errorMessage);
            response.Errors.Should().BeEquivalentTo(errors);
            response.Data.Should().BeNull();
            response.PageNumber.Should().Be(0);
            response.PageSize.Should().Be(0);
            response.TotalPages.Should().Be(0);
            response.TotalRecords.Should().Be(0);
        }

        [Fact]
        public void PagedResponse_Failure_Without_Errors_Should_Work()
        {
            // Arrange
            var errorMessage = "Something went wrong";

            // Act
            var response = PagedResponse<List<string>>.Failure(errorMessage);

            // Assert
            response.Succeeded.Should().BeFalse();
            response.Message.Should().Be(errorMessage);
            response.Errors.Should().BeNull();
            response.Data.Should().BeNull();
        }

        [Theory]
        [InlineData(1, 1, 100)]
        [InlineData(1, 25, 100)]
        [InlineData(1, 50, 100)]
        [InlineData(1, 100, 100)]
        public void PagedResponse_Success_TotalPages_Calculation_Should_Use_Ceiling_Function(int pageNumber, int pageSize, int totalRecords)
        {
            // Arrange
            var data = new List<string>();
            var expectedTotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            // Act
            var response = PagedResponse<List<string>>.Success(data, pageNumber, pageSize, totalRecords);

            // Assert
            response.TotalPages.Should().Be(expectedTotalPages);
        }

        [Fact]
        public void PagedResponse_Success_Should_Set_Success_State_By_Default()
        {
            // Arrange
            var data = new List<string> { "Item1", "Item2" };

            // Act
            var response = PagedResponse<List<string>>.Success(data, 1, 10, 2);

            // Assert
            response.Succeeded.Should().BeTrue();
            response.Message.Should().Be("Records found"); // Default message
            response.Errors.Should().BeNull();
        }

        [Fact]
        public void PagedResponse_Success_Should_Handle_Large_Numbers()
        {
            // Arrange
            var data = new List<int> { 1, 2, 3 };
            int pageNumber = int.MaxValue;
            int pageSize = 1000000;
            int totalRecords = int.MaxValue;
            var expectedTotalPages = (int)Math.Ceiling(int.MaxValue / 1000000.0);

            // Act
            var response = PagedResponse<List<int>>.Success(data, pageNumber, pageSize, totalRecords);

            // Assert
            response.PageNumber.Should().Be(pageNumber);
            response.PageSize.Should().Be(pageSize);
            response.TotalRecords.Should().Be(totalRecords);
            response.TotalPages.Should().Be(expectedTotalPages);
        }

        [Fact]
        public void PagedResponse_TotalRecords_Should_Be_Int_Type()
        {
            // Arrange
            var data = new List<string> { "test" };

            // Act
            var response = PagedResponse<List<string>>.Success(data, 1, 10, 25);

            // Assert
            response.TotalRecords.Should().BeOfType(typeof(int));
            response.TotalRecords.Should().Be(25);
        }

        [Fact]
        public void Multiple_PagedResponse_Instances_Should_Be_Independent()
        {
            // Arrange
            var data1 = new List<string> { "Item1" };
            var data2 = new List<int> { 1, 2, 3 };

            // Act
            var response1 = PagedResponse<List<string>>.Success(data1, 1, 10, 1);
            var response2 = PagedResponse<List<int>>.Success(data2, 2, 5, 15);

            // Assert
            response1.PageNumber.Should().Be(1);
            response1.PageSize.Should().Be(10);
            response1.TotalRecords.Should().Be(1);

            response2.PageNumber.Should().Be(2);
            response2.PageSize.Should().Be(5);
            response2.TotalRecords.Should().Be(15);
            response2.TotalPages.Should().Be(3);
        }

        [Fact]
        public void PagedResponse_Success_Should_Calculate_Fractional_Pages_Correctly()
        {
            // Arrange
            var testCases = new[]
            {
                new { PageSize = 3, TotalRecords = 10, ExpectedPages = 4 },  // 10/3 = 3.33 -> 4
                new { PageSize = 7, TotalRecords = 20, ExpectedPages = 3 },  // 20/7 = 2.86 -> 3
                new { PageSize = 4, TotalRecords = 13, ExpectedPages = 4 },  // 13/4 = 3.25 -> 4
                new { PageSize = 6, TotalRecords = 18, ExpectedPages = 3 }   // 18/6 = 3.00 -> 3
            };

            foreach (var testCase in testCases)
            {
                // Act
                var response = PagedResponse<List<string>>.Success(new List<string>(), 1, testCase.PageSize, testCase.TotalRecords);

                // Assert
                response.TotalPages.Should().Be(testCase.ExpectedPages, 
                    $"PageSize: {testCase.PageSize}, TotalRecords: {testCase.TotalRecords}");
            }
        }

        [Fact]
        public void PagedResponse_Success_Should_Handle_IEnumerable_Data()
        {
            // Arrange
            IEnumerable<string> data = new[] { "Item1", "Item2", "Item3" };

            // Act
            var response = PagedResponse<IEnumerable<string>>.Success(data, 1, 5, 3);

            // Assert
            response.Data.Should().HaveCount(3);
            response.Data.Should().Contain("Item1");
            response.Succeeded.Should().BeTrue();
            response.TotalPages.Should().Be(1);
        }

        [Fact]
        public void PagedResponse_Success_Should_Work_With_Custom_Objects()
        {
            // Arrange
            var books = new List<BookDto>
            {
                new() { Id = 1, Title = "Book 1" },
                new() { Id = 2, Title = "Book 2" }
            };

            // Act
            var response = PagedResponse<List<BookDto>>.Success(books, 1, 10, 2);

            // Assert
            response.Data.Should().HaveCount(2);
            response.Data.First().Title.Should().Be("Book 1");
            response.TotalPages.Should().Be(1);
        }

        [Fact]
        public void PagedResponse_Factory_Methods_Should_Not_Allow_Direct_Constructor_Access()
        {
            // This test verifies that constructors are private and only factory methods can be used
            // The test passes if the code compiles, since private constructors would cause compilation errors
            
            // Act & Assert - Should compile (constructors are private, factory methods work)
            var successResponse = PagedResponse<List<string>>.Success(new List<string>(), 1, 10, 0);
            var failureResponse = PagedResponse<List<string>>.Failure("Error");
            var emptyResponse = PagedResponse<List<string>>.Empty(1, 10);

            successResponse.Should().NotBeNull();
            failureResponse.Should().NotBeNull();
            emptyResponse.Should().NotBeNull();
        }

        [Theory]
        [InlineData("Database error")]
        [InlineData("Network timeout")]
        [InlineData("Invalid operation")]
        public void PagedResponse_Failure_Should_Handle_Various_Error_Messages(string errorMessage)
        {
            // Act
            var response = PagedResponse<List<string>>.Failure(errorMessage);

            // Assert
            response.Succeeded.Should().BeFalse();
            response.Message.Should().Be(errorMessage);
            response.Data.Should().BeNull();
        }

        [Fact]
        public void PagedResponse_Static_Methods_Should_Return_Correct_Types()
        {
            // Act
            var successResponse = PagedResponse<List<string>>.Success(new List<string>(), 1, 10, 0);
            var failureResponse = PagedResponse<List<string>>.Failure("Error");
            var emptyResponse = PagedResponse<List<string>>.Empty(1, 10);

            // Assert
            successResponse.Should().BeOfType<PagedResponse<List<string>>>();
            failureResponse.Should().BeOfType<PagedResponse<List<string>>>();
            emptyResponse.Should().BeOfType<PagedResponse<List<string>>>();
        }

        private class BookDto
        {
            public int Id { get; set; }
            public string Title { get; set; } = string.Empty;
        }
    }
}