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
public class RoomTypesController : Controller
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public RoomTypesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
    
    [HttpGet("{roomTypeId:guid}/discounts")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
    
    [HttpPost("discounts")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
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

    [HttpDelete("discounts/{discountId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
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