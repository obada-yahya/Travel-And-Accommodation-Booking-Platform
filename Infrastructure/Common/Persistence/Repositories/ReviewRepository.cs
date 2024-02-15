using System.Resources;
using Domain.Common.Interfaces;
using Domain.Common.Models;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Common.Persistence.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ReviewRepository> _logger;

    public ReviewRepository(ApplicationDbContext context, ILogger<ReviewRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<PaginatedList<Review>> GetAllByHotelIdAsync(Guid hotelId, string? searchQuery, int pageNumber, int pageSize)
    {
        try
        {
            var query = (from booking in _context.Bookings
                    join room in _context.Rooms on booking.RoomId equals room.Id
                    join roomType in _context.RoomTypes on room.RoomTypeId equals roomType.Id
                    join hotel in _context.Hotels on roomType.HotelId equals hotel.Id
                    join review in _context.Reviews on booking.Id equals review.BookingId
                    where roomType.HotelId == hotelId
                    select review)
                    .AsQueryable()
                    .AsNoTracking();
            
            var totalItemCount = await query.CountAsync();
            var pageData = new PageData(totalItemCount, pageSize, pageNumber);
            
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                query = query.Where
                    (review => review.Comment.Contains(searchQuery));
            }
            
            var result = query
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .AsNoTracking()
                .ToList();
        
            return new PaginatedList<Review>(result, pageData);
        }
        catch (Exception)
        {
            return new PaginatedList<Review>(new List<Review>(), 
            new PageData(0, 0, 0));
        }
    }

    public async Task<Review?> GetByIdAsync(Guid reviewId)
    {
        try
        {
            return await _context
                .Reviews
                .SingleAsync(review => review.Id.Equals(reviewId));
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
        }
        return null;
    }

    public async Task<Review?> InsertAsync(Review review)
    {
        try
        {
            await _context.Reviews.AddAsync(review);
            await SaveChangesAsync();
            return review;
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e.Message);
            return null;
        }
    }

    public async Task UpdateAsync(Review review)
    {
        _context.Reviews.Update(review);
        await SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid reviewId)
    {
        var reviewToRemove = new Review { Id = reviewId };
        _context.Reviews.Remove(reviewToRemove);
        await SaveChangesAsync();
    }

    public async Task<bool> DoesBookingHaveReviewAsync(Guid bookingId)
    {
        return await _context
            .Reviews
            .AnyAsync
            (review => 
                review
                .BookingId
                .Equals(bookingId));
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsExistsAsync(Guid reviewId)
    {
        return await _context
            .Reviews
            .AnyAsync
            (review => review.Id.Equals(reviewId));
    }
}