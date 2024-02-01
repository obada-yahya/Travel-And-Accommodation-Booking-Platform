using Application.Commands.BookingCommands;
using Domain.Common.Interfaces;
using MediatR;

namespace Application.Handlers.BookingHandlers;

public class DeleteBookingCommandHandler : IRequestHandler<DeleteBookingCommand>
{
    private readonly IBookingRepository _bookingRepository;

    public DeleteBookingCommandHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task Handle(DeleteBookingCommand request, CancellationToken cancellationToken)
    {
        await _bookingRepository.DeleteAsync(request.Id);
    }
}