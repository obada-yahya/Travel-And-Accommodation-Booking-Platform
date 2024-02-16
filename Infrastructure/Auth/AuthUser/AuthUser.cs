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
        var user = await _context
            .Users
            .SingleOrDefaultAsync(appUser => appUser.Email.Equals(email));
        
        if (user is null) return null;
        return new User
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Password = user.PasswordHash,
            Role = user.Role,
            Salt = user.Salt
        };
    }
}