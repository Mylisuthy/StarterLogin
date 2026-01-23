<<<<<<< HEAD
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
=======
using System;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
>>>>>>> origin/test
using Microsoft.Extensions.Configuration;
using StarterLogin.Application.Common.Interfaces;

namespace StarterLogin.Infrastructure.Services;

<<<<<<< HEAD
public class CloudinaryService : IImageService
=======
public class CloudinaryService : ICloudinaryService
>>>>>>> origin/test
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService(IConfiguration configuration)
    {
<<<<<<< HEAD
        var account = new Account(
            configuration["Cloudinary:CloudName"],
            configuration["Cloudinary:ApiKey"],
            configuration["Cloudinary:ApiSecret"]
        );

        _cloudinary = new Cloudinary(account);
    }

    public async Task<string> UploadImageAsync(Stream stream, string fileName)
    {
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(fileName, stream),
            Transformation = new Transformation().Height(500).Width(350).Crop("fill") // Pokemon Card Ratio approx
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        return uploadResult.SecureUrl.ToString();
    }

    public async Task DeleteImageAsync(string imageUrl)
    {
        // Extraction of publicId would be needed here for full implementation
        // keeping it simple for now
        await Task.CompletedTask;
=======
        var cloudName = configuration["CloudinarySettings:CloudName"];
        var apiKey = configuration["CloudinarySettings:ApiKey"];
        var apiSecret = configuration["CloudinarySettings:ApiSecret"];

        var account = new Account(cloudName, apiKey, apiSecret);
        _cloudinary = new Cloudinary(account);
    }

    public async Task<string?> UploadImageAsync(IFormFile file)
    {
        if (file == null || file.Length == 0) return null;

        using var stream = file.OpenReadStream();
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Transformation = new Transformation().Quality("auto").FetchFormat("auto")
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        return uploadResult.SecureUrl?.ToString();
    }

    public async Task<string?> UploadVideoAsync(IFormFile file)
    {
        if (file == null || file.Length == 0) return null;

        using var stream = file.OpenReadStream();
        var uploadParams = new VideoUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Overwrite = true
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        return uploadResult.SecureUrl?.ToString();
    }

    public async Task<bool> DeleteAsync(string publicId)
    {
        var deletionParams = new DeletionParams(publicId);
        var result = await _cloudinary.DestroyAsync(deletionParams);
        return result.Result == "ok";
>>>>>>> origin/test
    }
}
