using Domain.Enums;

namespace Application.DTOs.ImageDtos;

public record ImageCreationDto
{
    public Guid EntityId { get; set; }
    public string Base64Content { get; set; }
    public ImageFormat Format { get; set; }
    public ImageType Type { get; set; }
}