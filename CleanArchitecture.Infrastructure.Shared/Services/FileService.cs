using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Infrastructure.Shared.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Shared.Services
{
    public class FileService : IFileService
    {
        private static readonly string _rootFileFolder = "Files";
        private readonly FileSettingsDTO _settings;

        public FileService(IOptions<FileSettingsDTO> options)
        {
            _settings = options.Value;
        }

        public string GeneratePathToLocation(string location)
        {
            // generate path
            string pathToLocation = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                _rootFileFolder,
                location);

            // return result
            return pathToLocation;
        }

        public async Task DeleteFileAsync(string fileName, string pathToDirectory)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));
            }

            if (string.IsNullOrWhiteSpace(pathToDirectory))
            {
                throw new ArgumentException("Path to directory cannot be null or empty.", nameof(pathToDirectory));
            }

            try
            {
                // Create full path
                var filePath = Path.Combine(pathToDirectory, fileName);

                // Check if file exists and delete
                if (File.Exists(filePath))
                {
                    await Task.Run(() => File.Delete(filePath));
                }
                // If file doesn't exist, we consider it already "deleted" - no exception thrown
            }
            catch (UnauthorizedAccessException)
            {
                throw new InvalidOperationException($"Access denied when trying to delete file: {fileName}");
            }
            catch (DirectoryNotFoundException)
            {
                throw new InvalidOperationException($"Directory not found when trying to delete file: {fileName}");
            }
            catch (IOException ex)
            {
                throw new InvalidOperationException($"IO error occurred when trying to delete file: {fileName}. Error: {ex.Message}");
            }
            catch (Exception ex) when (!(ex is ArgumentException)) // Don't wrap ArgumentException
            {
                throw new InvalidOperationException($"Unexpected error occurred when trying to delete file: {fileName}. Error: {ex.Message}");
            }
        }

        public async Task SaveFileAsync(string fileName, string path, IFormFile file)
        {
            // make sure path exists
            bool isDirectoryAvailable = Directory.Exists(path);
            if (isDirectoryAvailable == false)
                Directory.CreateDirectory(path);

            // create full path
            var filePath = Path.Combine(path, fileName);

            // create file stream
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                // save image to storage
                await file.CopyToAsync(fileStream);
            }
        }

        public async Task<byte[]> GetFileAsByteArrayAsync(string fileName, string path)
        {
            try
            {
                // create full path
                var filePath = Path.Combine(path, fileName);

                // convert image to base 64 string
                byte[] imageBytes = await File.ReadAllBytesAsync(filePath);

                // return result
                return imageBytes;
            }
            catch
            {
                // return result
                return Array.Empty<byte>();
            }
        }
    }
}