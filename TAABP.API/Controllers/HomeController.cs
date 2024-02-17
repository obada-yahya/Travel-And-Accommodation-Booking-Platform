using System.Text.Json;
using Application.Queries.CityQueries;
using Application.Queries.HotelQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAABP.API.Validators.HomeValidators;

namespace TAABP.API.Controllers;

[ApiController]
[Route("api/home")]
[ApiVersion("1.0")]
public class HomeController : Controller
{
    private readonly IMediator _mediator;

    public HomeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves the top 5 trending cities.
    /// </summary>
    /// <returns>The top 5 trending cities.</returns>
    [HttpGet("destinations/trending")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetTrendingCities()
    {
        var request = new GetTrendingCitiesQuery();
        return Ok(await _mediator.Send(request));
    }

    /// <summary>
    /// Searches for hotels based on the provided search query.
    /// </summary>
    /// <param name="query">The hotel search query.</param>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/hotels/search?checkInDate=2024-02-01&amp;checkOutDate=2024-02-03&amp;
    ///     cityName=New%20York&amp;starRate=4&amp;adults=2&amp;children=1&amp;pageNumber=1&amp;pageSize=5
    /// 
    /// </remarks>
    /// <returns>A list of hotels matching the search criteria.</returns>
    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> Search([FromQuery] HotelSearchQuery query)
    {
        var validators = new HotelSearchQueryValidator();
        var errors = await validators.CheckForValidationErrorsAsync(query);
        if (errors.Count > 0) return BadRequest(errors);
        
        var result = await _mediator.Send(query);
        Response.Headers.Add("X-Pagination",
            JsonSerializer.Serialize(result.PageData));
        
        return Ok(result.Items);
    }
    
    /// <summary>
    /// Retrieves the top 5 featured hotel deals.
    /// </summary>
    /// <returns>The top 5 featured hotel deals.</returns>
    [HttpGet("featured-deals")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> FeaturedDeals()
    {
        return Ok(await _mediator.Send(new GetFeaturedDealsQuery{Count = 5}));
    }
}