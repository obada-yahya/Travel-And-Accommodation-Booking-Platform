namespace Application.DTOs.RoomDtos;

public record GetHotelAvailableRoomsDto
{
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
}