using Application.DTOs.CityDtos;
using Application.Queries.CityQueries;
using AutoMapper;
using Domain.Common;
using Domain.Common.Interfaces;
using MediatR;

namespace Application.Handlers.CityHandlers;

public class GetCitiesQueryHandler : IRequestHandler<GetCitiesQuery, (IReadOnlyList<CityDto>, PaginationMetaData)>
{
    private readonly ICityRepository _cityRepository;
    private readonly IMapper _mapper;
    
    public GetCitiesQueryHandler(ICityRepository cityRepository, IMapper mapper)
    {
        _cityRepository = cityRepository;
        _mapper = mapper;
    }

    public async Task<(IReadOnlyList<CityDto>, PaginationMetaData)> Handle(GetCitiesQuery request, CancellationToken cancellationToken)
    {
        var (cities, paginationMetaData) = await 
            _cityRepository
            .GetAllAsync(request.IncludeHotels,request.PageNumber,request.PageSize);
        return (_mapper.Map<IReadOnlyList<CityDto>>(cities), paginationMetaData);
    }
}