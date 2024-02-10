using MediatR;

namespace Application.Queries.RoomCategoryQueries;

public record CheckRoomTypeExistsQuery : IRequest<bool>
{
    public Guid Id { get; set; }
}