namespace Application.DTOs.RoomAmenityDtos;

public record RoomAmenityDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}