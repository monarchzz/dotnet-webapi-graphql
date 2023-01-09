using Domain.Entities;

namespace Shared.Authentication.Interface;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);

    string GenerateRefreshToken(User user);

    Guid? VerifyToken(string refreshToken);
}