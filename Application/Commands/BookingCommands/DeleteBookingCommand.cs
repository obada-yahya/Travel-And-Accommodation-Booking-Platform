using MediatR;

namespace Application.Commands.BookingCommands;

public record DeleteBookingCommand : IRequest
{
    public Guid Id { get; set; }
}