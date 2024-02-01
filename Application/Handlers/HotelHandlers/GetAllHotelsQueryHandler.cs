using Application.DTOs.HotelDtos;
using Application.Queries.HotelQueries;
using AutoMapper;
using Domain.Common.Interfaces;
using Domain.Common.Models;
using MediatR;

namespace Application.Handlers.HotelHandlers;

public class GetAllHotelsQueryHandler : IRequestHandler<GetAllHotelsQuery, PaginatedList<HotelWithoutRoomsDto>>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;

    public GetAllHotelsQueryHandler(IHotelRepository hotelRepository, IMapper mapper)
    {
        _hotelRepository = hotelRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<HotelWithoutRoomsDto>> Handle(GetAllHotelsQuery request, CancellationToken cancellationToken)
    {
        var paginatedList = await 
            _hotelRepository
                .GetAllAsync(
                    request.SearchQuery,
                    request.PageNumber,
                    request.PageSize);

        return new PaginatedList<HotelWithoutRoomsDto>(
            _mapper.Map<List<HotelWithoutRoomsDto>>(paginatedList.Items),
            paginatedList.PageData);
    }
}