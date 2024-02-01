using Application.DTOs.BookingDtos;
using MediatR;

namespace Application.Commands.BookingCommands;

public record ReserveRoomCommand : IRequest<BookingDto?>
{
    public Guid RoomId { get; set; }
    public string GuestEmail { get; set; }
    public DateTime CheckInDate { get;  set; }
    public DateTime CheckOutDate { get; set; }
    public DateTime BookingDate { get; set; } = DateTime.Now;
}