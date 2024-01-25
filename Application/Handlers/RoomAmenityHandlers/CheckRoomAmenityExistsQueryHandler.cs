using Application.Queries.RoomAmenityQueries;
using Domain.Common.Interfaces;
using MediatR;

namespace Application.Handlers.RoomAmenityHandlers;

public class CheckRoomAmenityExistsQueryHandler : IRequestHandler<CheckRoomAmenityExistsQuery, bool>
{
    private readonly IRoomAmenityRepository _roomAmenityRepository;

    public CheckRoomAmenityExistsQueryHandler(IRoomAmenityRepository amenityRepository)
    {
        _roomAmenityRepository = amenityRepository;
    }

    public async Task<bool> Handle(CheckRoomAmenityExistsQuery request, CancellationToken cancellationToken)
    {
        return await _roomAmenityRepository.IsExistsAsync(request.Id);
    }
}