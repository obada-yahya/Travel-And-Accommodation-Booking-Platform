using Application.Queries.HotelQueries;
using AutoMapper;
using Domain.Common.Interfaces;
using Domain.Common.Models;
using MediatR;

namespace Application.Handlers.HotelHandlers;

public class HotelSearchQueryHandler : IRequestHandler<HotelSearchQuery, PaginatedList<HotelSearchResult>>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;

    public HotelSearchQueryHandler(IHotelRepository hotelRepository, IMapper mapper)
    {
        _hotelRepository = hotelRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<HotelSearchResult>> Handle(HotelSearchQuery request,
    CancellationToken cancellationToken)
    {
        var hotelSearchParameters = _mapper.Map<HotelSearchParameters>(request);
        return await _hotelRepository.HotelSearchAsync(hotelSearchParameters);
    }
}