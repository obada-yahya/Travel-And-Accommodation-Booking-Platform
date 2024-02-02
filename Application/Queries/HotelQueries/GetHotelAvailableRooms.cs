using Application.DTOs.RoomDtos;
using MediatR;

namespace Application.Queries.HotelQueries;

public record GetHotelAvailableRoomsQuery : IRequest<List<RoomDto>>
{
    public Guid HotelId { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
}