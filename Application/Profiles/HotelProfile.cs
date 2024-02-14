using Application.Commands.HotelCommands;
using Application.DTOs.HotelDtos;
using Application.DTOs.RoomDtos;
using Application.Queries.CityQueries;
using Application.Queries.HotelQueries;
using AutoMapper;
using Domain.Common.Models;
using Domain.Entities;

namespace Application.Profiles;

public class HotelProfile : Profile
{
    public HotelProfile()
    {
        CreateMap<Hotel, HotelDto>();
        CreateMap<HotelDto, HotelWithoutRoomsDto>();
        CreateMap<Hotel, HotelWithoutRoomsDto>();
        
        // Commands and Queries
        CreateMap<HotelForCreationDto, CreateHotelCommand>();
        CreateMap<CreateHotelCommand, Hotel>();
        CreateMap<Hotel, HotelWithoutRoomsDto>();
        CreateMap<HotelForUpdateDto, UpdateHotelCommand>();
        CreateMap<UpdateHotelCommand, Hotel>();
        CreateMap<GetHotelAvailableRoomsDto, GetHotelAvailableRoomsQuery>();
        CreateMap<HotelSearchQuery, HotelSearchParameters>();
        CreateMap<FeaturedDeal, FeaturedDealDto>();
    }
}