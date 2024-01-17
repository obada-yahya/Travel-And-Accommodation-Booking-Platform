using Application.DTOs.CityDtos;
using MediatR;

namespace Application.Queries.CityQueries;

public record GetCityByIdQuery : IRequest<CityDto?>
{
    public Guid Id { get; set; }
    public bool IncludeHotels { get; set; }
}