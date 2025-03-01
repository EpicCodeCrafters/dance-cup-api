using Dapper;
using ECC.DanceCup.Api.Application.Abstractions.Storage.DomainModel;
using ECC.DanceCup.Api.Domain.Model.UserAggregate;
using ECC.DanceCup.Api.Infrastructure.Storage.DomainModel.Mappings;
using ECC.DanceCup.Api.Infrastructure.Storage.Tools;
using ECC.DanceCup.Api.Utils.Extensions;

namespace ECC.DanceCup.Api.Infrastructure.Storage.DomainModel;

public class UserRepository : IUserRepository
{
    private readonly IPostgresConnectionFactory _connectionFactory;

    public UserRepository(IPostgresConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<UserId> InsertAsync(User user, CancellationToken cancellationToken)
    {
        await using var connection = await _connectionFactory.CreateAsync();

        const string sqlCommand =
            """
            insert into "users" ("version", "created_at", "changed_at", "external_id", "username")
            values (@Version, @CreatedAt, @ChangedAt, @ExternalId, @Username)
            returning "id";
            """;
        
        var userId = await connection.QuerySingleAsync<long>(sqlCommand, user.ToDbo());

        return UserId.From(userId).AsRequired();
    }
}