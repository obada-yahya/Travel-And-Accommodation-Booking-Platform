using Domain.Common.Interfaces;
using Domain.Common.Models;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Common.Persistence.Repositories;

public class CityRepository : ICityRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CityRepository> _logger;

    public CityRepository(ApplicationDbContext context, ILogger<CityRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<PaginatedList<City>> GetAllAsync(bool includeHotels, string? searchQuery, int pageNumber, int pageSize)
    {
        try
        {
            var query = _context.Cities.AsQueryable();
            var totalItemCount = await query.CountAsync();
            var pageData = new PageData(totalItemCount, pageSize, pageNumber);
            
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                query = query.Where
                        (city => city.Name.Contains(searchQuery) ||
                        city.CountryName.Contains(searchQuery));
            }
            
            if (includeHotels)
            {
                query = query.Include(city => city.Hotels);
            }

            var result = query
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .AsNoTracking()
                .ToList();

            return new PaginatedList<City>(result, pageData);
        }
        catch (Exception)
        {
            return new PaginatedList<City>(new List<City>(), new PageData(0, 0, 0));
        }
    }

    public async Task<City?> GetByIdAsync(Guid cityId, bool includeHotels)
    {
        try
        {
            var query = _context
                .Cities
                .AsQueryable();

            if (includeHotels)
            {
                query = query.Include(city => city.Hotels);
            }

            return await query
                .AsNoTracking()
                .SingleAsync
                (city => city.Id.Equals(cityId));
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
        }
        return null;
    }

    public async Task<City?> InsertAsync(City city)
    {
        try
        {
            await _context.Cities.AddAsync(city);
            await SaveChangesAsync();
            return city;
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e.Message);
            return null;
        }
    }

    public async Task UpdateAsync(City city)
    {
        try
        {
            _context.Cities.Update(city);
            await SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            throw new DataConstraintViolationException("Error updating the city. Check for a violation of city attributes.");
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw new InvalidOperationException("Error Occured while updating city.");
        }
    }

    public async Task DeleteAsync(Guid cityId)
    {
        var cityToRemove = new City { Id = cityId };
        _context.Cities.Remove(cityToRemove);
        await SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsExistsAsync(Guid cityId)
    {
        return await _context
            .Cities
            .AnyAsync
            (city => city.Id.Equals(cityId));
    }
}