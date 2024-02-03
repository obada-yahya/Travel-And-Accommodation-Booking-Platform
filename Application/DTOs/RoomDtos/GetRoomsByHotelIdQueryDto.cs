namespace Application.DTOs.RoomDtos;

public record GetRoomsByHotelIdQueryDto
{
    public string? SearchQuery { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}