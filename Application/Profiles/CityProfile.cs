using Application.Commands.CityCommands;
using Application.DTOs.CityDtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles;

public class CityProfile : Profile
{
    public CityProfile()
    {
        CreateMap<City, CityDto>()
            .ForMember
            (cityDto => cityDto.Hotels,
                opt => opt.MapFrom(city => city.Hotels));
        CreateMap<CityDto, CityWithoutHotelsDto>();
        CreateMap<City, CityWithoutHotelsDto>();
        CreateMap<CityDto, CityForUpdateDto>();
        
        // Commands and Queries
        CreateMap<CityForCreationDto, CreateCityCommand>();
        CreateMap<CityForUpdateDto, UpdateCityCommand>();
        CreateMap<UpdateCityCommand, City>();
        CreateMap<CreateCityCommand, City>();
    }
}