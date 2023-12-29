namespace Domain.Entities;

public class Review 
{
    public Guid Id { get; set; }
    public Guid BookingId { get; set; }
    public string Comment { get; set; }
    public DateTime ReviewDate { get; set; }
    public float Rating { get; set; }
}