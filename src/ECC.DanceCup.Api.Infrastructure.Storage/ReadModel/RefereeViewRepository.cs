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

    public async Task<IReadOnlyCollection<RefereeView>> FindAllAsync(CancellationToken cancellationToken)
    {
        await using var connection = await GetConnectionAsync();

        const string sqlCommand =
            """
            select r."id" as "id"
                 , r."full_name" as "fullname"
              from "referees" as r;
            """;

        var referees = await connection.QueryAsync<RefereeView>(sqlCommand, cancellationToken);

        return referees.ToArray();
    }
}