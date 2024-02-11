using Application.DTOs.HotelDtos;
using MediatR;

namespace Application.Queries.HotelQueries;

public record GetFeaturedDealsQuery : IRequest<List<FeaturedDealDto>>
{
    public int Count { get; set; } = 5;
}
