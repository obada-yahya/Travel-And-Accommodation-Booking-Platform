using System.Text.Json;
using Application.Commands.HotelCommands;
using Application.Commands.RoomCommands;
using Application.DTOs.BookingDtos;
using Application.DTOs.HotelDtos;
using Application.DTOs.RoomCategoriesDtos;
using Application.DTOs.RoomDtos;
using Application.Queries.BookingQueries;
using Application.Queries.HotelQueries;
using Application.Queries.RoomCategoryQueries;
using Application.Queries.RoomQueries;
using AutoMapper;
using Domain.Enums;
using Domain.Exceptions;
using Infrastructure.ImageStorage;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAABP.API.Utils;
using TAABP.API.Validators.BookingValidators;
using TAABP.API.Validators.HotelValidators;
using TAABP.API.Validators.RoomValidators;

namespace TAABP.API.Controllers;

[ApiController]
[Route("/api/hotels")]
[ApiVersion("1.0")]
public class HotelsController : Controller
{
    private readonly IMediator _mediator;
    private readonly IImageService _imageService;
    private readonly IMapper _mapper;

    public HotelsController(IMapper mapper, IMediator mediator, IImageService imageService)
    {
        _mapper = mapper;
        _mediator = mediator;
        _imageService = imageService;
    }
    
    /// <summary>
    /// Retrieves all hotels.
    /// </summary>
    /// <param name="getAllHotelsQuery">Optional parameters for filtering and pagination.</param>
    /// <returns>
    /// - 200 OK: If the hotels are successfully retrieved.
    /// - 401 Unauthorized: If the user is not authorized to access the resource.
    /// - 403 Forbidden: If the user is not allowed to access the resource.
    /// - 500 Internal Server Error: If an unexpected error occurs.
    /// </returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize(Policy = "MustBeAdmin")]
    public async Task<IActionResult> GetAllHotelsAsync([FromQuery] GetAllHotelsQuery getAllHotelsQuery)
    {
        var validator = new GetAllHotelsValidator();
        var errors = await validator.CheckForValidationErrorsAsync(getAllHotelsQuery);
        if (errors.Count > 0) return BadRequest(errors);

        var paginatedListOfHotels = await _mediator.Send(getAllHotelsQuery);
        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginatedListOfHotels.PageData));

        return Ok(paginatedListOfHotels.Items);
    }

    /// <summary>
    /// Retrieves information about a hotel based on its unique identifier (GUID).
    /// </summary>
    /// <param name="hotelId">The unique identifier of the hotel.</param>
    /// <returns>
    /// - 200 OK: If the hotel information is successfully retrieved.
    /// - 404 Not Found: If the hotel with the given ID does not exist.
    /// - 500 Internal Server Error: If an unexpected error occurs.
    /// </returns>
    [HttpGet("{hotelId:guid}", Name = "GetHotel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetHotelAsync(Guid hotelId)
    {
        var request = new GetHotelByIdQuery {Id = hotelId};
        var result = await _mediator.Send(request);
        if (result is null) return NotFound();
        return Ok(result);
    }
    
    /// <summary>
    /// Creates a new hotel.
    /// </summary>
    /// <param name="hotel">The details of the hotel to be created.</param>
    /// <returns>
    /// - 201 Created: If the hotel is successfully created.
    /// - 400 Bad Request: If there are validation errors in the hotel data or if the request is malformed.
    /// - 401 Unauthorized: If the user is not authorized to create a hotel.
    /// - 500 Internal Server Error: If an unexpected error occurs.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize(Policy = "MustBeAdmin")]
    public async Task<ActionResult<HotelDto>> CreateHotelAsync(HotelForCreationDto hotel)
    {
        var validator = new CreateHotelValidator();
        var errors = await validator.CheckForValidationErrorsAsync(hotel);
        if (errors.Count > 0) return BadRequest(errors);
    
        var request = _mapper.Map<CreateHotelCommand>(hotel);
        var createdHotel = await _mediator.Send(request);
        if (createdHotel is null)
        {
            return BadRequest();
        }
        return CreatedAtRoute("GetHotel", 
        new {hotelId = createdHotel.Id},
        createdHotel);
    }
    
    /// <summary>
    /// Deletes a hotel based on its unique identifier (GUID).
    /// </summary>
    /// <param name="hotelId">The unique identifier of the hotel.</param>
    /// <returns>
    /// - 204 No Content: If the hotel is successfully deleted.
    /// - 400 Bad Request: If the hotel with the given ID does not exist.
    /// - 401 Unauthorized: If the user is not authorized to delete the hotel.
    /// - 500 Internal Server Error: If an unexpected error occurs.
    /// </returns>
    [HttpDelete("{hotelId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize(Policy = "MustBeAdmin")]
    public async Task<ActionResult> DeleteHotelAsync(Guid hotelId)
    {
        try
        {
            var deleteHotelCommand = new DeleteHotelCommand {Id = hotelId};
            await _mediator.Send(deleteHotelCommand);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    /// <summary>
    /// Updates information about a hotel based on its unique identifier (GUID).
    /// </summary>
    /// <param name="hotelId">The unique identifier of the hotel.</param>
    /// <param name="hotelForUpdateDto">The updated information for the hotel.</param>
    /// <returns>
    /// - 204 No Content: If the hotel information is successfully updated.
    /// - 400 Bad Request: If there are validation errors in the updated hotel information or if a data constraint violation occurs.
    /// - 500 Internal Server Error: If an unexpected error occurs.
    /// </returns>
    [HttpPut("{hotelId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateHotelAsync(Guid hotelId, HotelForUpdateDto hotelForUpdateDto)
    {
        try
        {
            var validator = new UpdateHotelValidator();
            var errors = await validator.CheckForValidationErrorsAsync(hotelForUpdateDto);
            if (errors.Count > 0) return BadRequest(errors);
    
            var updateCommand = _mapper.Map<UpdateHotelCommand>(hotelForUpdateDto);
            updateCommand.Id = hotelId;
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
    /// Retrieves available rooms for a hotel based on its unique identifier (GUID) and optional filtering criteria.
    /// </summary>
    /// <param name="hotelId">The unique identifier of the hotel.</param>
    /// <param name="hotelAvailableRoomsDto">Optional parameters for filtering available rooms.</param>
    /// <returns>
    /// - 200 OK: If the available rooms are successfully retrieved.
    /// - 400 Bad Request: If there are validation errors in the request parameters.
    /// - 401 Unauthorized: If the user is not authenticated.
    /// - 500 Internal Server Error: If an unexpected error occurs.
    /// </returns>
    [HttpGet("{hotelId:guid}/available-rooms")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetHotelAvailableRoomsAsync(Guid hotelId, [FromQuery] GetHotelAvailableRoomsDto hotelAvailableRoomsDto)
    {
        var validator = new GetHotelAvailableRoomsValidator();
        var errors = await validator.CheckForValidationErrorsAsync(hotelAvailableRoomsDto);
        if (errors.Count > 0) return BadRequest(errors);
        
        var request = _mapper.Map<GetHotelAvailableRoomsQuery>(hotelAvailableRoomsDto);
        request.HotelId = hotelId;
        var hotels = await _mediator.Send(request);
        return Ok(hotels);
    }

    /// <summary>
    /// Retrieves all photos associated with a hotel based on its unique identifier (GUID).
    /// </summary>
    /// <param name="hotelId">The unique identifier of the hotel.</param>
    /// <returns>
    /// - 200 OK: If the photos are successfully retrieved.
    /// - 404 Not Found: If the hotel with the given ID does not exist.
    /// - 500 Internal Server Error: If an unexpected error occurs.
    /// </returns>
    [HttpGet("{hotelId:guid}/photos")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetAllHotelPhotos(Guid hotelId)
    {
        if (!await _mediator.Send(new CheckHotelExistsQuery { Id = hotelId })) 
            return NotFound($"Hotel with ID {hotelId} does not exist");
        
        var images = await _imageService.GetAllImagesAsync(hotelId);
        return Ok(images);
    }
    
    /// <summary>
    /// Uploads an image for a hotel based on its unique identifier (GUID).
    /// </summary>
    /// <param name="hotelId">The unique identifier of the hotel.</param>
    /// <param name="file">The image file to be uploaded.</param>
    /// <returns>
    /// - 200 OK: If the image is uploaded successfully.
    /// - 404 Not Found: If the hotel with the given ID does not exist.
    /// - 400 Bad Request: If the image format is not supported.
    /// - 500 Internal Server Error: If an unexpected error occurs.
    /// </returns>
    [HttpPost("{hotelId:guid}/gallery")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize(Policy = "MustBeAdmin")]
    public async Task<IActionResult> UploadImageForHotelAsync(Guid hotelId, IFormFile file)
    {
        try
        {
            if (!await _mediator.Send(new CheckHotelExistsQuery() { Id = hotelId })) 
                return NotFound($"Hotel with ID {hotelId} does not exist");

            var imageCreationDto = await ImageUploadHelper.CreateImageCreationDto(hotelId, file, ImageType.Gallery);
            await _imageService.UploadImageAsync(imageCreationDto);
            return Ok("Image uploaded successfully.");
        }
        catch (NotSupportedException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    /// <summary>
    /// Uploads a thumbnail for a hotel based on its unique identifier (GUID).
    /// </summary>
    /// <param name="hotelId">The unique identifier of the hotel.</param>
    /// <param name="file">The image file to be uploaded.</param>
    /// <returns>
    /// - 200 OK: If the thumbnail is uploaded successfully.
    /// - 404 Not Found: If the hotel with the given ID does not exist.
    /// - 400 Bad Request: If the thumbnail format is not supported.
    /// - 500 Internal Server Error: If an unexpected error occurs during the operation.
    /// </returns>
    [HttpPut("{hotelId:guid}/thumbnail")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize("MustBeAdmin")]
    public async Task<IActionResult> UploadThumbnailForHotelAsync(Guid hotelId, IFormFile file)
    {
        try
        {
            if (!await _mediator.Send(new CheckHotelExistsQuery { Id = hotelId })) 
                return NotFound($"Hotel with ID {hotelId} does not exist");

            var imageCreationDto = await ImageUploadHelper.CreateImageCreationDto(hotelId, file, ImageType.Thumbnail);
            await _imageService.UploadThumbnailAsync(imageCreationDto);
            return Ok("Image uploaded successfully.");
        }
        catch(NotSupportedException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Deletes an image associated with a hotel based on its unique identifier (GUID).
    /// </summary>
    /// <param name="hotelId">The unique identifier of the hotel.</param>
    /// <param name="photoId">The unique identifier of the photo to be deleted.</param>
    /// <returns>
    /// - 204 No Content: If the image is deleted successfully.
    /// - 404 Not Found: If the specified hotel or photo does not exist.
    /// - 500 Internal Server Error: If an unexpected error occurs.
    /// </returns>
    [HttpDelete("{hotelId:guid}/photos/{photoId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize(Policy = "MustBeAdmin")]
    public async Task<IActionResult> DeleteHotelImageAsync(Guid hotelId, Guid photoId)
    {
        try
        {
            await _imageService.DeleteImageAsync(hotelId, photoId);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message + $" for Hotel ID {hotelId}.");
        }
        catch (InvalidOperationException e)
        {
            return StatusCode(500, new 
                { 
                    error = "Internal Server Error",
                    message = e.Message 
                });
        }
    }

    /// <summary>
    /// Retrieves all rooms available for a specific hotel based on its unique identifier (GUID).
    /// </summary>
    /// <param name="hotelId">The unique identifier of the hotel.</param>
    /// <param name="roomsByHotelDto">Optional parameters for filtering and pagination.</param>
    /// <returns>
    /// - 200 OK: If the rooms are successfully retrieved.
    /// - 404 Not Found: If the hotel with the given ID does not exist.
    /// - 500 Internal Server Error: If an unexpected error occurs.
    /// </returns>
    [HttpGet("{hotelId:guid}/rooms")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetAllHotelRoomsAsync(Guid hotelId,
    [FromQuery] GetRoomsByHotelIdDto roomsByHotelDto)
    {
        if (!await _mediator.Send(new CheckHotelExistsQuery { Id = hotelId })) 
            return NotFound($"Hotel with ID {hotelId} does not exist");

        var hotelQuery = _mapper.Map<GetRoomsByHotelIdQuery>(roomsByHotelDto);
        hotelQuery.HotelId = hotelId;
        
        var paginatedListOfHotels = await _mediator.Send(hotelQuery);
        Response.Headers.Add("X-Pagination",
            JsonSerializer.Serialize(paginatedListOfHotels.PageData));

        return Ok(paginatedListOfHotels.Items);
    }

    /// <summary>
    /// Retrieves a room from the specified hotel by its unique identifier.
    /// </summary>
    /// <param name="hotelId">The unique identifier of the hotel.</param>
    /// <param name="roomId">The unique identifier of the room.</param>
    /// <returns>
    /// Returns a response with the room details if the room exists and belongs to the specified hotel,
    /// otherwise returns a 404 Not Found response if the hotel or room does not exist or the room does not belong to the hotel,
    /// or a 500 Internal Server Error response if an unexpected error occurs.
    /// </returns>
    [HttpGet("{hotelId:guid}/rooms/{roomId:guid}", Name = "GetRoom")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetRoomByIdAndHotelIdAsync(Guid hotelId, Guid roomId)
    {
        if (!await _mediator.Send(new CheckHotelExistsQuery { Id = hotelId })) 
            return NotFound($"Hotel with ID {hotelId} doesn't exist");
        
        if(!await _mediator.Send(new CheckRoomBelongsToHotelQuery{HotelId = hotelId, RoomId = roomId}))
            return NotFound("The room doesn't belong to the hotel.");
        
        var request = new GetRoomByIdQuery{ RoomId = roomId };
        var result = await _mediator.Send(request);
        if (result is null) return NotFound();
        return Ok(result);
    }
    
    /// <summary>
    /// Creates a new room for a specific hotel based on its unique identifier (GUID).
    /// </summary>
    /// <param name="hotelId">The unique identifier of the hotel.</param>
    /// <param name="room">The details of the room to be created.</param>
    /// <returns>
    /// - 200 OK: If the room is successfully created.
    /// - 400 Bad Request: If there are validation errors in the room data.
    /// - 404 Not Found: If the hotel with the given ID does not exist.
    /// - 500 Internal Server Error: If an unexpected error occurs.
    /// </returns>
    [HttpPost("{hotelId:guid}/rooms")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize(Policy = "MustBeAdmin")]
    public async Task<IActionResult> CreateRoomForHotelAsync(Guid hotelId,
    RoomForCreationDto room)
    {
        try
        {
            var validator = new CreateRoomValidator();
            var errors = await validator.CheckForValidationErrorsAsync(room);
            if (errors.Count > 0) return BadRequest(errors);
    
            var request = _mapper.Map<CreateRoomCommand>(room);
            request.HotelId = hotelId;
        
            var createdRoom = await _mediator.Send(request);
            if (createdRoom is null) return BadRequest();
            
            return CreatedAtRoute("GetRoom", 
                new {roomId = createdRoom.Id},
                createdRoom);
        }
        catch (NotFoundException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Retrieves all room categories available for a specific hotel based on its unique identifier (GUID).
    /// </summary>
    /// <param name="hotelId">The unique identifier of the hotel.</param>
    /// <param name="categoriesByHotelIdDto">Optional parameters for filtering and pagination.</param>
    /// <returns>
    /// - 200 OK: If the room categories are successfully retrieved.
    /// - 404 Not Found: If the hotel with the given ID does not exist.
    /// - 500 Internal Server Error: If an unexpected error occurs.
    /// </returns>
    [HttpGet("{hotelId:guid}/room-types")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetAllHotelRoomCategoriesAsync(Guid hotelId,
    [FromQuery] GetRoomCategoriesByHotelIdDto categoriesByHotelIdDto)
    {
        if (!await _mediator.Send(new CheckHotelExistsQuery { Id = hotelId })) 
            return NotFound($"Hotel with ID {hotelId} doesn't exist");
        
        var hotelQuery = _mapper.Map<GetRoomCategoriesByHotelIdQuery>(categoriesByHotelIdDto);
        hotelQuery.HotelId = hotelId;
        
        var paginatedListOfRoomsCategories = await _mediator.Send(hotelQuery);
        Response.Headers.Add("X-Pagination",
            JsonSerializer.Serialize(paginatedListOfRoomsCategories.PageData));
        
        return hotelQuery.IncludeAmenities ? 
            Ok(paginatedListOfRoomsCategories.Items) : 
            Ok(_mapper
                .Map<List<RoomCategoryWithoutAmenitiesDto>>
                    (paginatedListOfRoomsCategories.Items));
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
        
        var paginatedListOfBookings = await _mediator.Send(getBookingQuery);
        Response.Headers.Add("X-Pagination",
            JsonSerializer.Serialize(paginatedListOfBookings.PageData));

        return Ok(paginatedListOfBookings.Items);
    }
}