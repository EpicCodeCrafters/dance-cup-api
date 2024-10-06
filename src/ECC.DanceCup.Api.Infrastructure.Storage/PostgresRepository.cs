using System.Data;
using ECC.DanceCup.Api.Infrastructure.Storage.Options;
using Microsoft.Extensions.Options;
using Npgsql;

namespace ECC.DanceCup.Api.Infrastructure.Storage;

public abstract class PostgresRepository
{
    private readonly IOptions<StorageOptions> _storageOptions;

    protected PostgresRepository(IOptions<StorageOptions> storageOptions)
    {
        _storageOptions = storageOptions;
    }

    protected async Task<NpgsqlConnection> GetConnectionAsync()
    {
        var connection = new NpgsqlConnection(_storageOptions.Value.ConnectionString);

        if (connection.State is ConnectionState.Closed)
        {
            await connection.OpenAsync();
        }

        return connection;
    }
}