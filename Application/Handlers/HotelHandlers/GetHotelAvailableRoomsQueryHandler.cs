using Application.DTOs.RoomDtos;
using Application.Queries.HotelQueries;
using AutoMapper;
using Domain.Common.Interfaces;
using MediatR;

namespace Application.Handlers.HotelHandlers;

public class GetHotelAvailableRoomsQueryHandler : IRequestHandler<GetHotelAvailableRoomsQuery, List<RoomDto>>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;

    public GetHotelAvailableRoomsQueryHandler(IHotelRepository hotelRepository, IMapper mapper)
    {
        _hotelRepository = hotelRepository;
        _mapper = mapper;
    }

    public async Task<List<RoomDto>> Handle(GetHotelAvailableRoomsQuery request, CancellationToken cancellationToken)
    {
        return _mapper.Map<List<RoomDto>>(await _hotelRepository
            .GetHotelAvailableRoomsAsync(
            request.HotelId,
            request.CheckInDate,
            request.CheckOutDate));
    }
}