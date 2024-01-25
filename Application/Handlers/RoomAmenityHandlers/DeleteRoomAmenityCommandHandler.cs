using Application.Commands.RoomAmenityCommands;
using AutoMapper;
using Domain.Common.Interfaces;
using Domain.Exceptions;
using MediatR;

namespace Application.Handlers.RoomAmenityHandlers;

public class DeleteRoomAmenityCommandHandler : IRequestHandler<DeleteRoomAmenityCommand>
{
    private readonly IRoomAmenityRepository _roomAmenityRepository;
    private readonly IMapper _mapper;

    public DeleteRoomAmenityCommandHandler(IRoomAmenityRepository roomAmenityRepository)
    {
        _roomAmenityRepository = roomAmenityRepository;
    }
    
    public async Task Handle(DeleteRoomAmenityCommand request, CancellationToken cancellationToken)
    {
        if (!await _roomAmenityRepository.IsExistsAsync(request.Id))
        {
            throw new NotFoundException("Room Amenity Doesn't Exists To Delete");
        }
        await _roomAmenityRepository.DeleteAsync(request.Id);
    }
}