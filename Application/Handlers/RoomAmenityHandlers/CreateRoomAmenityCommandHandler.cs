using Application.Commands.RoomAmenityCommands;
using Application.DTOs.RoomAmenityDtos;
using AutoMapper;
using Domain.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Handlers.RoomAmenityHandlers;

public class CreateRoomAmenityCommandHandler : IRequestHandler<CreateRoomAmenityCommand, RoomAmenityDto?>
{
    private readonly IRoomAmenityRepository _roomAmenityRepository;
    private readonly IMapper _mapper;

    public CreateRoomAmenityCommandHandler(IRoomAmenityRepository roomAmenityRepository, IMapper mapper)
    {
        _roomAmenityRepository = roomAmenityRepository;
        _mapper = mapper;
    }

    public async Task<RoomAmenityDto?> Handle(CreateRoomAmenityCommand request, CancellationToken cancellationToken)
    {
        var amenityToAdd = _mapper.Map<RoomAmenity>(request);
        var addedAmenity = await _roomAmenityRepository.InsertAsync(amenityToAdd);
        return addedAmenity is null ? null : _mapper.Map<RoomAmenityDto>(addedAmenity);
    }
}