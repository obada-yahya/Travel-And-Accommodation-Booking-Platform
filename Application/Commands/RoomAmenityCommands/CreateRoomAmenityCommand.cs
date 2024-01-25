using Application.DTOs.RoomAmenityDtos;
using MediatR;

namespace Application.Commands.RoomAmenityCommands;

public class CreateRoomAmenityCommand : IRequest<RoomAmenityDto?>
{
    public string Name { get; set; }
    public string Description { get; set; }
}