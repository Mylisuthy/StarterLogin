using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace StarterLogin.Application.Common.Interfaces;

public interface ICloudinaryService
{
    Task<string?> UploadImageAsync(IFormFile file);
    Task<string?> UploadVideoAsync(IFormFile file);
    Task<bool> DeleteAsync(string publicId);
}
