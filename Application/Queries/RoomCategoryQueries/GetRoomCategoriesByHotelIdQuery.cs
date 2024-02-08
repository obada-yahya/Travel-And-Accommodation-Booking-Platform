using Application.DTOs.RoomCategoriesDtos;
using Domain.Common.Models;
using MediatR;

namespace Application.Queries.RoomCategoryQueries;

public record GetRoomCategoriesByHotelIdQuery : IRequest<PaginatedList<RoomTypeDto>>
{
    public Guid HotelId { get; set; }
    public bool IncludeAmenities { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}