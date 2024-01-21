using Infrastructure.Auth.Models;

namespace Infrastructure.Auth.AuthUser;

public interface IAuthUser
{
    public Task<User?> GetUserAsync(string email);
}