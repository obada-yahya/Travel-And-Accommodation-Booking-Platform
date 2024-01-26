using Domain.Enums;

namespace Infrastructure.Auth.Models;

public class User
{
    public string Email { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; }
    public string Salt { get; set; }
    
    public User(string email, string password, UserRole role, string salt)
    {
        Email = email;
        Password = password;
        Role = role;
        Salt = salt;
    }
}