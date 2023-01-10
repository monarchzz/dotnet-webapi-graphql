using API;
using API.Authentication;
using EFCore;
using Shared;

var builder = WebApplication.CreateBuilder(args);
{
    // builder.Services.AddControllers();
    // builder.Services.AddEndpointsApiExplorer();

    builder.Services
        .AddEFCore()
        .AddShare()
        .AddAPI(builder.Configuration);
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
    }

    await app.Services.InitializeDatabasesAsync();

    app.UseHttpsRedirection();
    app.UseMultiTenant();
    app.UseAuthentication();
    app.UseAuthorization();
    // app.MapControllers();

    app.UseWebSockets()
        .UseRouting()
        .UseEndpoints(endpoint => endpoint.MapGraphQL());

    app.Run();
}