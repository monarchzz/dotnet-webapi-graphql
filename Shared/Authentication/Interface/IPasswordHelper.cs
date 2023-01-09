namespace Shared.Authentication.Interface;

public interface IPasswordHelper
{
    string HashPassword(string password);

    bool VerifyHashedPassword(string hashedPassword, string password);
}