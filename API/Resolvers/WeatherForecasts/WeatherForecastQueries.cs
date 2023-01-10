using API.Authentication.Permissions;
using HotChocolate.AspNetCore.Authorization;
using Shared.Authorization;

namespace API.Resolvers.WeatherForecasts;

[ExtendObjectType(OperationTypeNames.Query)]
[Authorize]
public class WeatherForecastQueries
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public IEnumerable<WeatherForecast> GetWeatherForecast()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
}