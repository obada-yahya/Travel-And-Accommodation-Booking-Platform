namespace Domain.Common.Models;

public record HotelSearchResult
{
    public Guid CityId { get; set; }
    public string CityName { get; set; }
    public Guid HotelId { get; set; }
    public string HotelName { get; set; }
    public float Rating{ get; set; }
    public Guid RoomId { get; set; }
    public string RoomType { get; set; }
    public float RoomPricePerNight { get; set; }
    public float Discount { get; set; }
}