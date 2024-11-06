using System.Data;
using ECC.DanceCup.Api.Infrastructure.Storage.Options;
using Microsoft.Extensions.Options;
using Npgsql;

namespace ECC.DanceCup.Api.Infrastructure.Storage.Tools;

public class PostgresConnectionFactory : IPostgresConnectionFactory
{
    private readonly IOptions<StorageOptions> _storageOptions;

    public PostgresConnectionFactory(IOptions<StorageOptions> storageOptions)
    {
        _storageOptions = storageOptions;
    }

    public async Task<NpgsqlConnection> CreateAsync()
    {
        var connection = new NpgsqlConnection(_storageOptions.Value.ConnectionString);

        if (connection.State is ConnectionState.Closed)
        {
            await connection.OpenAsync();
        }

        return connection;
    }
}