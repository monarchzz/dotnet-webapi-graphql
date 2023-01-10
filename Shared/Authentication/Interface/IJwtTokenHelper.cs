using Domain.Entities;

namespace Shared.Authentication.Interface;

public interface IJwtTokenHelper
{
    string GenerateToken(User user, string tenant);

    string GenerateRefreshToken(User user);

    Guid? VerifyRefreshToken(string refreshToken);
}