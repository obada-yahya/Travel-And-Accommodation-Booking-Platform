using MediatR;

namespace Application.Queries.BookingQueries;

public record CheckBookingExistenceForAuthenticatedGuestQuery : IRequest<bool>
{
    public Guid BookingId { get; set; }
    public string GuestEmail { get; set; }
};