using System.Text.Json;
using Application.DTOs.BookingDtos;
using Application.Queries.BookingQueries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAABP.API.Validators.BookingValidators;

namespace TAABP.API.Controllers;

[ApiController]
[Route("/api/hotels")]
public class HotelsController : Controller
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public HotelsController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }
    
    /// <summary>
    /// Retrieves all bookings for a specific hotel.
    /// </summary>
    /// <param name="bookingQuery">Optional. Query parameters for filtering, pagination, and sorting.</param>
    [HttpGet("{hotelId:guid}/bookings")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Policy = "MustBeAdmin")]
    public async Task<IActionResult> GetAllBookingsByHotelIdAsync(Guid hotelId,
        [FromQuery] BookingQueryDto bookingQuery)
    {
        var getBookingQuery = _mapper.Map<GetBookingsByHotelIdQuery>(bookingQuery);
        getBookingQuery.HotelId = hotelId;
        
        var validator = new BookingsQueryValidator();
        var errors = await validator.CheckForValidationErrorsAsync(getBookingQuery);
        if (errors.Count > 0) return BadRequest(errors);
        
        var paginatedListOfCities = await _mediator.Send(getBookingQuery);
        Response.Headers.Add("X-Pagination",
            JsonSerializer.Serialize(paginatedListOfCities.PageData));

        return Ok(paginatedListOfCities.Items);
    }
}