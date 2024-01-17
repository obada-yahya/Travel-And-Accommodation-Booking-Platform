using Infrastructure.Auth.Models;

namespace Infrastructure.Auth;

public interface ITokenGenerator
{
    public Task<string> GenerateToken(
        string email,
        string password,
        string secretKey,
        string issuer,
        string audience);

    public Task<User?> ValidateUserCredentials(string email, string password);
}