using Application.DTOs.CityDtos;
using MediatR;

namespace Application.Queries.CityQueries;

public record GetTrendingCitiesQuery : IRequest<List<CityWithoutHotelsDto>>
{
    public int Count { get; set; } = 5;
}