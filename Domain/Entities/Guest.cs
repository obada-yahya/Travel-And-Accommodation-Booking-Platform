namespace Domain.Entities;

public class Guest : Person
{
    public IList<Booking> Bookings { get; set; }
}