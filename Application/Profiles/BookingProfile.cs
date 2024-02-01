using Application.Commands.BookingCommands;
using Application.DTOs.BookingDtos;
using Application.Queries.BookingQueries;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles;

public class BookingProfile : Profile
{
    public BookingProfile()
    {
        CreateMap<Booking, BookingDto>();
        
        // Commands and Queries
        CreateMap<BookingQueryDto, GetBookingsByHotelIdQuery>();
        CreateMap<ReserveRoomDto, ReserveRoomCommand>();
        CreateMap<ReserveRoomCommand, Booking>();
    }
}