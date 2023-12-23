namespace Domain.Entities;

public class RoomType
{
    public Guid Id { get; set; }
    public Guid HotelId{ get; set; }
    public string Type{ get; set; }
    public float PricePerNight{ get; set; }
}