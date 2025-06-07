using Dapper;
using ECC.DanceCup.Api.Application.Abstractions.Models.Views;
using ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel;
using ECC.DanceCup.Api.Domain.Model.RefereeAggregate;
using ECC.DanceCup.Api.Infrastructure.Storage.Tools;

namespace ECC.DanceCup.Api.Infrastructure.Storage.ReadModel;

public class RefereeViewRepository : IRefereeViewRepository
{
    private readonly IPostgresConnectionFactory _postgresConnectionFactory;

    public RefereeViewRepository(IPostgresConnectionFactory postgresConnectionFactory)
    {
        _postgresConnectionFactory = postgresConnectionFactory;
    }

    public async Task<IReadOnlyCollection<RefereeView>> FindAllAsync(RefereeFullName? refereeFullName, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        await using var connection = await _postgresConnectionFactory.CreateAsync();

        const string sqlCommand =
            """
            select r."id" as "id"
                 , r."full_name" as "full_name"
              from "referees" as r
             where (@FullName IS NULL OR r."full_name" = @FullName)
             order by "id"
             limit @Limit
            offset @Offset;
            """;

        var referees = await connection.QueryAsync<RefereeView>(
            sqlCommand,
            new
            {
                FullName = refereeFullName?.Value,
                Limit = pageSize,
                Offset = (pageNumber - 1) * pageSize,
            }
        );

        return referees.ToArray();
    }
}