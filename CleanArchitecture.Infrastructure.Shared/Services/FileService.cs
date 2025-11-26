
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

    }
}