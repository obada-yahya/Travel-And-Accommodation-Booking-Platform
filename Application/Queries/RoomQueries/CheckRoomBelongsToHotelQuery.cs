using MediatR;

namespace Application.Queries.RoomQueries;

public record CheckRoomBelongsToHotelQuery : IRequest<bool>
{
    public Guid HotelId { get; set; }
    public Guid RoomId { get; set; }
}