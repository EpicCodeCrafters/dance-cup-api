using Dapper;
using ECC.DanceCup.Api.Application.Abstractions.Storage.DomainModel;
using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using ECC.DanceCup.Api.Infrastructure.Storage.DomainModel.Dbo;
using ECC.DanceCup.Api.Infrastructure.Storage.DomainModel.Mappings;
using ECC.DanceCup.Api.Infrastructure.Storage.Tools;
using ECC.DanceCup.Api.Utils.Extensions;
using Npgsql;
using NpgsqlTypes;

namespace ECC.DanceCup.Api.Infrastructure.Storage.DomainModel;

public class TournamentRepository : ITournamentRepository
{
    private readonly IPostgresConnectionFactory _connectionFactory;

    public TournamentRepository(IPostgresConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<TournamentId> InsertAsync(Tournament tournament, CancellationToken cancellationToken)
    {
        await using var connection = await _connectionFactory.CreateAsync();
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);
        
        const string sqlCommandTournament =
            """
            insert into "tournaments" (
                "id",
                "version",
                "created_at",
                "changed_at",
                "user_id",
                "name",
                "description",
                "date",
                "state",
                "registration_started_at",
                "registration_finished_at",
                "started_at",
                "finished_at")
            values (
                @Id,
                @Version,
                @Created_at,
                @Changed_at,
                @User_id,
                @Name,
                @Description,
                @Date,
                @State,
                @Registration_started_at,
                @Registration_finished_at,
                @Started_at,
                @Finished_at
            )
            returning "Id";
            """;

        var tournamentId = await connection.QuerySingleAsync<long>(sqlCommandTournament, tournament.ToDbo());
        
        const string sqlCommandCategories =
            """
            insert into "categories" ("id", "tournament_id", "name")
            select * from unnest(@Id, @TournamentId, @Name);
            """;

        var idsParameter = new NpgsqlParameter<long[]>("Id", NpgsqlDbType.Bigint | NpgsqlDbType.Array);
        var tournamentIdsParameter = new NpgsqlParameter<long[]>("TournamentId", NpgsqlDbType.Bigint | NpgsqlDbType.Array);
        var namesParameter = new NpgsqlParameter<string[]>("Name", NpgsqlDbType.Text | NpgsqlDbType.Array);

        var categoriesDbos = tournament.Categories.Select(x => x.ToDbo()).ToArray();
        
        idsParameter.TypedValue = categoriesDbos.Select(x => x.Id).ToArray();
        tournamentIdsParameter.TypedValue = categoriesDbos.Select(x => x.TournamentId).ToArray();
        namesParameter.TypedValue = categoriesDbos.Select(x => x.Name).ToArray();
        
        var command = connection.CreateCommand();
        command.CommandText = sqlCommandCategories;
        command.Parameters.Add(idsParameter);
        command.Parameters.Add(tournamentIdsParameter);
        command.Parameters.Add(namesParameter);

        await command.ExecuteNonQueryAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return TournamentId.From(tournamentId).AsRequired();
    }

    public async Task UpdateAsync(Tournament tournament, CancellationToken cancellationToken)
    {
        await using var connection = await _connectionFactory.CreateAsync();

        const string sqlCommand =
            """
            update "tournaments" set
                "version" = @version,
                "changed_at" = @changed_at,
                "user_id" = @user_id,
                "name" = @name,
                "description" = @description,
                "date" = @date,
                "state" = @state,
                "registration_started_at" = @registration_started_at,
                "registration_finished_at" = @registration_finished_at,
                "started_at" = @started_at,
                "finished_at" = @finished_at
            where "id" = @id;
            """;

        await connection.ExecuteAsync(sqlCommand, tournament.ToDbo());
    }

    public async Task<Tournament?> FindByIdAsync(TournamentId tournamentId, CancellationToken cancellationToken)
    {
        await using var connection = await _connectionFactory.CreateAsync();
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);

        const string sqlCommand =
            """
            select 
                "id",
                "version",
                "created_at",
                "changed_at",
                "user_id",
                "name",
                "description",
                "date",
                "state",
                "registration_started_at",
                "registration_finished_at",
                "started_at",
                "finished_at"
            from "tournaments"
            where "id" = @id;
            """;

        var tournamentDbo = await connection.QuerySingleOrDefaultAsync<TournamentDbo>(sqlCommand, new { id = tournamentId.Value });

        await transaction.CommitAsync(cancellationToken);

        throw new NotImplementedException();

        //return tournamentDbo?.ToDomain();
    }
}