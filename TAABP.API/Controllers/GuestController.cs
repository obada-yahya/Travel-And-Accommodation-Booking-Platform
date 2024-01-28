using System.Security.Claims;
using Application.Queries.UserQueries;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TAABP.API.Controllers;

[ApiController]
[Route("api/guests")]
public class GuestController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<GuestController> _logger;

    public GuestController(IMediator mediator, ILogger<GuestController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves the recent 5 distinct hotels visited by a specific guest.
    /// </summary>
    /// <param name="guestId">The ID of the guest.</param>
    /// <returns>An ActionResult containing the recent 5 distinct hotels.</returns>
    [HttpGet("{guestId:guid}/recently-visited-hotels")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
            return BadRequest(e.Message);
        }
    }
    
    /// <summary>
    /// Retrieves the recent 5 distinct hotels visited by the authenticated guest.
    /// </summary>
    /// <returns>An ActionResult containing the recent 5 distinct hotels.</returns>
    [HttpGet("recently-visited-hotels")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize]
    public async Task<IActionResult> GetRecentlyVisitedHotelsForAuthenticatedGuestAsync()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity; 
        var emailClaim = identity.Claims.FirstOrDefault(c => c.Type == "Email")?.Value;
        var request = new GetRecentlyVisitedHotelsForAuthenticatedGuestQuery { Email = emailClaim };
        return Ok(await _mediator.Send(request));
    }
}
