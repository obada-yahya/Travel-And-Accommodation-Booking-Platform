using MediatR;

namespace Application.Commands.RoomAmenityCommands;

public record DeleteRoomAmenityCommand : IRequest
{
    public Guid Id { get; set; }
}