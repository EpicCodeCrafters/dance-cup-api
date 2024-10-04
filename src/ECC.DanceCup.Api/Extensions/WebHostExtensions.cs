using FluentMigrator.Runner;

namespace ECC.DanceCup.Api.Extensions;

public static class WebHostExtensions
{
    public static async Task MigrateAsync(this IWebHost host)
    {
        await using var scope = host.Services.CreateAsyncScope();
        var migrationRunner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
    
        migrationRunner.MigrateUp();
    }
}