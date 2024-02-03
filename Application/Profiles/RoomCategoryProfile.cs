using Application.DTOs.RoomCategoriesDtos;
using Application.Queries.RoomCategoryQueries;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles;

public class RoomCategoryProfile : Profile
{
    public RoomCategoryProfile()
    {
        CreateMap<RoomType, RoomTypeDto>()
            .ForMember
            (roomDto => roomDto.Amenities,
                opt => opt.MapFrom(room => room.Amenities)
            );
        CreateMap<RoomTypeDto, RoomCategoryWithoutAmenitiesDto>();
        
        // Commands and Queries
        CreateMap<GetRoomCategoriesByHotelIdDto, GetRoomCategoriesByHotelIdQuery>();
    }
}