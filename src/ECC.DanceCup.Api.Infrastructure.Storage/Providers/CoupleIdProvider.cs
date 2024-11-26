using Dapper;
using ECC.DanceCup.Api.Application.Abstractions.Storage.Providers;
using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using ECC.DanceCup.Api.Infrastructure.Storage.Tools;
using ECC.DanceCup.Api.Utils.Extensions;

namespace ECC.DanceCup.Api.Infrastructure.Storage.Providers;

public class CoupleIdProvider : ICoupleIdProvider
{
    private readonly IPostgresConnectionFactory _connectionFactory;

    public CoupleIdProvider(IPostgresConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<CoupleId> CreateNewAsync(CancellationToken cancellationToken)
    {
        await using var connection = await _connectionFactory.CreateAsync();
        
        const string sqlCommand = 
            """
            select nextval("couples_ids_seq")
            """;
        
        var coupleId = await connection.QuerySingleAsync<long>(sqlCommand, cancellationToken);
        
        return CoupleId.From(coupleId).AsRequired();
    }
}