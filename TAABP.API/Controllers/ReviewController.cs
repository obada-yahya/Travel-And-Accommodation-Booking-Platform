using System.Resources;
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
using TAABP.API.Validators.ReviewsValidators;

namespace TAABP.API.Controllers;

[ApiController]
[Route("/api/hotels")]
public class ReviewController : Controller
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ReviewController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
    
    [HttpGet("{hotelId}/reviews")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
    
    [HttpPost("reviews")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    public async Task<ActionResult<ReviewDto>> CreateReviewAsync(ReviewForCreationDto review)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity; 
        var emailClaim = identity.Claims.FirstOrDefault(c => c.Type == "Email")?.Value;
        
        if (!await CheckBookingExistsAsync(review.BookingId))
            return NotFound($"Booking with ID {review.BookingId} does not exist");
        
        if (!await CheckAuthorizedGuestAsync(review.BookingId, emailClaim))
            return NotFound("The authenticated user is not the same as the one who booked the room");
        
        if(await CheckReviewExistsForBookingAsync(review.BookingId))
            return NotFound("You already did a review for this booking");
        
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
                BookingId = bookingId
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