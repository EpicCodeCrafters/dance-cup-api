using Dapper;
using ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel;
using ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel.Views;
using ECC.DanceCup.Api.Infrastructure.Storage.Options;
using ECC.DanceCup.Api.Infrastructure.Storage.Tools;
using Microsoft.Extensions.Options;

namespace ECC.DanceCup.Api.Infrastructure.Storage.ReadModel;

public class DanceViewRepository : IDanceViewRepository
{
    private readonly IPostgresConnectionFactory _connectionFactory;

    public DanceViewRepository(IPostgresConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyCollection<DanceView>> FindAllAsync(CancellationToken cancellationToken)
    {
        await using var connection = await _connectionFactory.CreateAsync();
        
        const string sqlCommand =
            """
            select d."id" as "id"
                 , d."short_name" as "short_name"
                 , d."name" as "name"
              from "dances" as d;
            """;
        
        var dances = await connection.QueryAsync<DanceView>(sqlCommand);

        return dances.ToArray();
    }
}