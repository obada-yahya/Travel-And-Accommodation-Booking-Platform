using Domain.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Common.Persistence.Repositories;

public class OwnerRepository : IOwnerRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger _logger;

    public OwnerRepository(ApplicationDbContext context, ILogger logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IReadOnlyList<Owner>> GetAllAsync()
    {
        try
        {
            return await _context
                .Owners
                .Include(owner => owner.Hotels)
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception)
        {
            return Array.Empty<Owner>();
        }
    }

    public async Task<Owner?> GetByIdAsync(Guid ownerId)
    {
        try
        {
            return await _context
                .Owners
                .SingleAsync(owner => owner.Id.Equals(ownerId));
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
        }
        return null;
    }

    public async Task<Owner?> InsertAsync(Owner owner)
    {
        try
        {
            await _context.Owners.AddAsync(owner);
            await SaveChangesAsync();
            return owner;
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e.Message);
            return null;
        }
    }

    public async Task UpdateAsync(Owner owner)
    {
        _context.Owners.Update(owner);
        await SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid ownerId)
    {
        var ownerToRemove = new Owner { Id = ownerId };
        _context.Owners.Remove(ownerToRemove);
        await SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsExistsAsync(Guid ownerId)
    {
        return await _context
            .Owners
            .AnyAsync
            (owner => owner.Id.Equals(ownerId));
    }
}