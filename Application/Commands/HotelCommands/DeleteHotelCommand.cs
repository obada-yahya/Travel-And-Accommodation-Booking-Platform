using MediatR;

namespace Application.Commands.HotelCommands;

public record DeleteHotelCommand : IRequest
{
    public Guid Id { get; set; }
}