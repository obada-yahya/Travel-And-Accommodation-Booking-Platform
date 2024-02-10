using Application.DTOs.DiscountDtos;
using MediatR;

namespace Application.Queries.DiscountQueries;

public record GetDiscountByIdQuery : IRequest<DiscountDto>
{
    public Guid Id { get; set; }
}