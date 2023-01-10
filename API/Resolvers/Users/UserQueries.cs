using System.Security.Claims;
using API.Authentication;
using API.Resolvers.Users.Dtos;
using Domain.Entities;
using EFCore.Repository;
using HotChocolate.AspNetCore.Authorization;
using Mapster;

namespace API.Resolvers.Users;

[ExtendObjectType(OperationTypeNames.Query)]
[Authorize]
public class UserQueries
{
    public async Task<UserPayload?> Profile(
        CancellationToken cancellationToken,
        [Service()] IAppRepository<User> userRepository,
        ClaimsPrincipal claimsPrincipal
    )
    {
        var currentUser = new CurrentUser(claimsPrincipal);
        var user = await userRepository.Find(currentUser.GetUserId(), cancellationToken);

        return user?.Adapt<UserPayload>();
    }
}