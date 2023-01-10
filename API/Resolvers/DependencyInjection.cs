using API.Resolvers.Authentication;
using API.Resolvers.Tenants;
using API.Resolvers.Users;
using API.Resolvers.WeatherForecasts;
using HotChocolate.Execution.Configuration;

namespace API.Resolvers;

public static class DependencyInjection
{
    public static IServiceCollection AddResolvers(this IServiceCollection services)
    {
        services.AddGraphQLServer()
            // .AddMutationConventions(applyToAllMutations: true)
            .AddAuthorization()
            .AddQuery()
            .AddMutation()
            // .AddSubscription()
            .AddFiltering()
            .AddSorting()
            .AddProjections();

        return services;
    }

    private static IRequestExecutorBuilder AddQuery(this IRequestExecutorBuilder builder)
    {
        // Add type extensions
        builder
            .AddQueryType()
            .AddTypeExtension<WeatherForecastQueries>()
            .AddTypeExtension<TenantQueries>()
            .AddTypeExtension<UserQueries>();

        return builder;
    }

    private static IRequestExecutorBuilder AddMutation(this IRequestExecutorBuilder builder)
    {
        // Add type extensions
        builder
            .AddMutationType()
            .AddTypeExtension<TenantMutations>()
            .AddTypeExtension<AuthenticationMutations>();

        return builder;
    }

    private static IRequestExecutorBuilder AddSubscription(this IRequestExecutorBuilder builder)
    {
        // Add type extensions
        builder
            .AddSubscriptionType();

        return builder;
    }
}