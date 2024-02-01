using System.Text.Json;
using Application.Commands.RoomAmenityCommands;
using Application.DTOs.RoomAmenityDtos;
using Application.Queries.RoomAmenityQueries;
using AutoMapper;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using TAABP.API.Validators.RoomAmenityValidators;

namespace TAABP.API.Controllers;

[ApiController]
[Route("api/room-amenities")]
public class RoomAmenitiesController : Controller
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    
    public RoomAmenitiesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Retrieves a list of room amenities, supporting pagination.
    /// </summary>
    /// <param name="pageSize">Number of items per page.</param>
    /// <param name="pageNumber">Page number.</param>
    /// <param name="searchQuery">Search query string.</param>
    /// <returns>Returns a list of room amenities.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Policy = "MustBeAdmin")]
    public async Task<IActionResult> GetAllRoomAmenitiesAsync(
        [FromQuery] GetAllRoomAmenitiesQuery getAllRoomAmenitiesQuery)
    {
        var validator = new GetAllRoomAmenitiesValidator();
        var errors = await validator.CheckForValidationErrorsAsync(getAllRoomAmenitiesQuery);
        if (errors.Count > 0) return BadRequest(errors);
        
        var paginatedListOfAmenities = await _mediator.Send(getAllRoomAmenitiesQuery);
        Response.Headers.Add("X-Pagination",
            JsonSerializer.Serialize(paginatedListOfAmenities.PageData));

        return Ok(paginatedListOfAmenities.Items);
    }
    
    /// <summary>
    /// Retrieves details for a specific room amenity.
    /// </summary>
    /// <param name="roomAmenityId">The unique identifier for the room amenity.</param>
    /// <returns>Returns the room amenity details.</returns>
    [HttpGet("{roomAmenityId:guid}", Name = "GetRoomAmenity")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)] 
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetRoomAmenityAsync(Guid roomAmenityId)
    {
        var request = new GetRoomAmenityByIdQuery {Id = roomAmenityId};
        var result = await _mediator.Send(request);
        if (result is null) return NotFound();
        return Ok(result);
    }
    
    /// <summary>
    /// Creates a new room amenity based on the provided data.
    /// </summary>
    /// <param name="roomAmenity">The data for creating a new room amenity.</param>
    /// <returns>Returns the created room amenity details.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RoomAmenityDto>> CreateRoomAmenityAsync(RoomAmenityForCreationDto roomAmenity)
    {
        var validator = new CreateRoomAmenityValidator();
        var errors = await validator.CheckForValidationErrorsAsync(roomAmenity);
        if (errors.Count > 0) return BadRequest(errors);
        
        var request = _mapper.Map<CreateRoomAmenityCommand>(roomAmenity);
        var amenityToReturn = await _mediator.Send(request);
        if (amenityToReturn is null)
        {
            return BadRequest();
        }
        return CreatedAtRoute("GetRoomAmenity",
            new {
                roomAmenityId = amenityToReturn.Id
            },amenityToReturn);
    }
    
    /// <summary>
    /// Deletes a room amenity with the specified ID.
    /// </summary>
    /// <param name="roomAmenityId">The unique identifier for the room amenity.</param>
    /// <returns>Indicates successful deletion.</returns>
    [HttpDelete("{roomAmenityId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteRoomAmenityAsync(Guid roomAmenityId)
    {
        var deleteRoomAmenityCommand = new DeleteRoomAmenityCommand { Id = roomAmenityId };
        try
        {
            await _mediator.Send(deleteRoomAmenityCommand);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    /// <summary>
    /// Updates an existing room amenity with the provided data.
    /// </summary>
    /// <param name="roomAmenityId">The unique identifier for the room amenity.</param>
    /// <param name="roomAmenityForUpdateDto">The data for updating the room amenity.</param>
    /// <returns>Indicates successful update.</returns>
    [HttpPut("{roomAmenityId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateRoomAmenityAsync(Guid roomAmenityId, RoomAmenityForUpdateDto roomAmenityForUpdateDto)
    {
        try
        {
            var validator = new UpdateRoomAmenityValidator();
            var errors = await validator.CheckForValidationErrorsAsync(roomAmenityForUpdateDto);
            if (errors.Count > 0) return BadRequest(errors);
        
            var updateCommand = _mapper.Map<UpdateRoomAmenityCommand>(roomAmenityForUpdateDto);
            updateCommand.Id = roomAmenityId;
            await _mediator.Send(updateCommand);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return BadRequest(e.Message);
        }
        catch (DataConstraintViolationException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    /// <summary>
    /// Partially updates an existing room amenity based on a JSON Patch document.
    /// </summary>
    /// <param name="roomAmenityId">The unique identifier for the room amenity.</param>
    /// <param name="patchDocument">JSON Patch document for partial update.</param>
    /// <returns>Indicates successful partial update.</returns>
    [HttpPatch("{roomAmenityId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> PartiallyUpdateRoomAmenityAsync(Guid roomAmenityId, JsonPatchDocument<RoomAmenityForUpdateDto> patchDocument)
    {
        try
        {
            var roomAmenityDto = await _mediator.Send(new GetRoomAmenityByIdQuery { Id = roomAmenityId });
            var roomAmenityForUpdateDto = _mapper.Map<RoomAmenityForUpdateDto>(roomAmenityDto);
            patchDocument.ApplyTo(roomAmenityForUpdateDto, ModelState);

            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!TryValidateModel(roomAmenityForUpdateDto)) return BadRequest(ModelState);

            var updateCommand = _mapper.Map<UpdateRoomAmenityCommand>(roomAmenityForUpdateDto);
            updateCommand.Id = roomAmenityId;

            await _mediator.Send(updateCommand);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (DataConstraintViolationException e)
        {
            return BadRequest(e.Message);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }
}