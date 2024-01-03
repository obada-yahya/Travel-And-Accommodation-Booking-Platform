using Domain.Enums;

namespace Domain.Entities;

public class RoomType
{
    public Guid Id { get; set; }
    public Guid HotelId { get; set; }
    public RoomCategory Type { get; set; }
    public float PricePerNight { get; set; }
}