using Application.Commands.DiscountCommands;
using Application.DTOs.DiscountDtos;
using Application.Queries.DiscountQueries;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles;

public class DiscountProfile : Profile
{
    public DiscountProfile()
    {
        CreateMap<Discount, DiscountDto>();
        CreateMap<DiscountDto, Discount>();
        
        // Commands and Queries
        CreateMap<GetAllRoomTypeDiscountsDto, GetAllRoomTypeDiscountsQuery>();
        CreateMap<DiscountForCreationDto, CreateDiscountCommand>();
        CreateMap<CreateDiscountCommand, Discount>();
    }
}