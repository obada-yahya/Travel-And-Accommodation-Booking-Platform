namespace Application.DTOs.DiscountDtos;

public record DiscountDto
{
    public Guid Id { get; set; }
    public Guid RoomTypeId { get; set; }
    public float DiscountPercentage { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
}