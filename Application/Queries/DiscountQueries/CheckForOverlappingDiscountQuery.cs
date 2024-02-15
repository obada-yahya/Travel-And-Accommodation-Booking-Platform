using MediatR;

namespace Application.Queries.DiscountQueries;

public record CheckForOverlappingDiscountQuery : IRequest<bool>
{
    public Guid RoomTypeId { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
}