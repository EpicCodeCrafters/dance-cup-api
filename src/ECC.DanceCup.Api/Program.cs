using ECC.DanceCup.Api;
using ECC.DanceCup.Api.Extensions;
using Microsoft.AspNetCore;

var host = WebHost
    .CreateDefaultBuilder(args)
    .UseStartup<Startup>()
    .ConfigureAppConfiguration(configurationBuilder =>
    {
        configurationBuilder.AddEnvironmentVariables();
    })
    .Build();

if (args is ["--migrate"])
{
    await host.MigrateAsync();
    return;
}

await host.RunAsync();