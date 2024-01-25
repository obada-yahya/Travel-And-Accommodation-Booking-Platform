using Application.DTOs.CityDtos;
using Application.Queries.CityQueries;
using AutoMapper;
using Domain.Common.Interfaces;
using Domain.Common.Models;
using MediatR;

namespace Application.Handlers.CityHandlers;

public class GetCitiesQueryHandler : IRequestHandler<GetCitiesQuery, PaginatedList<CityDto>>
{
    private readonly ICityRepository _cityRepository;
    private readonly IMapper _mapper;
    
    public GetCitiesQueryHandler(ICityRepository cityRepository, IMapper mapper)
    {
        _cityRepository = cityRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<CityDto>> Handle(GetCitiesQuery request, CancellationToken cancellationToken)
    {
        var paginatedList = await 
            _cityRepository
            .GetAllAsync(
            request.IncludeHotels,
            request.SearchQuery,
            request.PageNumber,
            request.PageSize);

        return new PaginatedList<CityDto>(
        _mapper.Map<List<CityDto>>(paginatedList.Items),
             paginatedList.PageData);
    }
}