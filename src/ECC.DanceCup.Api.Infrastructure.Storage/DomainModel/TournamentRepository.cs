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
                @Version,
                @CreatedAt,
                @ChangedAt,
                @UserId,
                @Name,
                @Description,
                @Date,
                @State,
                @RegistrationStartedAt,
                @RegistrationFinishedAt,
                @StartedAt,
                @FinishedAt
            )
            returning "id";
            """;

        var tournamentId = await connection.QuerySingleAsync<long>(sqlCommandTournament, tournament.ToDbo());

        const string sqlCommandCategories =
            """
            insert into "categories" ("tournament_id", "name")
            select * from unnest(@TournamentId, @Name)
            returning "id";
            """;
        
        var tournamentsIdsParameter =
            new NpgsqlParameter<long[]>("TournamentId", NpgsqlDbType.Bigint | NpgsqlDbType.Array);
        var namesParameter = new NpgsqlParameter<string[]>("Name", NpgsqlDbType.Text | NpgsqlDbType.Array);

        var categoriesDbos = tournament.Categories.Select(x => x.ToDbo()).ToArray();
        
        tournamentsIdsParameter.TypedValue = categoriesDbos.Select(x => tournamentId).ToArray();
        namesParameter.TypedValue = categoriesDbos.Select(x => x.Name).ToArray();

        var command = connection.CreateCommand();
        command.CommandText = sqlCommandCategories;
        command.Parameters.Add(tournamentsIdsParameter);
        command.Parameters.Add(namesParameter);

        var reader = await command.ExecuteReaderAsync();
        
        var categoryId = new List<long>();
        while (await reader.ReadAsync())
        {
            categoryId.Add(reader.GetInt64(0));
        }
        
        reader.Dispose();
        
        const string sqlCommandCouples =
            """
            insert into "couples" ("tournament_id", "first_participant_full_name", "second_participant_full_name", "dance_organization_name", "first_trainer_full_name", "second_trainer_full_name")
            select * from unnest(@TournamentId, @FirstParticipantFullName, @SecondParticipantFullName, @DanceOrganizationName, @FirstTrainerFullName, @SecondTrainerFullName)
            """;
        
        tournamentsIdsParameter = new NpgsqlParameter<long[]>("TournamentId", NpgsqlDbType.Bigint | NpgsqlDbType.Array);
        var firstParticipantsFullNamesParameter =
            new NpgsqlParameter<string[]>("FirstParticipantFullName", NpgsqlDbType.Text | NpgsqlDbType.Array);
        var secondParticipantsFullNamesParameter =
            new NpgsqlParameter<string?[]>("SecondParticipantFullName", NpgsqlDbType.Text | NpgsqlDbType.Array);
        var danceOrganizationsNamesParameter =
            new NpgsqlParameter<string?[]>("DanceOrganizationName", NpgsqlDbType.Text | NpgsqlDbType.Array);
        var firstTrainersFullNamesParameter =
            new NpgsqlParameter<string?[]>("FirstTrainerFullName", NpgsqlDbType.Text | NpgsqlDbType.Array);
        var secondTrainersFullNamesParameter =
            new NpgsqlParameter<string?[]>("SecondTrainerFullName", NpgsqlDbType.Text | NpgsqlDbType.Array);

        var couplesDbos = tournament.Couples.Select(x => x.ToDbo()).ToArray();
        
        tournamentsIdsParameter.TypedValue = couplesDbos.Select(x => tournamentId).ToArray();
        firstParticipantsFullNamesParameter.TypedValue = couplesDbos.Select(x => x.FirstParticipantFullName).ToArray();
        secondParticipantsFullNamesParameter.TypedValue =
            couplesDbos.Select(x => x.SecondParticipantFullName).ToArray();
        danceOrganizationsNamesParameter.TypedValue = couplesDbos.Select(x => x.DanceOrganizationName).ToArray();
        firstTrainersFullNamesParameter.TypedValue = couplesDbos.Select(x => x.FirstTrainerFullName).ToArray();
        secondTrainersFullNamesParameter.TypedValue = couplesDbos.Select(x => x.SecondTrainerFullName).ToArray();

        command = connection.CreateCommand();
        command.CommandText = sqlCommandCouples;
        command.Parameters.Add(tournamentsIdsParameter);
        command.Parameters.Add(firstParticipantsFullNamesParameter);
        command.Parameters.Add(secondParticipantsFullNamesParameter);
        command.Parameters.Add(danceOrganizationsNamesParameter);
        command.Parameters.Add(firstTrainersFullNamesParameter);
        command.Parameters.Add(secondTrainersFullNamesParameter);

        await command.ExecuteNonQueryAsync(cancellationToken);

        const string sqlCommandCategoriesCouples =
            """
            insert into "categories_couples" ("category_id", "couple_id")
            select * from unnest(@CategoryId, @CoupleId);
            """;

        var categoryIdsParameter = new NpgsqlParameter<long[]>("CategoryId", NpgsqlDbType.Bigint | NpgsqlDbType.Array);
        var coupleIdsParameter = new NpgsqlParameter<long[]>("CoupleId", NpgsqlDbType.Bigint | NpgsqlDbType.Array);

        var index = 0;
        var categoriesCouples = tournament.Categories
            .SelectMany(category => category.CouplesIds.Select(coupleId => new { Id = categoryId[index++], coupleId }))
            .ToArray();

        categoryIdsParameter.TypedValue = categoriesCouples.Select(x => x.Id).ToArray();
        coupleIdsParameter.TypedValue = categoriesCouples.Select(x => x.coupleId.Value).ToArray();
        
        command = connection.CreateCommand();
        command.CommandText = sqlCommandCategoriesCouples;
        command.Parameters.Add(categoryIdsParameter);
        command.Parameters.Add(coupleIdsParameter);
        
        await command.ExecuteNonQueryAsync(cancellationToken);

        const string sqlCommandCategoryReferee =
            """
            insert into "categories_referees" ("category_id", "referee_id")
            select * from unnest(@CategoryId, @RefereeId);
            """;
        
        categoryIdsParameter = new NpgsqlParameter<long[]>("CategoryId", NpgsqlDbType.Bigint | NpgsqlDbType.Array);
        var refereeIdsParameter = new NpgsqlParameter<long[]>("RefereeId", NpgsqlDbType.Bigint | NpgsqlDbType.Array);

        index = 0;
        var categoriesReferee = tournament.Categories
            .SelectMany(category => category.RefereesIds.Select(refereeId => new { Id = categoryId[index++], refereeId }))
            .ToArray();

        categoryIdsParameter.TypedValue = categoriesReferee.Select(x => x.Id).ToArray();
        refereeIdsParameter.TypedValue = categoriesReferee.Select(x => x.refereeId.Value).ToArray();
        
        command = connection.CreateCommand();
        command.CommandText = sqlCommandCategoryReferee;
        command.Parameters.Add(categoryIdsParameter);
        command.Parameters.Add(refereeIdsParameter);
        
        await command.ExecuteNonQueryAsync(cancellationToken);

        const string sqlCommandCategoryDance =
            """
            insert into "categories_dances" ("category_id", "dance_id")
            select * from unnest(@CategoryId, @DanceId);
            """;
        
        categoryIdsParameter = new NpgsqlParameter<long[]>("CategoryId", NpgsqlDbType.Bigint | NpgsqlDbType.Array);
        var danceIdsParametr = new NpgsqlParameter<long[]>("DanceId", NpgsqlDbType.Bigint | NpgsqlDbType.Array);

        index = 0;
        var categoriesDance = tournament.Categories
            .SelectMany(category => category.DancesIds.Select(danceId => new { Id = categoryId[index++], danceId }))
            .ToArray();

        categoryIdsParameter.TypedValue = categoriesDance.Select(x => x.Id).ToArray();
        danceIdsParametr.TypedValue = categoriesDance.Select(x => x.danceId.Value).ToArray();
        
        command = connection.CreateCommand();
        command.CommandText = sqlCommandCategoryDance;
        command.Parameters.Add(categoryIdsParameter);
        command.Parameters.Add(danceIdsParametr);
        
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
                "version" = @Version,
                "changed_at" = @Changed_at,
                "user_id" = @User_id,
                "name" = @Name,
                "description" = @Description,
                "date" = @Date,
                "state" = @State,
                "registration_started_at" = @Registration_started_at,
                "registration_finished_at" = @Registration_finished_at,
                "started_at" = @Started_at,
                "finished_at" = @Finished_at
            where "id" = @Id;
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
            where "id" = @Id;
            """;

        var tournamentDbo =
            await connection.QuerySingleOrDefaultAsync<TournamentDbo>(sqlCommand, new { id = tournamentId.Value });

        await transaction.CommitAsync(cancellationToken);

        throw new NotImplementedException();

        //return tournamentDbo?.ToDomain();
    }
}