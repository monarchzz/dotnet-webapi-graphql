using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Authentication.Interface;
using Shared.Provider;

namespace Shared.Authentication.Implementations;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenGenerator(IOptions<JwtSettings> jwtOptions)
    {
        _jwtSettings = jwtOptions.Value;
    }

    public string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var signingCredentials =
            new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
                SecurityAlgorithms.HmacSha256);
        var securityToken = new JwtSecurityToken(
            claims: claims,
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            expires: DateTimeProvider.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }


    public string GenerateRefreshToken(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var signingCredentials =
            new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.RefreshSecret)),
                SecurityAlgorithms.HmacSha256);
        var securityToken = new JwtSecurityToken(
            claims: claims,
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            expires: DateTimeProvider.UtcNow.AddMinutes(_jwtSettings.RefreshExpiryMinutes),
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }


    public Guid? VerifyToken(string refreshToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.RefreshSecret)),
        };
        try
        {
            var principal = tokenHandler.ValidateToken(refreshToken, validationParameters, out _);
            if (null != principal)
            {
                var id = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                return id == null ? null : Guid.Parse(id.Value);
            }
        }
        catch (Exception)
        {
            return null;
        }

        return null;
    }
}