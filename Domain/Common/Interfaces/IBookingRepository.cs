﻿using Domain.Common.Models;
using Domain.Entities;

namespace Domain.Common.Interfaces;

public interface IBookingRepository
{ 
    public Task<PaginatedList<Booking>> 
    GetAllByHotelIdAsync(Guid hotelId, int pageNumber, int pageSize);
    public Task<bool> CanBookRoom(Guid roomId,
        DateTime proposedCheckIn,
        DateTime proposedCheckOut);
    public Task<Booking?> GetByIdAsync(Guid bookingId);
    public Task<Booking?> InsertAsync(Booking booking);
    public Task<Invoice> GetInvoiceByBookingIdAsync(Guid bookingId);
    public Task UpdateAsync(Booking booking);
    public Task DeleteAsync(Guid bookingId);
    public Task SaveChangesAsync();
    public Task<bool> CheckBookingExistenceForGuestAsync(Guid bookingId, string guestEmail);
    public Task<bool> IsExistsAsync(Guid bookingId);
}