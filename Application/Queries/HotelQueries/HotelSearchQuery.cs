using Domain.Common.Models;
using MediatR;

namespace Application.Queries.HotelQueries;

public record HotelSearchQuery : IRequest<PaginatedList<HotelSearchResult>>
{
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public string? CityName { get; set; }
    public float StarRate { get; set; } = 3;
    public int Adults { get; set; } = 2;
    public int Children { get; set; } = 1;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 5;
}