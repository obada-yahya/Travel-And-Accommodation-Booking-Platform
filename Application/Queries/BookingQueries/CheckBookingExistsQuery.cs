using MediatR;

namespace Application.Queries.BookingQueries;

public record CheckBookingExistsQuery : IRequest<bool>
{
    public Guid Id { get; set; }
};