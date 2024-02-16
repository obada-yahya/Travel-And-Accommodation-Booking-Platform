using System.Security.Cryptography;
using System.Text;
using Isopoh.Cryptography.Argon2;
using Microsoft.Extensions.Configuration;

namespace PasswordHashing;

public class Argon2PasswordGenerator : IPasswordGenerator
{
    private readonly int _saltSize;
    private readonly IConfiguration _configuration;

    public Argon2PasswordGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
        _saltSize = int.Parse(_configuration["PasswordGenerator:SaltSize"]);
    }

    public byte[] GenerateSalt()
    {
        var salt = new byte[_saltSize];
        var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);
        return salt;
    }
    public string? GenerateHashedPassword(string password, byte[] salt)
    {
        try
        {
            if (salt.Length < 8)
            {
                throw new InvalidOperationException("Short Salt Value, Enter 8 Bytes Or More ");
            }

            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var config = new Argon2Config
            {
                Type = Argon2Type.DataIndependentAddressing,
                Version = Argon2Version.Nineteen,
                TimeCost = int.Parse(_configuration["PasswordGenerator:TimeCost"]),
                Threads = Environment.ProcessorCount,
                Password = passwordBytes,
                Salt = salt,
                Secret = Encoding.UTF8.GetBytes(_configuration["PasswordGenerator:Secret"]),
                HashLength = int.Parse(_configuration["PasswordGenerator:HashLength"])
            };

            var argon2 = new Argon2(config);
            using var hashA = argon2.Hash();
            var hashString = config.EncodeString(hashA.Buffer);

            return hashString;
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }
    

    public bool VerifyPassword(string userPassword, string hashedPassword, byte[] salt)
    {
        try
        {
            return hashedPassword.Equals(GenerateHashedPassword(userPassword, salt));
        }
        catch (Exception)
        {
            return false;
        }
    }
}