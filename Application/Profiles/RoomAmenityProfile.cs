using Application.Commands.RoomAmenityCommands;
using Application.DTOs.RoomAmenityDtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles;

public class RoomAmenityProfile : Profile
{
    public RoomAmenityProfile()
    {
        CreateMap<RoomAmenity, RoomAmenityDto>();
        
        // Commands and Queries
        CreateMap<RoomAmenityForCreationDto, CreateRoomAmenityCommand>();
        CreateMap<CreateRoomAmenityCommand, RoomAmenity>();
    }
}