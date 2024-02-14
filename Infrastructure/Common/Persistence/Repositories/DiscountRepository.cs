using Domain.Common.Interfaces;
using Domain.Common.Models;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Common.Persistence.Repositories;

public class DiscountRepository : IDiscountRepository
{
    private readonly ApplicationDbContext _context;

    public DiscountRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<Discount>> GetAllByRoomTypeIdAsync(Guid roomTypeId,
        int pageNumber,
        int pageSize)
    {
        try
        {
            var query = _context.Discounts
                .Where(discount => discount.RoomTypeId.Equals(roomTypeId))
                .AsQueryable();
            
            var totalItemCount = await query.CountAsync();
            var pageData = new PageData(totalItemCount, pageSize, pageNumber);

            var result = query
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .AsNoTracking()
                .ToList();

            return new PaginatedList<Discount>(result, pageData);
        }
        catch (Exception)
        {
            return new PaginatedList<Discount>(new List<Discount>(),
            new PageData(0, 0, 0));
        }
    }

    public async Task<Discount?> GetByIdAsync(Guid discountId)
    {
        return await _context
            .Discounts
            .SingleAsync(discount => discount.Id.Equals(discountId));
    }

    public async Task<bool> HasOverlappingDiscountAsync(Guid roomTypeId, 
        DateTime fromDate,
        DateTime toDate)
    {
        return await _context.Discounts.Where(discount =>
                discount.RoomTypeId.Equals(roomTypeId))
                .AnyAsync(discount =>
                    discount.FromDate.Date <= toDate.Date && 
                    discount.ToDate.Date >= fromDate.Date);
    }

    public async Task<Discount?> InsertAsync(Discount discount)
    {
        if (discount.FromDate.Date < DateTime.Today.Date)
            throw new DiscountDateException("Discount start date cannot be in the past.");
        
        await _context.Discounts.AddAsync(discount);
        await SaveChangesAsync();
        return discount;
    }
    
    public async Task DeleteAsync(Guid discountId)
    {
        _context.Discounts.Remove(new Discount { Id = discountId });
        await SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsExistsAsync(Guid discountId)
    {
        return await _context
            .Discounts
            .AnyAsync
            (discount => discount.Id.Equals(discountId));
    }
}