using Domain.Entities;

namespace Application.DTOs.HotelDtos;

public record HotelDto
{
    public Guid Id { get; set; }
    public Guid CityId { get; set; }
    public Guid OwnerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public float Rating { get; set; }
    public string StreetAddress { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public int FloorsNumber { get; set; }
    public IList<Room> Rooms { get; set; }
}