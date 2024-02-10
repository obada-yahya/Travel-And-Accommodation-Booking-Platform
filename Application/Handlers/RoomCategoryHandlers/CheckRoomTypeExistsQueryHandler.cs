using Application.Queries.RoomCategoryQueries;
using Domain.Common.Interfaces;
using MediatR;

namespace Application.Handlers.RoomCategoryHandlers;

public class CheckRoomTypeExistsQueryHandler : IRequestHandler<CheckRoomTypeExistsQuery, bool>
{
    private readonly IRoomTypeRepository _roomTypeRepository;

    public CheckRoomTypeExistsQueryHandler(IRoomTypeRepository roomTypeRepository)
    {
        _roomTypeRepository = roomTypeRepository;
    }

    public Task<bool> Handle(CheckRoomTypeExistsQuery request, CancellationToken cancellationToken)
    {
        return _roomTypeRepository.IsExistsAsync(request.Id);
    }
}