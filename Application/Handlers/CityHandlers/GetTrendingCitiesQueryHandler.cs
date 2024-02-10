using Application.DTOs.CityDtos;
using Application.Queries.CityQueries;
using AutoMapper;
using Domain.Common.Interfaces;
using MediatR;

namespace Application.Handlers.CityHandlers;

public class GetTrendingCitiesQueryHandler : IRequestHandler<GetTrendingCitiesQuery,List<CityWithoutHotelsDto>>
{
    private readonly ICityRepository _cityRepository;
    private readonly IMapper _mapper;

    public GetTrendingCitiesQueryHandler(ICityRepository cityRepository, IMapper mapper)
    {
        _cityRepository = cityRepository;
        _mapper = mapper;
    }

    public async Task<List<CityWithoutHotelsDto>> Handle(GetTrendingCitiesQuery request, CancellationToken cancellationToken)
    {
        var trendingCities = await _cityRepository.GetTrendingCitiesAsync(request.Count);
        return _mapper.Map<List<CityWithoutHotelsDto>>(trendingCities);
    }
}