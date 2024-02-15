using System.Text.Json;
using Application.Commands.DiscountCommands;
using Application.DTOs.DiscountDtos;
using Application.Queries.DiscountQueries;
using Application.Queries.RoomCategoryQueries;
using AutoMapper;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAABP.API.Validators.DiscountValidators;

namespace TAABP.API.Controllers;

[ApiController]
[Route("/api/room-types")]
[ApiVersion("1.0")]
public class RoomTypesController : Controller
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public RoomTypesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Retrieves all discounts associated with a room type based on its unique identifier (GUID).
    /// </summary>
    /// <param name="roomTypeId">The unique identifier of the room type.</param>
    /// <param name="allRoomTypeDiscountsDto">Query parameters for filtering discounts.</param>
    /// <returns>
    /// - 200 OK: If discounts are successfully retrieved.
    /// - 404 Not Found: If the room type with the given ID does not exist.
    /// - 500 Internal Server Error: If an unexpected error occurs during the operation.
    /// </returns>
    [HttpGet("{roomTypeId:guid}/discounts")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize(Policy = "MustBeAdmin")]
    public async Task<IActionResult> GetAllDiscountsByRoomTypeIdAsync(Guid roomTypeId,
    [FromQuery] GetAllRoomTypeDiscountsDto allRoomTypeDiscountsDto)
    {
        if (!await _mediator.Send(new CheckRoomTypeExistsQuery{Id = roomTypeId})) 
            return NotFound($"RoomType with Id {roomTypeId} doesn't exists");
        
        var allRoomTypeDiscountsQuery = _mapper.Map<GetAllRoomTypeDiscountsQuery>(allRoomTypeDiscountsDto);
        allRoomTypeDiscountsQuery.RoomTypeId = roomTypeId;
        
        var paginatedListOfAmenities = await _mediator.Send(allRoomTypeDiscountsQuery);
        Response.Headers.Add("X-Pagination",
            JsonSerializer.Serialize(paginatedListOfAmenities.PageData));

        return Ok(paginatedListOfAmenities.Items);
    }

    /// <summary>
    /// Retrieves a discount by its unique identifier (GUID).
    /// </summary>
    /// <param name="discountId">The unique identifier of the discount.</param>
    /// <returns>
    /// - 200 OK: If the discount is successfully retrieved.
    /// - 404 Not Found: If the discount with the given ID does not exist.
    /// </returns>
    [HttpGet("discounts/{discountId:guid}", Name = "GetDiscount")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    public async Task<IActionResult> GetDiscountByIdAsync(Guid discountId)
    {
        if (!await _mediator.Send(new CheckDiscountExistsQuery{Id = discountId})) 
            return NotFound($"Discount with Id {discountId} doesn't exists");

        var request = new GetDiscountByIdQuery { Id = discountId };
        return Ok(await _mediator.Send(request));
    }
    
    /// <summary>
    /// Creates a new discount.
    /// </summary>
    /// <param name="createDiscountCommand">The command containing discount creation data.</param>
    /// <returns>
    /// - 201 Created: If the discount is successfully created.
    /// - 400 Bad Request: If the request data is invalid or a conflicting discount already exists.
    /// - 404 Not Found: If the room type with the given ID does not exist.
    /// - 500 Internal Server Error: If an unexpected error occurs during the operation.
    /// </returns>
    [HttpPost("discounts")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize("MustBeAdmin")]
    public async Task<IActionResult> CreateDiscountAsync(CreateDiscountCommand createDiscountCommand)
    {
        if (!await RoomTypeExistsAsync(createDiscountCommand.RoomTypeId)) 
            return NotFound($"RoomType with Id {createDiscountCommand.RoomTypeId} doesn't exists");
        
        if (await HasOverlappingDiscount(createDiscountCommand)) 
            return BadRequest("Cannot create discount." + 
            " There is already an overlapping discount for the same room type.");
        
        var validator = new CreateRoomTypeValidator();
        var errors = await validator
            .CheckForValidationErrorsAsync(createDiscountCommand);
        if (errors.Count > 0) return BadRequest(errors);
        
        try
        {
            var discountToReturn = await _mediator.Send(createDiscountCommand);
            if (discountToReturn is null)
                return BadRequest("Failed to create discount." + 
                " Please check your request data and try again.");

            return CreatedAtRoute("GetDiscount", 
            new { discountId = discountToReturn.Id }, discountToReturn);
        }
        catch (DiscountDateException e)
        {
            return BadRequest($"Discount start date is invalid: {e.Message}");
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
       "An unexpected error occurred while processing your request. Please try again later.");
        }
    }

    /// <summary>
    /// Deletes a discount by its unique identifier (GUID).
    /// </summary>
    /// <param name="discountId">The unique identifier of the discount to delete.</param>
    /// <returns>
    /// - 204 No Content: If the discount is successfully deleted.
    /// - 404 Not Found: If the discount with the given ID does not exist.
    /// - 500 Internal Server Error: If an unexpected error occurs during the operation.
    /// </returns>
    [HttpDelete("discounts/{discountId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize("MustBeAdmin")]
    public async Task<IActionResult> DeleteDiscountAsync(Guid discountId)
    {
        if (!await _mediator.Send(new CheckDiscountExistsQuery{Id = discountId})) 
            return NotFound($"Discount with Id {discountId} doesn't exists");
        
        var deleteDiscountCommand = new DeleteDiscountCommand { Id = discountId };
        await _mediator.Send(deleteDiscountCommand);
        return NoContent();
    }
    
    private async Task<bool> RoomTypeExistsAsync(Guid roomTypeId)
    {
        return await _mediator.Send(new CheckRoomTypeExistsQuery { Id = roomTypeId });
    }
    
    private async Task<bool> HasOverlappingDiscount(CreateDiscountCommand createDiscountCommand)
    {
        return await _mediator.Send(new CheckForOverlappingDiscountQuery
        {
            RoomTypeId = createDiscountCommand.RoomTypeId,
            FromDate = createDiscountCommand.FromDate,
            ToDate = createDiscountCommand.ToDate
        });
    }
}