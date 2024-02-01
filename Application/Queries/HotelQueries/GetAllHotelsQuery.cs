using Application.DTOs.HotelDtos;
using Domain.Common.Models;
using MediatR;

namespace Application.Queries.HotelQueries;

public record GetAllHotelsQuery : IRequest<PaginatedList<HotelWithoutRoomsDto>>
{
    public string? SearchQuery { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}