using Microsoft.Extensions.DependencyInjection;
using Shared.Authentication.Implementations;
using Shared.Authentication.Interface;
using Shared.Provider;

namespace Shared;

public static class DependencyInjection
{
    public static IServiceCollection AddShare(this IServiceCollection services)
    {
        services.AddSingleton<IPasswordHelper, PasswordHelper>();
        services.AddSingleton<IJwtTokenHelper, JwtTokenHelper>();

        return services;
    }
}