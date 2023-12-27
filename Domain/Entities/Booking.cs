namespace Domain.Entities;

public class Booking : Entity
{
    public Guid RoomId{ get; set; }
    public Guid GuestId{ get; set; }
    public DateTime CheckInDate{ get;  set; }
    public DateTime CheckOutDate{ get; set; }
    public DateTime BookingDate { get; set; }
    public Review? Review { get; set; }
    public Payment? Payment { get;  set; }
}