using Application.Queries.CityQueries;
using Domain.Common.Interfaces;
using MediatR;

namespace Application.Handlers.CityHandlers;

public class CheckCityExistsQueryHandler : IRequestHandler<CheckCityExistsQuery, bool>
{
    private readonly ICityRepository _cityRepository;

    public CheckCityExistsQueryHandler(ICityRepository cityRepository)
    {
        _cityRepository = cityRepository;
    }
    
    public async Task<bool> Handle(CheckCityExistsQuery request, CancellationToken cancellationToken)
    {
        return await _cityRepository.IsExistsAsync(request.Id);
    }
}