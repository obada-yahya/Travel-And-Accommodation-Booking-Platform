using Application.DTOs.ImageDtos;

namespace Infrastructure.ImageStorage;

public interface IIMageService
{
    public Task UploadImageAsync(ImageCreationDto imageCreationDto);
    public Task<List<string>> GetAllImagesAsync(Guid entityId);
}