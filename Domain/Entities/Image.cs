using Domain.Enums;

namespace Domain.Entities;

public class Image
{
    public Guid Id { get; set; }
    public Guid EntityId { get; set; }
    public string Url { get; set; }
    public ImageFormat Format { get; set; }
}