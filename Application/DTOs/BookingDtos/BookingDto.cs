namespace Application.DTOs.BookingDtos;

public record BookingDto
{
    public Guid Id { get; set; }
    public Guid RoomId { get; set; }
    public Guid UserId { get; set; }
    public DateTime CheckInDate { get;  set; }
    public DateTime CheckOutDate { get; set; }
    public DateTime BookingDate { get; set; }
}