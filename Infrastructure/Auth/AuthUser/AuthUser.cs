using Infrastructure.Auth.Models;
using Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Auth.AuthUser;

public class AuthUser : IAuthUser
{
    private readonly ApplicationDbContext _context;

    public AuthUser(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserAsync(string email)
    {
        var appUser = await _context
            .AppUsers
            .SingleOrDefaultAsync(appUser => appUser.Email.Equals(email));

        if (appUser is null) return null;
        
        return new User(
            appUser.Email,
            appUser.PasswordHash,
            appUser.Role,
            appUser.Salt);
    }
}