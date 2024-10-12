using Dapper;
using ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel;
using ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel.Views;
using ECC.DanceCup.Api.Infrastructure.Storage.Options;
using Microsoft.Extensions.Options;

namespace ECC.DanceCup.Api.Infrastructure.Storage.ReadModel;

public class DanceViewRepository : PostgresRepository, IDanceViewRepository
{
    public DanceViewRepository(IOptions<StorageOptions> storageOptions)
        : base(storageOptions)
    {
    }
    
    public async Task<IReadOnlyCollection<DanceView>> FindAllAsync(CancellationToken cancellationToken)
    {
        await using var connection = await GetConnectionAsync();
        
        const string sqlCommand =
            """
            select d."id" as "id"
                 , d."short_name" as "short_name"
                 , d."name" as "name"
              from "dances" as d;
            """;
        
        var dances = await connection.QueryAsync<DanceView>(sqlCommand, cancellationToken);

        return dances.ToArray();
    }
}