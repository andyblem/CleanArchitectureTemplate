using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Interfaces
{
    public interface IFileService
    {
        string GeneratePathToLocation(string relativePath);

        Task DeleteFileAsync(string fileName, string path);
        Task SaveFileAsync(string fileName, string path, IFormFile file);

        Task<byte[]> GetFileAsByteArrayAsync(string fileName, string path);
    }
}