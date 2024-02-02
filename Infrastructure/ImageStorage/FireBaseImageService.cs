using Application.DTOs.ImageDtos;
using Domain.Entities;
using Firebase.Storage;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.ImageStorage;

public class FireBaseImageService : IImageService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<FireBaseImageService> _logger;
    private readonly string BucketName;

    public FireBaseImageService(ApplicationDbContext context, IConfiguration configuration, ILogger<FireBaseImageService> logger)
    {
        _context = context;
        _configuration = configuration;
        _logger = logger;
        BucketName = _configuration["FireBaseBucketName"]!;
    }

    private async Task<string> GetImagePublicUrl(string destinationObjectName)
    {
        var storage = new FirebaseStorage(BucketName);
        var starsRef = storage.Child(destinationObjectName);
        return await starsRef.GetDownloadUrlAsync();
    }
    
    private async Task AddImageAsync(Image image)
    {
        await _context.Images.AddAsync(image);
        await _context.SaveChangesAsync();
    }

    private string GetContentType(string format)
    {
        switch (format.ToLower())
        {
            case "jpg":
            case "jpeg":
                return "image/jpeg";
            case "png":
                return "image/png";
            default:
                return "application/octet-stream";
        }
    }
    
    private string GetCredentialsFilePath()
    {
        var parentPath = Directory.GetCurrentDirectory();
        if (parentPath == null) throw new Exception("Invalid File Path.");
        return Path.Combine(parentPath, "credentials.json");
    }
    
    public async Task UploadImageAsync(ImageCreationDto imageCreationDto)
    {
        var credentialsPath = GetCredentialsFilePath();
        var image = new Image
        {
            Id = Guid.NewGuid(),
            Format = imageCreationDto.Format,
            EntityId = imageCreationDto.EntityId
        };
        var format = image.Format.ToString().ToLower();
        var imageBytes = Convert.FromBase64String(imageCreationDto.Base64Content);
        
        var credential = GoogleCredential.FromFile(credentialsPath);
        var storage = await StorageClient.CreateAsync(credential);
        var destinationObjectName = $"{image.Id}.{format}";
        var contentType = GetContentType(format);
        
        using var uploadStream = new MemoryStream(imageBytes);
        
        storage.UploadObject(BucketName, destinationObjectName, contentType, uploadStream);
        image.Url = await GetImagePublicUrl(destinationObjectName);
        await AddImageAsync(image);
        _logger.LogInformation("Image uploaded successfully.");
    }

    public async Task<List<string>> GetAllImagesAsync(Guid entityId)
    {
        try
        {
            return await _context
                .Images
                .Where(image => image.EntityId.Equals(entityId))
                .Select(image => image.Url)
                .ToListAsync();
        }
        catch (Exception)
        {
            return new List<string>();
        }
    }
}