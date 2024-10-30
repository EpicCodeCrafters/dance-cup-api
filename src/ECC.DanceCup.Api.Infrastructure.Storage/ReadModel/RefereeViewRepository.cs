using Dapper;
using ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel;
using ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel.Views;
using ECC.DanceCup.Api.Domain.Model.RefereeAggregate;
using ECC.DanceCup.Api.Infrastructure.Storage.Options;
using Microsoft.Extensions.Options;

namespace ECC.DanceCup.Api.Infrastructure.Storage.ReadModel;

public class RefereeViewRepository : PostgresRepository, IRefereeViewRepository
{
    public RefereeViewRepository(IOptions<StorageOptions> storageOptions)
    : base(storageOptions)
    {
    }

    public async Task<IReadOnlyCollection<RefereeView>> FindAllAsync(RefereeFullName? refereeFullName, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        await using var connection = await GetConnectionAsync();

        const string sqlCommand =
           """
            select "id", "full_name"
            from "referees"
            order by "id"
            limit @Limit offset @Offset;
            """;

        var referees = await connection.QueryAsync<RefereeView>(
            sqlCommand,
            new
            {
            Limit = pageSize,
            Offset = pageNumber*pageSize--,
            }
        );

        return referees.ToArray();
    }
}