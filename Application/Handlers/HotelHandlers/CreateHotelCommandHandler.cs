using Application.Commands.HotelCommands;
using Application.DTOs.HotelDtos;
using AutoMapper;
using Domain.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Handlers.HotelHandlers;

public class CreateHotelCommandHandler : IRequestHandler<CreateHotelCommand, HotelWithoutRoomsDto?>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;

    public CreateHotelCommandHandler(IHotelRepository hotelRepository, IMapper mapper)
    {
        _hotelRepository = hotelRepository;
        _mapper = mapper;
    }

    public async Task<HotelWithoutRoomsDto?> Handle(CreateHotelCommand request, CancellationToken cancellationToken)
    {
        var hotelToAdd = _mapper.Map<Hotel>(request);
        var addedHotel = await _hotelRepository.InsertAsync(hotelToAdd);
        return addedHotel is null ? null : _mapper.Map<HotelWithoutRoomsDto>(addedHotel);
    }
}