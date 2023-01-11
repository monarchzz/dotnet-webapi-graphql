using System.Security.Claims;
using API.Authentication;
using API.Authentication.Permissions;
using API.Resolvers.Users.Dtos;
using Domain.Entities;
using EFCore.Repository;
using HotChocolate.AspNetCore.Authorization;
using Mapster;
using Shared.Authorization;

namespace API.Resolvers.Users;

[ExtendObjectType(OperationTypeNames.Query)]
public class UserQueries
{
    [Authorize]
    public async Task<UserPayload?> Profile(
        CancellationToken cancellationToken,
        [Service(ServiceKind.Synchronized)] IAppRepository<User> userRepository,
        ClaimsPrincipal claimsPrincipal
    )
    {
        var currentUser = new CurrentUser(claimsPrincipal);
        var user = await userRepository.Find(currentUser.GetUserId(), cancellationToken);

        return user?.Adapt<UserPayload>();
    }

    [Permission(Actions.View, Resources.Users)]
    public async Task<UserPayload?> GetUser(
        Guid userId,
        CancellationToken cancellationToken,
        [Service(ServiceKind.Synchronized)] IAppRepository<User> userRepository
    )
    {
        var user = await userRepository.Find(userId, cancellationToken);

        return user?.Adapt<UserPayload>();
    }
}