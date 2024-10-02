using DanceCup.Api;
using Microsoft.AspNetCore;

var host = WebHost
    .CreateDefaultBuilder()
    .UseStartup<Startup>()
    .ConfigureAppConfiguration(configurationBuilder =>
    {
        // TODO Раскомментировать, если понадобится
        // configurationBuilder.AddEnvironmentVariables();
        // configurationBuilder.AddUserSecrets<Startup>();
    })
    .Build();

await host.RunAsync();