namespace Application.DTOs.BookingDtos;

public record InvoiceDto
{
    public Guid Id { get; set; }
    public DateTime BookingDate { get; set; }
    public double Price { get; set; }
    public string HotelName { get; set; }
    public string OwnerName { get; set; }
}