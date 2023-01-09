using Shared.Authentication.Interface;
using BC = BCrypt.Net.BCrypt;

namespace Shared.Authentication.Implementations;

public class PasswordHelper: IPasswordHelper
{
    public string HashPassword(string password)
    {
        return BC.HashPassword(password);
    }

    public bool VerifyHashedPassword(string hashedPassword, string password)
    {
        return BC.Verify(password, hashedPassword);
    }
}