using Application.DTOs.BookingDtos;
using MediatR;

namespace Application.Queries.BookingQueries;

public record GetBookingInvoiceQuery : IRequest<InvoiceDto>
{
    public Guid BookingId { get; set; }
}