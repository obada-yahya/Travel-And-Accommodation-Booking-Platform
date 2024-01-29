using Application.DTOs.HotelDtos;
using MediatR;

namespace Application.Queries.UserQueries;

public record GetRecentlyVisitedHotelsForGuestQuery : IRequest<List<HotelWithoutRoomsDto>>
{
    public Guid GuestId { get; set; }
    public int Count { get; set; } = 5;
}