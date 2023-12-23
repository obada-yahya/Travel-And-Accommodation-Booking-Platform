namespace Domain.Entities;

public class Room
{
    public Guid Id { get; set; }
    public Guid HotelId { get; set; }
    public Guid RoomTypeId { get; set; }
    public int AdultsCapacity { get; set; }
    public int ChildrenCapacity { get; set; }
    public string View { get; set; }
    public float Rating { get; set; }
}