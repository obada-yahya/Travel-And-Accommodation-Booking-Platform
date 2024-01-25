using MediatR;

namespace Application.Queries.RoomAmenityQueries;

public record CheckRoomAmenityExistsQuery : IRequest<bool>
{
    public Guid Id { get; set; }
}