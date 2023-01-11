using System.Security.Claims;
using API.Authentication;
using API.Errors;
using API.Resolvers.Authentication.Dtos;
using Domain.Entities;
using EFCore.Repository;
using HotChocolate.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication.Interface;

namespace API.Resolvers.Authentication;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class AuthenticationMutations
{
    public async Task<AuthPayload> Login(LoginInput input,
        CancellationToken cancellationToken,
        [Service(ServiceKind.Synchronized)] IAppRepository<User> userRepository,
        [Service()] IJwtTokenHelper jwtTokenHelper,
        [Service()] IPasswordHelper passwordHelper)
    {
        var user = await userRepository
            .NoTrackingQuery(user => user.Email == input.Email)
            .FirstOrDefaultAsync(cancellationToken);
        if (user == null) throw AppErrors.Authentication.InvalidCredentials;

        if (!passwordHelper.VerifyHashedPassword(user.Password, input.Password))
            throw AppErrors.Authentication.InvalidCredentials;

        var token = jwtTokenHelper.GenerateToken(user, userRepository.TenantIdentifier());
        var refreshToken = jwtTokenHelper.GenerateRefreshToken(user);

        return new AuthPayload
        {
            UserId = user.Id,
            Token = token,
            RefreshToken = refreshToken
        };
    }

    public async Task<AuthPayload> SignUp(SignUpInput input,
        CancellationToken cancellationToken,
        [Service(ServiceKind.Synchronized)] IAppRepository<User> userRepository,
        [Service()] IJwtTokenHelper jwtTokenHelper,
        [Service()] IPasswordHelper passwordHelper)
    {
        var user = await userRepository
            .NoTrackingQuery(user => user.Email == input.Email)
            .FirstOrDefaultAsync(cancellationToken);
        if (user != null) throw AppErrors.Authentication.EmailAlreadyExists;

        user = new User
        {
            FirstName = input.FirstName,
            LastName = input.LastName,
            Email = input.Email,
            Password = passwordHelper.HashPassword(input.Password),
        };

        await userRepository.Add(user, cancellationToken);

        var token = jwtTokenHelper.GenerateToken(user, userRepository.TenantIdentifier());
        var refreshToken = jwtTokenHelper.GenerateRefreshToken(user);

        return new AuthPayload
        {
            UserId = user.Id,
            Token = token,
            RefreshToken = refreshToken
        };
    }

    public async Task<AuthPayload> RefreshToken(RefreshTokenInput input,
        CancellationToken cancellationToken,
        [Service(ServiceKind.Synchronized)] IAppRepository<User> userRepository,
        [Service()] IJwtTokenHelper jwtTokenHelper)
    {
        var userId = jwtTokenHelper.VerifyRefreshToken(input.RefreshToken);
        if (userId == null) throw AppErrors.Authentication.TokenExpiredOrInvalid;

        var user = await userRepository.Find((Guid) userId, cancellationToken);
        if (user == null) throw AppErrors.Authentication.InvalidCredentials;

        var token = jwtTokenHelper.GenerateToken(user, userRepository.TenantIdentifier());
        var refreshToken = jwtTokenHelper.GenerateRefreshToken(user);

        return new AuthPayload
        {
            UserId = user.Id,
            Token = token,
            RefreshToken = refreshToken
        };
    }

    [Authorize]
    public async Task<bool> ChangePassword(ChangePasswordInput input,
        CancellationToken cancellationToken,
        [Service(ServiceKind.Synchronized)] IAppRepository<User> userRepository,
        [Service()] IPasswordHelper passwordHelper,
        ClaimsPrincipal claimsPrincipal
    )
    {
        var currentUser = new CurrentUser(claimsPrincipal);

        // authenticate
        var user = await userRepository.Find(currentUser.GetUserId(), cancellationToken);

        if (user == null) throw AppErrors.Authentication.InvalidCredentials;

        if (!passwordHelper.VerifyHashedPassword(user.Password, input.CurrentPassword))
            throw AppErrors.Authentication.InvalidCredentials;

        // change password
        user.Password = passwordHelper.HashPassword(input.NewPassword);

        await userRepository.Update(user, cancellationToken);

        return true;
    }
}