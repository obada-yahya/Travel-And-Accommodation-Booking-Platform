using Application.DTOs.HotelDtos;
using Application.Queries.HotelQueries;
using AutoMapper;
using Domain.Common.Interfaces;
using MediatR;

namespace Application.Handlers.HotelHandlers;

public class GetHotelByIdQueryHandler : IRequestHandler<GetHotelByIdQuery,HotelWithoutRoomsDto?>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;

    public GetHotelByIdQueryHandler(IHotelRepository hotelRepository, IMapper mapper)
    {
        _hotelRepository = hotelRepository;
        _mapper = mapper;
    }

    public async Task<HotelWithoutRoomsDto?> Handle(GetHotelByIdQuery request, CancellationToken cancellationToken)
    {
        var hotel = await _hotelRepository.GetByIdAsync(request.Id);
        return _mapper.Map<HotelWithoutRoomsDto>(hotel);
    }
}