using Domain.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Common.Persistence.Repositories;

public class CityRepository : ICityRepository
{
    private readonly ApplicationDbContext _context;

    public CityRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<City>> GetAllAsync()
    {
        try
        {
            return await _context
                .Cities
                .Include(city => city.Hotels)
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception e)
        {
            return Array.Empty<City>();
        }
    }

    public async Task<City?> GetByIdAsync(Guid cityId)
    {
        try
        {
            return await _context
                .Cities
                .SingleAsync(city => city.Id.Equals(cityId));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
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
            Console.WriteLine(e.Message);
            return null;
        }
    }

    public async Task UpdateAsync(City city)
    {
        _context.Cities.Update(city);
        await SaveChangesAsync();
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