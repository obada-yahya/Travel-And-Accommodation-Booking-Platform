using MediatR;

namespace Application.Queries.RoomQueries;

public record CheckRoomExistsQuery : IRequest<bool>
{
    public Guid Id { get; set; }
}