using ECC.DanceCup.Api;
using Microsoft.AspNetCore;

var host = WebHost
    .CreateDefaultBuilder(args)
    .UseStartup<Startup>()
    .ConfigureAppConfiguration(configurationBuilder =>
    {
        configurationBuilder.AddEnvironmentVariables();
        configurationBuilder.AddUserSecrets<Startup>();
    })
    .Build();

await host.RunAsync();