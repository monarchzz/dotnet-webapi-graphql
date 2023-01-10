using System.Security.Claims;
using Shared.Authorization;

namespace API.Authentication;

public class CurrentUser
{
    private ClaimsPrincipal? _user;

    private Guid _userId = Guid.Empty;

    public CurrentUser(ClaimsPrincipal? user = null)
    {
        _user = user;
    }

    public Guid GetUserId()
    {
        return IsAuthenticated()
            ? _user?.GetUserId() ?? _userId
            : _userId;
    }

    public string? GetTenant()
    {
        return IsAuthenticated() ? _user?.GetTenant() : string.Empty;
    }

    public string? GetUserEmail()
    {
        return IsAuthenticated()
            ? _user!.GetEmail()
            : string.Empty;
    }

    public bool IsAuthenticated()
    {
        return _user?.Identity?.IsAuthenticated is true;
    }
}