using Application.DTOs.ImageDtos;

namespace Infrastructure.ImageStorage;

public interface IImageService
{
    public Task UploadImageAsync(ImageCreationDto imageCreationDto);
    public Task UploadThumbnailAsync(ImageCreationDto imageCreationDto);
    public Task<List<string>> GetAllImagesAsync(Guid entityId);
    public Task DeleteImageAsync(Guid entityId, Guid imageId);
}
