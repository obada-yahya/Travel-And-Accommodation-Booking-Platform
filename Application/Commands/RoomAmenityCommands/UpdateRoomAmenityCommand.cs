using MediatR;

namespace Application.Commands.RoomAmenityCommands;

public record UpdateRoomAmenityCommand : IRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}