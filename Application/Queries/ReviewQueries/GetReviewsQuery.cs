using Application.DTOs.ReviewsDtos;
using Domain.Common.Models;
using MediatR;

namespace Application.Queries.ReviewQueries;

public record GetReviewsQuery : IRequest<PaginatedList<ReviewDto>>
{
    public Guid HotelId { get; set; }
    public string? SearchQuery { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}