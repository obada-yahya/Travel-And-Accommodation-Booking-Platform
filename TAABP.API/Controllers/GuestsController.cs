using System.Security.Claims;
using Application.Commands.BookingCommands;
using Application.DTOs.BookingDtos;
using Application.Queries.BookingQueries;
using Application.Queries.RoomQueries;
using Application.Queries.UserQueries;
using AutoMapper;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAABP.API.Validators.BookingValidators;

namespace TAABP.API.Controllers;

[ApiController]
[Route("api/guests")]
[ApiVersion("1.0")]
public class GuestsController : Controller
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    public GuestsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
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
        
        if (!await CheckAuthorizedGuestAsync(bookingId, emailClaim))
            return Unauthorized("The authenticated user is not the same as the one who booked the room");

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
    
    private async Task<bool> CheckAuthorizedGuestAsync(Guid bookingId, string? guestEmail)
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
