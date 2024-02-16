namespace Application.DTOs.BookingDtos;

public record BookingQueryDto
{
    public string? SearchQuery { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 5;
}