namespace Application.DTOs.ReviewsDtos;

public record ReviewForCreationDto
{
    public Guid BookingId { get; set; }
    public string Comment { get; set; }
    public float Rating { get; set; } = -1;
}