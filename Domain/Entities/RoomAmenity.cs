namespace Domain.Entities;

public class RoomAmenity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public IList<RoomType> RoomTypes { get; set; }
}