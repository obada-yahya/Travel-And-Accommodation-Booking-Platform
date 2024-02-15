using Application.DTOs.ImageDtos;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
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
        await UploadImageAsyncInternal(imageCreationDto);
    }

    public async Task UploadThumbnailAsync(ImageCreationDto imageCreationDto)
    {
        var thumbnail = await _context
            .Images
            .SingleOrDefaultAsync(e => e.Type == ImageType.Thumbnail && e.EntityId.Equals(imageCreationDto.EntityId));
        
        if (thumbnail is not null)
        {
            await DeleteThumbnailAsync(thumbnail.Id, thumbnail.Format.ToString().ToLower());
            await UploadImageAsyncInternal(imageCreationDto, thumbnail.Id);
        }
        else
        {
            await UploadImageAsync(imageCreationDto);
        }
    }

    private async Task UploadImageAsyncInternal(ImageCreationDto imageCreationDto, Guid? id = null)
    {
        var credentialsPath = GetCredentialsFilePath();
        var image = new Image
        {
            Id = id ?? Guid.NewGuid(),
            Format = imageCreationDto.Format,
            EntityId = imageCreationDto.EntityId,
            Type = imageCreationDto.Type
        };
        
        var formatToUse = image.Format;
        var formatString = formatToUse.ToString().ToLower();
        var imageBytes = Convert.FromBase64String(imageCreationDto.Base64Content);
        
        var credential = GoogleCredential.FromFile(credentialsPath);
        var storage = await StorageClient.CreateAsync(credential);
        var destinationObjectName = $"{image.Id}.{formatString}";
        var contentType = GetContentType(formatString);
        
        using var uploadStream = new MemoryStream(imageBytes);
        
        storage.UploadObject(BucketName, destinationObjectName, contentType, uploadStream);
        image.Url = await GetImagePublicUrl(destinationObjectName);
        
        if (id is not null)
        {
            var existingImage = await _context.Images.FindAsync(id);
            existingImage.Url = image.Url;
            existingImage.Format = image.Format;
        }
        else
        {
            await AddImageAsync(image);
        }
        
        await _context.SaveChangesAsync();
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

    public async Task DeleteImageAsync(Guid entityId, Guid imageId)
    {
        try
        {
            var image = await _context.Images
                .FirstOrDefaultAsync
                (image => image.Id == imageId && 
                image.EntityId == entityId);
            
            if (image == null)
                throw new NotFoundException($"Image with ID {imageId} not found");

            var destinationObjectName = $"{image.Id}.{image.Format.ToString().ToLower()}";
            var storage = new FirebaseStorage(BucketName);
            await storage.Child(destinationObjectName).DeleteAsync();

            _context.Images.Remove(image);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Image with ID {imageId} deleted successfully.");
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception)
        {
            throw new InvalidOperationException($"Error deleting image with ID {imageId}");
        }
    }
    
    private async Task DeleteThumbnailAsync(Guid imageId, string format)
    {
        try
        {
            var destinationObjectName = $"{imageId}.{format}";
            var storage = new FirebaseStorage(BucketName);
            await storage.Child(destinationObjectName).DeleteAsync();
            _logger.LogInformation($"Image with ID {imageId} deleted successfully.");
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception)
        {
            throw new InvalidOperationException($"Error deleting image with ID {imageId}");
        }
    }
}