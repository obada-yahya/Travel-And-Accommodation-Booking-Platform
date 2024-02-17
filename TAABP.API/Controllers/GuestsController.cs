using System.Security.Claims;
using Application.Commands.BookingCommands;
using Application.DTOs.BookingDtos;
using Application.Queries.BookingQueries;
using Application.Queries.RoomQueries;
using Application.Queries.UserQueries;
using AutoMapper;
using Domain.Exceptions;
using Infrastructure.Email;
using Infrastructure.Email.Models;
using Infrastructure.Pdf;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAABP.API.Utils;
using TAABP.API.Validators.BookingValidators;

namespace TAABP.API.Controllers;

[ApiController]
[Route("api/guests")]
[ApiVersion("1.0")]
public class GuestsController : Controller
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IPdfService _pdfService;
    private readonly IEmailService _emailService;
    
    public GuestsController(IMediator mediator,
        IMapper mapper,
        IPdfService pdfService,
        IEmailService emailService)
    {
        _mediator = mediator;
        _mapper = mapper;
        _pdfService = pdfService;
        _emailService = emailService;
    }

    /// <summary>
    /// Retrieves the recent 5 distinct hotels visited by a specific guest.
    /// </summary>
    /// <param name="guestId">The ID of the guest.</param>
    /// <returns>An ActionResult containing the recent 5 distinct hotels.</returns>
    [HttpGet("{guestId:guid}/recently-visited-hotels")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize(Policy = "MustBeAdmin")]
    public async Task<IActionResult> GetRecentlyVisitedHotelsForGuestAsync(Guid guestId)
    {
        try
        {
            var request = new GetRecentlyVisitedHotelsForGuestQuery { GuestId = guestId };
            return Ok(await _mediator.Send(request));
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    
    /// <summary>
    /// Retrieves the recent 5 distinct hotels visited by the authenticated guest.
    /// </summary>
    /// <returns>An ActionResult containing the recent 5 distinct hotels.</returns>
    [HttpGet("recently-visited-hotels")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetRecentlyVisitedHotelsForAuthenticatedGuestAsync()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity; 
        var emailClaim = identity.Claims.FirstOrDefault(c => c.Type == "Email")?.Value;
        var request = new GetRecentlyVisitedHotelsForAuthenticatedGuestQuery { Email = emailClaim };
        return Ok(await _mediator.Send(request));
    }

    /// <summary>
    /// Retrieves bookings for the authenticated guest.
    /// </summary>
    /// <param name="count">The maximum number of bookings to retrieve.</param>
    /// <returns>Returns the bookings for the authenticated guest.</returns>
    [HttpGet("bookings")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetBookingsForAuthenticatedGuestAsync(int count = 5)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity; 
        var emailClaim = identity.Claims.FirstOrDefault(c => c.Type == "Email")?.Value;
        
        var request = new GetBookingsForAuthenticatedGuestQuery
            {Email = emailClaim!, Count = count};
        
        return Ok(await _mediator.Send(request));
    }
    
    /// <summary>
    /// Cancels a booking for the authenticated guest.
    /// </summary>
    /// <param name="bookingId">The ID of the booking to cancel.</param>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete("bookings/{bookingId:guid}")]
    [Authorize]
    public async Task<ActionResult> CancelBookingForAuthenticatedGuestAsync(Guid bookingId)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity; 
        var emailClaim = identity.Claims.FirstOrDefault(c => c.Type == "Email")?.Value;
        
        if (!await CheckBookingExistsAsync(bookingId))
            return NotFound($"Booking with ID {bookingId} doesn't exist");
        
        if (!await IsBookingAccessibleForGuestAsync(bookingId, emailClaim))
            return Unauthorized("The authenticated guest is not the same as the one who booked the room");

        try
        {
            var deleteBookingCommand = new DeleteBookingCommand {Id = bookingId};
            await _mediator.Send(deleteBookingCommand);
            return NoContent();
        }
        catch (BookingCheckInDatePassedException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    /// <summary>
    /// Reserve a room.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/guests/bookings
    ///     {
    ///        "roomId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///        "checkInDate": "2024-02-01",
    ///        "checkOutDate": "2024-02-03"
    ///     }
    ///
    /// </remarks>
    /// <param name="booking">Booking details</param>
    [HttpPost("bookings")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> ReserveRoomForAuthenticatedGuestAsync(ReserveRoomDto booking)
    {
        var validator = new ReserveRoomValidator();
        var errors = await validator.CheckForValidationErrorsAsync(booking);
        if (errors.Count > 0) return BadRequest(errors);
        var bookingConflictMessage  = "Can't book a date between " +
                                      $"{booking.CheckInDate:yyyy-MM-dd} and " +
                                      $"{booking.CheckOutDate:yyyy-MM-dd}";
        
        var identity = HttpContext.User.Identity as ClaimsIdentity; 
        var emailClaim = identity!.Claims.FirstOrDefault(c => c.Type == "Email")?.Value;
        
        if (!await CheckRoomExistsAsync(booking.RoomId))
            return NotFound($"Room with ID {booking.RoomId} doesn't exist");
        
        var request = _mapper.Map<ReserveRoomCommand>(booking);
        request.GuestEmail = emailClaim!;
        var bookingToReturn = await _mediator.Send(request);
        if (bookingToReturn is null) 
            return BadRequest(bookingConflictMessage);
        
        return Ok("Booking has been successfully submitted!");
    }

    /// <summary>
    /// Retrieves the invoice for a specific booking associated with an authenticated guest. 
    /// </summary>
    /// <param name="bookingId">The unique identifier of the booking.</param>
    /// <param name="sendByEmail">Optional. Indicates whether to send the invoice by email. Default is false.</param>
    /// <returns>
    /// Returns the invoice file if sendByEmail is false, or a success message if the invoice is sent by email. 
    /// Returns 401 Unauthorized if the authenticated user is not authorized to access the booking. 
    /// Returns 404 Not Found if the booking associated with the provided ID does not exist. 
    /// Returns 500 Internal Server Error if an unexpected error occurs during the process.
    /// </returns>
    [HttpGet("bookings/{bookingId:guid}/invoice")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetInvoiceForAuthenticatedGuest(Guid bookingId,
    [FromQuery] bool sendByEmail = false)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity; 
        var emailClaim = identity!.Claims.First(c => c.Type == "Email").Value;
        var nameClaim = identity.Claims.First(c => c.Type == "Name").Value;
    
        if (!await CheckBookingExistsAsync(bookingId))
            return NotFound($"Booking with ID {bookingId} doesn't exist");
    
        if (!await IsBookingAccessibleForGuestAsync(bookingId, emailClaim))
            return Unauthorized("The authenticated guest is not the same as the one who booked the room");
    
        try
        {
            var invoice = await _mediator.Send(new GetBookingInvoiceQuery{BookingId = bookingId});
            var pdfBytes = await _pdfService
                .CreatePdfFromHtmlAsync(InvoiceUtils.GenerateInvoiceForUser(invoice, nameClaim));

            if (!sendByEmail) return File(pdfBytes, "application/pdf", "invoice.pdf");
            var attachments = new List<FileAttachment>
            {
                new("invoice.pdf", pdfBytes, "application/pdf")
            };
            var message = InvoiceUtils.CreateInvoiceEmailMessage(bookingId, emailClaim, nameClaim, invoice);
            await _emailService.SendEmailAsync(message, attachments);
            return Ok("Invoice sent successfully");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while generating PDF: {ex.Message}");
        }
    }
    
    private async Task<bool> IsBookingAccessibleForGuestAsync(Guid bookingId, string? guestEmail)
    {
        return await _mediator.Send(new CheckBookingExistenceForAuthenticatedGuestQuery
        {
            BookingId = bookingId,
            GuestEmail = guestEmail
        });
    }
    
    private async Task<bool> CheckBookingExistsAsync(Guid bookingId)
    {
        return await _mediator
            .Send(new CheckBookingExistsQuery
            {
                Id = bookingId
            });
    }
    
    private async Task<bool> CheckRoomExistsAsync(Guid bookingId)
    {
        return await _mediator
            .Send(new CheckRoomExistsQuery
            {
                Id = bookingId
            });
    }
}
