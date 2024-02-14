using Application.Queries.RoomQueries;
using Domain.Common.Interfaces;
using MediatR;

namespace Application.Handlers.RoomHandlers;

public record CheckRoomBelongsToHotelQueryHandler : IRequestHandler<CheckRoomBelongsToHotelQuery, bool>
{
    private readonly IRoomRepository _roomRepository;

    public CheckRoomBelongsToHotelQueryHandler(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<bool> Handle(CheckRoomBelongsToHotelQuery request,
    CancellationToken cancellationToken)
    {
        return await _roomRepository.
        CheckRoomBelongsToHotelAsync(request.HotelId, request.RoomId);
    }
}