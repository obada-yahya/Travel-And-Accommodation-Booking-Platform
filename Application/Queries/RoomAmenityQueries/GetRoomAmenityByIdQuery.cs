using Application.DTOs.RoomAmenityDtos;
using MediatR;

namespace Application.Queries.RoomAmenityQueries;

public record GetRoomAmenityByIdQuery : IRequest<RoomAmenityDto?>
{
    public Guid Id { get; set; }
}