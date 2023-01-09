using API.Resolvers.Tenants;
using API.Resolvers.WeatherForecasts;
using HotChocolate.Execution.Configuration;

namespace API.Resolvers;

public static class DependencyInjection
{
    public static IServiceCollection AddResolvers(this IServiceCollection services)
    {
        services.AddGraphQLServer()
            .AddQuery()
            // .AddMutation()
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
            .AddTypeExtension<TenantQueries>();


        return builder;
    }

    private static IRequestExecutorBuilder AddMutation(this IRequestExecutorBuilder builder)
    {
        // Add type extensions
        builder.AddMutationType();

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