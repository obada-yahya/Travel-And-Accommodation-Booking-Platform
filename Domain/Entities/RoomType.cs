namespace Domain.Entities;

public class RoomType : Entity
{
    public Guid HotelId { get; set; }
    public string Type { get; set; }
    public float PricePerNight { get; set; }
}