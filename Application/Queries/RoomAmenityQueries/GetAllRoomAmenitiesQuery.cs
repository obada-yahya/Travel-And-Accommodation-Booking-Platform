using Application.DTOs.RoomAmenityDtos;
using Domain.Common.Models;
using MediatR;

namespace Application.Queries.RoomAmenityQueries;

public record GetAllRoomAmenitiesQuery : IRequest<PaginatedList<RoomAmenityDto>>
{
    public string? SearchQuery { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}