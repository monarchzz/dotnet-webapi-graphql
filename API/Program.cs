using API;
using EFCore;
using Shared;

var builder = WebApplication.CreateBuilder(args);
{
    // builder.Services.AddControllers();
    // builder.Services.AddEndpointsApiExplorer();

    builder.Services
        .AddEFCore()
        .AddShare()
        .AddAPI();
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
    }

    await app.Services.InitializeDatabasesAsync();

    app.UseHttpsRedirection();
    app.UseMultiTenant();
    // app.UseAuthorization();
    // app.MapControllers();

    app.UseWebSockets()
        .UseRouting()
        .UseEndpoints(endpoint => endpoint.MapGraphQL());

    app.Run();
}