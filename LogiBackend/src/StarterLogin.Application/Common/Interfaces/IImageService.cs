using System.IO;
using System.Threading.Tasks;

namespace StarterLogin.Application.Common.Interfaces;

public interface IImageService
{
    Task<string> UploadImageAsync(Stream stream, string fileName);
    Task DeleteImageAsync(string imageUrl);
}
