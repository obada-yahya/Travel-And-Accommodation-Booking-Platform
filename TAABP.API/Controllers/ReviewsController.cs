using System.Security.Claims;
using System.Text.Json;
using Application.Commands.ReviewCommands;
using Application.DTOs.ReviewsDtos;
using Application.Queries.BookingQueries;
using Application.Queries.ReviewQueries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAABP.API.Validators.ReviewValidators;

namespace TAABP.API.Controllers;

[ApiController]
[Route("/api/reviews")]
[ApiVersion("1.0")]
public class ReviewsController : Controller
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ReviewsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Retrieves a paginated list of reviews for a specific hotel.
    /// </summary>
    /// <param name="hotelId">The ID of the hotel for which reviews are requested.</param>
    /// <param name="reviewQueryDto">DTO containing parameters for pagination and filtering.</param>
    /// <returns>
    /// Returns a paginated list of reviews for the specified hotel.
    /// </returns>
    /// <response code="200">Returns a paginated list of reviews.</response>
    /// <response code="400">Returns validation errors if the request parameters are invalid.</response>
    /// <response code="401">Returns if the user is not authenticated.</response>
    /// <response code="500">Returns if an unexpected error occurs.</response>
    [HttpGet("hotels/{hotelId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetAllReviewsAsync(Guid hotelId,
        [FromQuery] ReviewQueryDto reviewQueryDto)
    {
        var reviewQuery = _mapper.Map<GetReviewsQuery>(reviewQueryDto);
        reviewQuery.HotelId = hotelId;
        
        var validator = new ReviewsQueryValidator();
        var errors = await validator.CheckForValidationErrorsAsync(reviewQuery);
        if (errors.Count > 0) return BadRequest(errors);
        
        var paginatedListOfCities = await _mediator.Send(reviewQuery);
        Response.Headers.Add("X-Pagination", 
            JsonSerializer.Serialize(paginatedListOfCities.PageData));

        return Ok(paginatedListOfCities.Items);
    }
    
    /// <summary>
    /// Creates a new review.
    /// </summary>
    /// <param name="review">DTO containing review data.</param>
    /// <returns>
    /// Returns the created review if successful.
    /// </returns>
    /// <response code="201">Returns the created review.</response>
    /// <response code="400">Returns if the request parameters are invalid or a review already exists for the booking.</response>
    /// <response code="401">Returns if the user is not authenticated or not authorized to create a review.</response>
    /// <response code="404">Returns if the booking does not exist.</response>
    /// <response code="500">Returns if an unexpected error occurs.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<ActionResult<ReviewDto>> CreateReviewAsync(ReviewForCreationDto review)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity; 
        var emailClaim = identity.Claims.FirstOrDefault(c => c.Type == "Email")?.Value;
        
        if (!await CheckBookingExistsAsync(review.BookingId))
            return NotFound($"Booking with ID {review.BookingId} does not exist");
        
        if (!await CheckAuthorizedGuestAsync(review.BookingId, emailClaim))
            return Unauthorized("The authenticated user is not the same as the one who booked the room");
        
        if(await CheckReviewExistsForBookingAsync(review.BookingId))
            return Conflict("You already did a review for this booking");
        
        var validator = new CreateReviewValidator();
        var errors = await validator.CheckForValidationErrorsAsync(review);
        if (errors.Count > 0) return BadRequest(errors);
        
        var request = _mapper.Map<CreateReviewCommand>(review);
        var reviewToReturn = await _mediator.Send(request);
        if (reviewToReturn is null)
        {
            return BadRequest();
        }
        return Ok("Review submitted successfully!");
    }
    
    private async Task<bool> CheckBookingExistsAsync(Guid bookingId)
    {
        return await _mediator
            .Send(new CheckBookingExistsQuery
            {
                Id = bookingId
            });
    }
    
    private async Task<bool> CheckAuthorizedGuestAsync(Guid bookingId, string? guestEmail)
    {
        return await _mediator.Send(new CheckBookingExistenceForAuthenticatedGuestQuery
        {
            BookingId = bookingId,
            GuestEmail = guestEmail
        });
    }
    
    private async Task<bool> CheckReviewExistsForBookingAsync(Guid bookingId)
    {
        return await _mediator.Send(new CheckReviewExistenceForBookingQuery
        {
            BookingId = bookingId
        });
    }
}