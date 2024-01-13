using Application.DTOs.CityDtos;
using Domain.Common.Models;
using MediatR;

namespace Application.Queries.CityQueries;

public record GetCitiesQuery : IRequest<PaginatedList<CityDto>>
{
    public bool IncludeHotels { get; set; } = false;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}