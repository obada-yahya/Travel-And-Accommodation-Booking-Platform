namespace Application.DTOs.DiscountDtos;

public record DiscountForCreationDto
{
    public Guid RoomTypeId { get; set; }
    public float DiscountPercentage { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
}