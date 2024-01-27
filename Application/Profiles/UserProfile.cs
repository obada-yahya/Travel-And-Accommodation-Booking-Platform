using Application.Commands.UserCommands;
using Application.DTOs.AppUserDtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserCommand, User>()
            .ForMember(dest => dest.PasswordHash,
            opt => opt.MapFrom(src => src.Password));
        CreateMap<UserForCreationDto, CreateUserCommand>();
    }
}