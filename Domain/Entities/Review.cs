namespace Domain.Entities;

public class Review : Entity
{
    public Guid BookingId { get; set; }
    public string Comment { get; set; }
    public DateTime ReviewDate { get; set; }
    public float Rating { get; set; }
}