using MediatR;

namespace Application.Queries.ReviewQueries;

public record CheckReviewExistenceForBookingQuery : IRequest<bool>
{
    public Guid BookingId { get; set; }
}