using System.Data;
using ECC.DanceCup.Api.Infrastructure.Storage.Options;
using Microsoft.Extensions.Options;
using Npgsql;

namespace ECC.DanceCup.Api.Infrastructure.Storage.Repositories;

public abstract class PostgresRepositoryBase
{
    private readonly IOptions<StorageOptions> _storageOptions;

    protected PostgresRepositoryBase(IOptions<StorageOptions> storageOptions)
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