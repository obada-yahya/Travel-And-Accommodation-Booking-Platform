﻿using Application.Commands.BookingCommands;
using Application.DTOs.BookingDtos;
using AutoMapper;
using Domain.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Handlers.BookingHandlers;

public class ReserveRoomCommandHandler : IRequestHandler<ReserveRoomCommand, BookingDto?>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IMapper _mapper;

    public ReserveRoomCommandHandler(IBookingRepository bookingRepository, IMapper mapper, IUserRepository userRepository, IRoomRepository roomRepository)
    {
        _bookingRepository = bookingRepository;
        _mapper = mapper;
        _userRepository = userRepository;
        _roomRepository = roomRepository;
    }

    public async Task<BookingDto?> Handle(ReserveRoomCommand request, CancellationToken cancellationToken)
    {
        var bookingToAdd = _mapper.Map<Booking>(request);
        bookingToAdd.UserId = await _userRepository.GetGuestIdByEmailAsync(request.GuestEmail);
        bookingToAdd.Price = await _roomRepository.GetPriceForRoomWithDiscount(request.RoomId);
        var addedBooking = await _bookingRepository.InsertAsync(bookingToAdd);
        return _mapper.Map<BookingDto>(addedBooking);
    }
}