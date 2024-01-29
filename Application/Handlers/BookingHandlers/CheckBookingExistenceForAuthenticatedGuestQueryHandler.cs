using Application.Queries.BookingQueries;
using Domain.Common.Interfaces;
using MediatR;

namespace Application.Handlers.BookingHandlers;

public class CheckBookingExistenceForAuthenticatedGuestQueryHandler : IRequestHandler<CheckBookingExistenceForAuthenticatedGuestQuery, bool>
{
    private readonly IBookingRepository _bookingRepository;

    public CheckBookingExistenceForAuthenticatedGuestQueryHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<bool> Handle(CheckBookingExistenceForAuthenticatedGuestQuery request, CancellationToken cancellationToken)
    {
        return await _bookingRepository
               .CheckBookingExistenceForGuestAsync
               (request.BookingId, request.GuestEmail);
    }
}