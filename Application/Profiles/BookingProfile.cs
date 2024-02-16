using Application.Commands.BookingCommands;
using Application.DTOs.BookingDtos;
using Application.Queries.BookingQueries;
using AutoMapper;
using Domain.Common.Models;
using Domain.Entities;

namespace Application.Profiles;

public class BookingProfile : Profile
{
    public BookingProfile()
    {
        CreateMap<Booking, BookingDto>();
        CreateMap<Invoice, InvoiceDto>();
        
        // Commands and Queries
        CreateMap<BookingQueryDto, GetBookingsByHotelIdQuery>();
        CreateMap<ReserveRoomDto, ReserveRoomCommand>();
        CreateMap<ReserveRoomCommand, Booking>();
    }
}