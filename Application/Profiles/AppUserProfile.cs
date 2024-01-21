using Application.Commands.AppUserCommands;
using Application.DTOs.AppUserDtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles;

public class AppUserProfile : Profile
{
    public AppUserProfile()
    {
        CreateMap<CreateAppUserCommand, AppUser>()
            .ForMember(dest => dest.PasswordHash,
            opt => opt.MapFrom(src => src.Password));
        CreateMap<AppUserForCreationDto, CreateAppUserCommand>();
    }
}