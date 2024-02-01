using Domain.Common.Interfaces;
using Domain.Common.Models;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Common.Persistence.Repositories;

public class HotelRepository : IHotelRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<HotelRepository> _logger;

    public HotelRepository(ApplicationDbContext context, ILogger<HotelRepository> logger)
    {
        _context = context;
        _logger = logger;
    }


    public async Task<PaginatedList<Hotel>> GetAllAsync(string? searchQuery, int pageNumber, int pageSize)
    {
        try
        {
            var query = _context.Hotels.AsQueryable();
            var totalItemCount = await query.CountAsync();
            var pageData = new PageData(totalItemCount, pageSize, pageNumber);

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                query = query.Where
                (city => city.Name.Contains(searchQuery) ||
                         city.Description.Contains(searchQuery) ||
                         city.StreetAddress.Contains(searchQuery)
                );
            }

            var result = query
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .AsNoTracking()
                .ToList();

            return new PaginatedList<Hotel>(result, pageData);
        }
        catch (Exception)
        {
            return new PaginatedList<Hotel>(new List<Hotel>(), new PageData(0, 0, 0));
        }
    }

    public async Task<Hotel?> GetByIdAsync(Guid hotelId)
    {
        try
        {
            return await _context
                .Hotels
                .AsNoTracking()
                .SingleAsync(hotel => hotel.Id.Equals(hotelId));
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
        }
        return null;
    }

    public async Task<Hotel?> InsertAsync(Hotel hotel)
    {
        try
        {
            await _context.Hotels.AddAsync(hotel);
            await SaveChangesAsync();
            return hotel;
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e.Message);
            return null;
        }
    }

    public async Task UpdateAsync(Hotel hotel)
    {
        try
        {
            _context.Hotels.Update(hotel);
            await SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            throw new DataConstraintViolationException("Error updating the hotel. Check for a violation of hotel attributes.");
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw new InvalidOperationException("Error Occured while updating hotel.");
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var hotelToRemove = new Hotel { Id = id };
        _context.Hotels.Remove(hotelToRemove);
        await _context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsExistsAsync(Guid hotelId)
    {
        return await _context
            .Hotels
            .AnyAsync
            (hotel => hotel.Id.Equals(hotelId));
    }
}