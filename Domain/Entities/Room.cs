namespace Domain.Entities;

public class Room
{
    public Room(Guid id, Guid hotelId, Guid roomTypeId, int adultsCapacity, int childrenCapacity, string view, float rating)
    {
        Id = id;
        HotelId = hotelId;
        RoomTypeId = roomTypeId;
        AdultsCapacity = adultsCapacity;
        ChildrenCapacity = childrenCapacity;
        View = view;
        Rating = rating;
    }

    public Guid Id { get; private set; }
    public Guid HotelId { get; private set; }
    public Guid RoomTypeId { get; private set; }
    public int AdultsCapacity { get; private set; }
    public int ChildrenCapacity { get; private set; }
    public string View { get; private set; }
    public float Rating { get; private set; }
}