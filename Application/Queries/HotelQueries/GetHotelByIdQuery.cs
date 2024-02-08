using Application.DTOs.HotelDtos;
using MediatR;

namespace Application.Queries.HotelQueries;

public record GetHotelByIdQuery : IRequest<HotelWithoutRoomsDto?>
{
    public Guid Id { get; set; }
}