using Npgsql;

namespace ECC.DanceCup.Api.Infrastructure.Storage.Tools;

public interface IPostgresConnectionFactory
{
    Task<NpgsqlConnection> CreateAsync();
}