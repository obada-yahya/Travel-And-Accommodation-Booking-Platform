using Application.DTOs.RoomDtos;
using Application.Queries.RoomQueries;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles;

public class RoomProfile : Profile
{
    public RoomProfile()
    {
        CreateMap<Room, RoomDto>();
        
        // Commands and Queries
        CreateMap<GetRoomsByHotelIdDto, GetRoomsByHotelIdQuery>();
    }
}