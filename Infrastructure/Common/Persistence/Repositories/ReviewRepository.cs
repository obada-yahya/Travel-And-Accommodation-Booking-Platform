using Domain.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Common.Persistence.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly ApplicationDbContext _context;

    public ReviewRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Review>> GetAllAsync()
    {
        try
        {
            return await _context
                .Reviews
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception e)
        {
            return Array.Empty<Review>();
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
            Console.WriteLine(e.Message);
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
            Console.WriteLine(e.Message);
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