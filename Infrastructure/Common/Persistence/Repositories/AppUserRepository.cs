using Domain.Common.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Common.Persistence.Repositories;

public class AppUserRepository : IAppUserRepository
{
    private readonly ApplicationDbContext _context;

    public AppUserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task InsertAsync(AppUser appUser)
    {
        try
        {
            await _context.AppUsers.AddAsync(appUser);
            await SaveChangesAsync();
        }
        catch (DbUpdateException e)
        {
            if (e.InnerException != null && e.InnerException.Message.Contains("Role"))
            {
                throw new UserAlreadyExistsException("User already exists in the system.");
            }
            throw new DataConstraintViolationException("Error Adding AppUser. Check for a violation of AppUser attributes.");
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}