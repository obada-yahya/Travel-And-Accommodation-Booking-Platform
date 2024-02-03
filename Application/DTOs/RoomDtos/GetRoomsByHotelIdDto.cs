﻿namespace Application.DTOs.RoomDtos;

public record GetRoomsByHotelIdDto
{
    public bool IncludeAmenities { get; set; } = true;
    public string? SearchQuery { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}