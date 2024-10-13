using Dapper;
using ECC.DanceCup.Api.Application.Abstractions.Storage.DomainModel;
using ECC.DanceCup.Api.Domain.Model.RefereeAggregate;
using ECC.DanceCup.Api.Infrastructure.Storage.DomainModel.Mappings;
using ECC.DanceCup.Api.Infrastructure.Storage.Options;
using ECC.DanceCup.Api.Utils.Extensions;
using Microsoft.Extensions.Options;

namespace ECC.DanceCup.Api.Infrastructure.Storage.DomainModel;

public class RefereeRepository : PostgresRepository, IRefereeRepository
{
    public RefereeRepository(IOptions<StorageOptions> storageOptions)
        : base(storageOptions)
    {
    }
    
    public async Task<RefereeId> AddAsync(Referee referee, CancellationToken cancellationToken)
    {
        await using var connection = await GetConnectionAsync();

        const string sqlCommand =
            """
            insert into "referees" ("version", "created_at", "changed_at", "full_name")
            values (@Version, @CreatedAt, @ChangedAt, @FullName)
            returning "id";
            """;
        
        var refereeId = await connection.QuerySingleAsync<long>(sqlCommand, referee.ToDbo());

        return RefereeId.From(refereeId).AsRequired();
    }
}