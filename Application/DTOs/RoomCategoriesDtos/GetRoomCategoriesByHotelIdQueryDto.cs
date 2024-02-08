namespace Application.DTOs.RoomCategoriesDtos;

public record GetRoomCategoriesByHotelIdDto
{
    public bool IncludeAmenities { get; set; } = false;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}