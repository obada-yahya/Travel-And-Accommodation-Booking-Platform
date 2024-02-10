namespace Domain.Entities;

public class Discount
{
    public Guid Id { get; set; }
    public Guid RoomTypeId { get; set; }
    public float DiscountPercentage { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
}