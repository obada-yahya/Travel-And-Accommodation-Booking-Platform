using Application.Commands.HotelCommands;
using Application.DTOs.HotelDtos;
using AutoMapper;
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
    }
}