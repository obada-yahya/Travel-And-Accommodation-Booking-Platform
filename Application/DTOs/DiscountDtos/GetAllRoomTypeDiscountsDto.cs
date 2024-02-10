namespace Application.DTOs.DiscountDtos;

public record GetAllRoomTypeDiscountsDto
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 5;
}