using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Infrastructure.Auth.AuthUser;
using Infrastructure.Auth.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PasswordHashing;

namespace Infrastructure.Auth.Token;

public class JwtTokenGenerator : ITokenGenerator
{
    private readonly IAuthUser _authUser;
    private readonly IPasswordGenerator _passwordGenerator;
    private readonly IConfiguration _configuration;

    public JwtTokenGenerator(IAuthUser authUser, IPasswordGenerator passwordGenerator, IConfiguration configuration)
    {
        _authUser = authUser;
        _passwordGenerator = passwordGenerator;
        _configuration = configuration;
    }
    
    public async Task<string> GenerateToken(string email, string password, string secretKey, string issuer, string audience)
    {
        var user = await ValidateUserCredentials(email, password);
        var securityKey =
            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claimsForToken = new List<Claim>
        {
            new("Email", user.Email),
            new("Role", user.Role.ToString())
        };

        var jwtSecurityToken = new JwtSecurityToken(
            issuer,
            audience,
            claimsForToken,
            DateTime.UtcNow,
            DateTime.UtcNow
            .AddMinutes(int.Parse(
            _configuration["Authentication:TokenLifespanMinutes"]
            ?? "30")),
            signingCredentials);
        
        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }
    
    public async Task<User?> ValidateUserCredentials(string email, string password)
    {
        var user = await _authUser.GetUserAsync(email);
        if (user is null) return null;
        var isPasswordMatch = _passwordGenerator
                              .VerifyPassword(
                              password,
                              user.Password,
                              Convert.FromBase64String(user.Salt));
        return isPasswordMatch ? user : null;
    }
}