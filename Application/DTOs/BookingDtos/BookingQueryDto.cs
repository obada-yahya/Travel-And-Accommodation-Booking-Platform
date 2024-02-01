namespace Application.DTOs.BookingDtos;

public record BookingQueryDto
{
    public string? SearchQuery { get; set; }
    public int PageNumber { get; set; } = 1;
}