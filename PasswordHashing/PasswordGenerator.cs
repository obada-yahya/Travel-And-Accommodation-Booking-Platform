namespace PasswordHashing;

public interface IPasswordGenerator
{
    public byte[] GenerateSalt();
    public string? GenerateHashedPassword(string password, byte[] salt);
    public bool VerifyPassword(string userPassword, string hashedPassword, byte[] salt);
}