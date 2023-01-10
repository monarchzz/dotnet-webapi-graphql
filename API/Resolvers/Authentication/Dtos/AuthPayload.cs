namespace API.Resolvers.Authentication.Dtos;

public class AuthPayload
{
    public Guid UserId { get; set; }
    
    public string Token { get; set; } = null!;

    public string RefreshToken { get; set; } = null!;
}