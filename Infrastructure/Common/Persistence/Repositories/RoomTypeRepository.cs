﻿using Domain.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Common.Persistence.Repositories;

public class RoomTypeRepository : IRoomTypeRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger _logger;

    public RoomTypeRepository(ApplicationDbContext context, ILogger logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IReadOnlyList<RoomType>> GetAllAsync()
    {
        try
        {
            return await _context
                .RoomTypes
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception)
        {
            return Array.Empty<RoomType>();
        }
    }

    public async Task<RoomType?> GetByIdAsync(Guid roomTypeId)
    {
        try
        {
            return await _context
                .RoomTypes
                .SingleAsync(roomType => roomType.Id.Equals(roomTypeId));
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
        }
        return null;
    }

    public async Task<RoomType?> InsertAsync(RoomType roomType)
    {
        try
        {
            await _context.RoomTypes.AddAsync(roomType);
            await SaveChangesAsync();
            return roomType;
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e.Message);
            return null;
        }
    }

    public async Task UpdateAsync(RoomType roomType)
    {
        _context.RoomTypes.Update(roomType);
        await SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid roomTypeId)
    {
        var roomTypeToRemove = new RoomType { Id = roomTypeId };
        _context.RoomTypes.Remove(roomTypeToRemove);
        await SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsExistsAsync(Guid roomTypeId)
    {
        return await _context
            .RoomTypes
            .AnyAsync
            (roomType => roomType.Id.Equals(roomTypeId));
    }
}