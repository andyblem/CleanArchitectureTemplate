using CleanArchitecture.Infrastructure.Shared.DTOs;
using CleanArchitecture.Infrastructure.Shared.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CleanArchitecture.UnitTests.Infrastructure.Shared.Services
{
    public class FileServiceTests : IDisposable
    {
        private readonly FileService _fileService;
        private readonly string _testDirectory;
        private readonly Mock<IOptions<FileSettingsDTO>> _mockOptions;

        public FileServiceTests()
        {
            _mockOptions = new Mock<IOptions<FileSettingsDTO>>();
            _mockOptions.Setup(x => x.Value).Returns(new FileSettingsDTO());
            
            _fileService = new FileService(_mockOptions.Object);
            _testDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(_testDirectory);
        }

        [Fact]
        public void GeneratePathToLocation_Should_Return_Valid_Path()
        {
            // Arrange
            var location = "uploads/profile-pictures";

            // Act
            var result = _fileService.GeneratePathToLocation(location);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Contain("Files");
            result.Should().Contain("uploads");
            result.Should().Contain("profile-pictures");
        }

        [Fact]
        public async Task SaveFileAsync_Should_Create_File_Successfully()
        {
            // Arrange
            var fileName = "test.txt";
            var content = "test content";
            var formFile = CreateTestFormFile(fileName, content);

            // Act
            await _fileService.SaveFileAsync(fileName, _testDirectory, formFile);

            // Assert
            var filePath = Path.Combine(_testDirectory, fileName);
            File.Exists(filePath).Should().BeTrue();
            
            var savedContent = await File.ReadAllTextAsync(filePath);
            savedContent.Should().Be(content);
        }

        [Fact]
        public async Task GetFileAsByteArrayAsync_Should_Return_File_Bytes()
        {
            // Arrange
            var fileName = "test.txt";
            var content = "test content";
            var expectedBytes = Encoding.UTF8.GetBytes(content);
            
            var filePath = Path.Combine(_testDirectory, fileName);
            await File.WriteAllBytesAsync(filePath, expectedBytes);

            // Act
            var result = await _fileService.GetFileAsByteArrayAsync(fileName, _testDirectory);

            // Assert
            result.Should().Equal(expectedBytes);
        }

        [Fact]
        public async Task GetFileAsByteArrayAsync_Should_Return_Empty_When_File_Not_Found()
        {
            // Arrange
            var fileName = "nonexistent.txt";

            // Act
            var result = await _fileService.GetFileAsByteArrayAsync(fileName, _testDirectory);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task DeleteFileAsync_Should_Remove_File_Successfully()
        {
            // Arrange
            var fileName = "test.txt";
            var filePath = Path.Combine(_testDirectory, fileName);
            await File.WriteAllTextAsync(filePath, "test content");

            File.Exists(filePath).Should().BeTrue(); // Verify file exists first

            // Act
            await _fileService.DeleteFileAsync(fileName, _testDirectory);

            // Assert
            File.Exists(filePath).Should().BeFalse();
        }

        [Fact]
        public async Task DeleteFileAsync_Should_Not_Throw_When_File_Not_Exists()
        {
            // Arrange
            var fileName = "nonexistent.txt";

            // Act & Assert
            var action = async () => await _fileService.DeleteFileAsync(fileName, _testDirectory);
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task DeleteFileAsync_Should_Throw_When_Invalid_Parameters()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _fileService.DeleteFileAsync("", _testDirectory));
            await Assert.ThrowsAsync<ArgumentException>(() => _fileService.DeleteFileAsync("test.txt", ""));
            await Assert.ThrowsAsync<ArgumentException>(() => _fileService.DeleteFileAsync(null, _testDirectory));
        }

        private IFormFile CreateTestFormFile(string fileName, string content)
        {
            var bytes = Encoding.UTF8.GetBytes(content);
            var stream = new MemoryStream(bytes);
            var formFile = new FormFile(stream, 0, stream.Length, "file", fileName);
            return formFile;
        }

        public void Dispose()
        {
            if (Directory.Exists(_testDirectory))
            {
                Directory.Delete(_testDirectory, true);
            }
        }
    }
}