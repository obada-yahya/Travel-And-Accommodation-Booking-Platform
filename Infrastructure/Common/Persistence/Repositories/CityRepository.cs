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

    public async Task<IEnumerable<City>> GetAllAsync()
    {
        return await _context
            .Cities
            .Include(city => city.Hotels)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<City?> GetByIdAsync(Guid id)
    {
        try
        {
            return await _context
                .Cities
                .SingleAsync(city => city.Id == id);
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

    public async Task DeleteAsync(Guid id)
    {
        var cityToRemove = new City { Id = id };
        _context.Cities.Remove(cityToRemove);
        await SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}