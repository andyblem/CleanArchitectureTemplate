using CleanArchitecture.Application.Wrappers;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace CleanArchitecture.UnitTests.Application.Wrappers
{
    public class ResponseTests
    {
        [Fact]
        public void Response_DefaultConstructor_Should_Initialize_With_Default_Values()
        {
            // Act
            var response = new Response<string>();

            // Assert
            response.Succeeded.Should().BeFalse(); // Updated: Now defaults to false
            response.Message.Should().BeNull();
            response.Data.Should().BeNull();
            response.Errors.Should().BeNull();
        }

        [Fact]
        public void Response_Success_Should_Set_Success_State_With_Data()
        {
            // Arrange
            var data = "test data";
            var message = "Success message";

            // Act
            var response = Response<string>.Success(data, message);

            // Assert
            response.Succeeded.Should().BeTrue();
            response.Message.Should().Be(message);
            response.Data.Should().Be(data);
            response.Errors.Should().BeNull();
        }

        [Fact]
        public void Response_Success_With_Data_And_Null_Message_Should_Set_Success_State()
        {
            // Arrange
            var data = "test data";

            // Act
            var response = Response<string>.Success(data);

            // Assert
            response.Succeeded.Should().BeTrue();
            response.Message.Should().BeNull();
            response.Data.Should().Be(data);
            response.Errors.Should().BeNull();
        }

        [Fact]
        public void Response_Failure_Should_Set_Failed_State()
        {
            // Arrange
            var errorMessage = "Error occurred";

            // Act
            var response = Response<string>.Failure(errorMessage);

            // Assert
            response.Succeeded.Should().BeFalse();
            response.Message.Should().Be(errorMessage);
            response.Data.Should().BeNull();
            response.Errors.Should().BeNull();
        }

        [Fact]
        public void Response_Failure_With_Errors_Should_Set_Error_Collection()
        {
            // Arrange
            var errorMessage = "Multiple errors occurred";
            var errors = new List<string> { "Error 1", "Error 2" };

            // Act
            var response = Response<string>.Failure(errorMessage, errors);

            // Assert
            response.Succeeded.Should().BeFalse();
            response.Message.Should().Be(errorMessage);
            response.Data.Should().BeNull();
            response.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public void Response_Properties_Should_Be_Settable()
        {
            // Arrange
            var response = new Response<int>();
            var errors = new List<string> { "Error 1", "Error 2" };

            // Act
            response.Succeeded = true;
            response.Message = "Custom message";
            response.Data = 42;
            response.Errors = errors;

            // Assert
            response.Succeeded.Should().BeTrue();
            response.Message.Should().Be("Custom message");
            response.Data.Should().Be(42);
            response.Errors.Should().BeEquivalentTo(errors);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("Valid message")]
        public void Response_Success_Should_Handle_Various_Message_Values(string message)
        {
            // Arrange
            var data = 123;

            // Act
            var response = Response<int>.Success(data, message);

            // Assert
            response.Succeeded.Should().BeTrue();
            response.Message.Should().Be(message);
            response.Data.Should().Be(data);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("Error message")]
        public void Response_Failure_Should_Handle_Various_Message_Values(string message)
        {
            // Act
            var response = Response<int>.Failure(message);

            // Assert
            response.Succeeded.Should().BeFalse();
            response.Message.Should().Be(message);
            response.Data.Should().Be(default(int)); // Default value for int
        }

        [Fact]
        public void Response_Success_Should_Work_With_Complex_Data_Types()
        {
            // Arrange
            var complexData = new
            {
                Id = 1,
                Name = "Test",
                Items = new List<string> { "Item1", "Item2" }
            };

            // Act
            var response = Response<object>.Success(complexData, "Success");

            // Assert
            response.Succeeded.Should().BeTrue();
            response.Data.Should().Be(complexData);
            response.Message.Should().Be("Success");
        }

        [Fact]
        public void Response_Success_Should_Work_With_Reference_Types()
        {
            // Arrange
            var listData = new List<string> { "Item1", "Item2", "Item3" };

            // Act
            var response = Response<List<string>>.Success(listData);

            // Assert
            response.Succeeded.Should().BeTrue();
            response.Data.Should().BeEquivalentTo(listData);
            response.Data.Should().BeSameAs(listData); // Reference equality
        }

        [Fact]
        public void Response_Success_Should_Work_With_Value_Types()
        {
            // Arrange
            var decimalValue = 99.99m;

            // Act
            var response = Response<decimal>.Success(decimalValue, "Price updated");

            // Assert
            response.Succeeded.Should().BeTrue();
            response.Data.Should().Be(decimalValue);
            response.Message.Should().Be("Price updated");
        }

        [Fact]
        public void Response_Errors_List_Should_Be_Modifiable_After_Creation()
        {
            // Arrange
            var errors = new List<string> { "Error 1", "Error 2" };
            var response = Response<string>.Failure("Multiple errors", errors);

            // Act
            response.Errors.Add("Error 3");

            // Assert
            response.Errors.Should().HaveCount(3);
            response.Errors.Should().Contain("Error 3");
        }

        [Fact]
        public void Response_Success_Should_Handle_Null_Data()
        {
            // Act
            var response = Response<string>.Success(null, "Success with null data");

            // Assert
            response.Succeeded.Should().BeTrue();
            response.Message.Should().Be("Success with null data");
            response.Data.Should().BeNull();
        }

        [Fact]
        public void Response_Multiple_Instantiations_Should_Be_Independent()
        {
            // Arrange & Act
            var response1 = Response<int>.Success(100, "First response");
            var response2 = Response<int>.Failure("Error in second response");

            // Assert
            response1.Succeeded.Should().BeTrue();
            response1.Data.Should().Be(100);
            response1.Message.Should().Be("First response");

            response2.Succeeded.Should().BeFalse();
            response2.Data.Should().Be(0);
            response2.Message.Should().Be("Error in second response");
        }

        [Fact]
        public void Response_Success_Should_Handle_Boolean_Data_Type()
        {
            // Act
            var successResponse = Response<bool>.Success(true, "Operation succeeded");
            var failDataResponse = Response<bool>.Success(false, "Operation returned false");

            // Assert
            successResponse.Succeeded.Should().BeTrue();
            successResponse.Data.Should().BeTrue();
            
            failDataResponse.Succeeded.Should().BeTrue(); // Success method sets Succeeded = true regardless of data
            failDataResponse.Data.Should().BeFalse();
        }

        [Fact]
        public void Response_Should_Allow_Setting_Properties_After_Construction()
        {
            // Arrange
            var response = Response<string>.Failure("Initial error");
            var newErrors = new List<string> { "Validation error 1", "Validation error 2" };

            // Act
            response.Succeeded = true;
            response.Message = "Updated to success";
            response.Data = "New data";
            response.Errors = newErrors;

            // Assert
            response.Succeeded.Should().BeTrue();
            response.Message.Should().Be("Updated to success");
            response.Data.Should().Be("New data");
            response.Errors.Should().BeEquivalentTo(newErrors);
        }

        [Fact]
        public void Response_Success_Should_Handle_Nullable_Value_Types()
        {
            // Arrange & Act
            var responseWithNull = Response<int?>.Success(null, "Null integer");
            var responseWithValue = Response<int?>.Success(42, "Valid integer");

            // Assert
            responseWithNull.Succeeded.Should().BeTrue();
            responseWithNull.Data.Should().BeNull();
            responseWithNull.Message.Should().Be("Null integer");

            responseWithValue.Succeeded.Should().BeTrue();
            responseWithValue.Data.Should().Be(42);
            responseWithValue.Message.Should().Be("Valid integer");
        }

        [Fact]
        public void Response_Should_Maintain_Error_Collection_Reference()
        {
            // Arrange
            var errors = new List<string> { "Error 1" };
            var response = Response<string>.Failure("Error occurred", errors);

            // Act
            errors.Add("Error 2");

            // Assert
            response.Errors.Should().HaveCount(2);
            response.Errors.Should().Contain("Error 2");
        }

        [Fact]
        public void Response_Success_Should_Handle_Generic_Collections()
        {
            // Arrange
            var dictionary = new Dictionary<string, int> { { "key1", 1 }, { "key2", 2 } };

            // Act
            var response = Response<Dictionary<string, int>>.Success(dictionary, "Dictionary data");

            // Assert
            response.Succeeded.Should().BeTrue();
            response.Data.Should().ContainKey("key1");
            response.Data.Should().ContainValue(2);
            response.Message.Should().Be("Dictionary data");
        }

        [Fact]
        public void Response_Static_Factory_Methods_Should_Return_Correct_Types()
        {
            // Act
            var successResponse = Response<string>.Success("data");
            var failureResponse = Response<string>.Failure("error");

            // Assert
            successResponse.Should().BeOfType<Response<string>>();
            failureResponse.Should().BeOfType<Response<string>>();
        }

        [Fact]
        public void Response_Failure_Should_Handle_Empty_Error_List()
        {
            // Arrange
            var emptyErrors = new List<string>();

            // Act
            var response = Response<string>.Failure("Error occurred", emptyErrors);

            // Assert
            response.Succeeded.Should().BeFalse();
            response.Message.Should().Be("Error occurred");
            response.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Response_Success_And_Failure_Should_Create_Different_States()
        {
            // Arrange
            var data = "test data";
            var errorMessage = "error occurred";

            // Act
            var successResponse = Response<string>.Success(data);
            var failureResponse = Response<string>.Failure(errorMessage);

            // Assert
            successResponse.Succeeded.Should().BeTrue();
            successResponse.Data.Should().Be(data);
            successResponse.Message.Should().BeNull();

            failureResponse.Succeeded.Should().BeFalse();
            failureResponse.Data.Should().BeNull();
            failureResponse.Message.Should().Be(errorMessage);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void Response_Success_Should_Handle_Various_Integer_Values(int value)
        {
            // Act
            var response = Response<int>.Success(value, $"Value: {value}");

            // Assert
            response.Succeeded.Should().BeTrue();
            response.Data.Should().Be(value);
            response.Message.Should().Be($"Value: {value}");
        }

        [Fact]
        public void Response_Failure_Without_Errors_Should_Set_Errors_To_Null()
        {
            // Act
            var response = Response<string>.Failure("Simple error");

            // Assert
            response.Succeeded.Should().BeFalse();
            response.Message.Should().Be("Simple error");
            response.Errors.Should().BeNull();
        }

        [Fact]
        public void Response_Factory_Methods_Should_Be_Fluent_And_Readable()
        {
            // Act & Assert - These should compile and be readable
            var userNotFound = Response<string>.Failure("User not found");
            var userCreated = Response<int>.Success(123, "User created successfully");
            var validationErrors = Response<object>.Failure("Validation failed", 
                new List<string> { "Name is required", "Email is invalid" });

            userNotFound.Succeeded.Should().BeFalse();
            userCreated.Succeeded.Should().BeTrue();
            validationErrors.Errors.Should().HaveCount(2);
        }
    }
}