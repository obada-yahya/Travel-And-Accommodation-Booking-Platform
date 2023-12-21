namespace Domain.Entities;

public class RoomType
{
    public RoomType(Guid id, Guid hotelId, string type, float price)
    {
        Id = id;
        HotelId = hotelId;
        Type = type;
        Price = price;
    }

    public Guid Id { get; private set; }
    public Guid HotelId{ get; private set; }
    public string Type{ get; private set; }
    public float Price{ get; private set; }
}