namespace Domain.Exceptions;

public class BookingCheckInDatePassedException : Exception
{
    public BookingCheckInDatePassedException() : base("Cannot delete a booking with check-in date today or in the past")
    {
    }
}