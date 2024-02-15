using Application.DTOs.RoomDtos;
using MediatR;

namespace Application.Queries.RoomQueries;

public record GetRoomByIdQuery : IRequest<RoomDto?>
{
    public Guid RoomId { get; set; }
}