using Application.Commands.RoomAmenityCommands;
using AutoMapper;
using Domain.Common.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;

namespace Application.Handlers.RoomAmenityHandlers;

public class UpdateRoomAmenityCommandHandler : IRequestHandler<UpdateRoomAmenityCommand>
{
    private readonly IRoomAmenityRepository _roomAmenityRepository;
    private readonly IMapper _mapper;

    public UpdateRoomAmenityCommandHandler(IRoomAmenityRepository roomAmenityRepository, IMapper mapper)
    {
        _roomAmenityRepository = roomAmenityRepository;
        _mapper = mapper;
    }

    public async Task Handle(UpdateRoomAmenityCommand request, CancellationToken cancellationToken)
    {
        if (!await _roomAmenityRepository.IsExistsAsync(request.Id))
        {
            throw new NotFoundException($"Room Amenity with ID {request.Id} does not exist.");
        }
        var roomAmenityToUpdate = _mapper.Map<RoomAmenity>(request);
        await _roomAmenityRepository.UpdateAsync(roomAmenityToUpdate);
    }
}