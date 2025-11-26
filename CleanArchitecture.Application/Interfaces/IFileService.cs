using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Interfaces
{
    public interface IFileService
    {
        string GeneratePathToLocation(string relativePath);

        Task<byte[]> GetFileAsByteArrayAsync(string fileName, string pathToDirectory);
        Task SaveFileAsync(string fileName, string path, IFormFile file);
    }
}