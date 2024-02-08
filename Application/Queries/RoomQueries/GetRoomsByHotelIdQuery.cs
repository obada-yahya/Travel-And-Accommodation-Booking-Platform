using Application.DTOs.RoomDtos;
using Domain.Common.Models;
using MediatR;

namespace Application.Queries.RoomQueries;

public record GetRoomsByHotelIdQuery : IRequest<PaginatedList<RoomDto>>
{
    public Guid HotelId { get; set; }
    public string? SearchQuery { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}