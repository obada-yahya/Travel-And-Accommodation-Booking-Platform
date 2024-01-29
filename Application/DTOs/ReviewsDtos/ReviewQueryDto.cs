namespace Application.DTOs.ReviewsDtos;

public record ReviewQueryDto
{
    public string? SearchQuery { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}