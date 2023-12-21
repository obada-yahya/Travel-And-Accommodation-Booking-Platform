namespace Domain.Entities;

public class Booking
{
    public Booking(Guid id, Guid roomId, Guid guestId, DateTime checkInDate, DateTime checkOutDate, DateTime bookingDate, float bookingPrice)
    {
        Id = id;
        RoomId = roomId;
        GuestId = guestId;
        CheckInDate = checkInDate;
        CheckOutDate = checkOutDate;
        BookingDate = bookingDate;
        BookingPrice = bookingPrice;
    }

    public Guid Id { get; private set; }
    public Guid RoomId{ get; private set; }
    public Guid GuestId{ get; private set; }
    public DateTime CheckInDate{ get; private set; }
    public DateTime CheckOutDate{ get; private set; }
    public DateTime BookingDate { get; private set; }
    public float BookingPrice { get; private set; }
}