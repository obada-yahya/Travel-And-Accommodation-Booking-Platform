using Application.DTOs.CityDtos;
using Domain.Common;
using MediatR;

namespace Application.Queries.CityQueries;

public record GetCitiesQuery : IRequest<(IReadOnlyList<CityDto>, PaginationMetaData)>
{
    public bool IncludeHotels { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
