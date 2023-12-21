namespace Domain.Entities;

public class Review
{
    public Review(Guid id, Guid bookingId, string comment, DateTime reviewDate, float rating)
    {
        Id = id;
        BookingId = bookingId;
        Comment = comment;
        ReviewDate = reviewDate;
        Rating = rating;
    }

    public Guid Id { get; private set; }
    public Guid BookingId { get; private set; }
    public string Comment { get; private set; }
    public DateTime ReviewDate { get; private set; }
    public float Rating { get; private set; }
}