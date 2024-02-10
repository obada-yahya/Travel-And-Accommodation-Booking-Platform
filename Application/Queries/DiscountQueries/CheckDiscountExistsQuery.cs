using MediatR;

namespace Application.Queries.DiscountQueries;

public record CheckDiscountExistsQuery : IRequest<bool>
{
    public Guid Id { get; set; }
}