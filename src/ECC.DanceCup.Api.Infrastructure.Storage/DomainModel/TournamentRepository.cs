using Dapper;
using ECC.DanceCup.Api.Application.Abstractions.Storage.DomainModel;
using ECC.DanceCup.Api.Domain.Model.DanceAggregate;
using ECC.DanceCup.Api.Domain.Model.RefereeAggregate;
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

        const string sqlCommandSetTournament =
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

        var tournamentId = await connection.QuerySingleAsync<long>(sqlCommandSetTournament, tournament.ToDbo());

        const string sqlCommandSetCategories =
            """
            insert into "categories" ("tournament_id", "name")
            select * from unnest(@TournamentId, @Name)
            returning "id";
            """;
        
        var tournamentsIdsParameter = new NpgsqlParameter<long[]>("TournamentId", NpgsqlDbType.Bigint | NpgsqlDbType.Array);
        var namesParameter = new NpgsqlParameter<string[]>("Name", NpgsqlDbType.Text | NpgsqlDbType.Array);

        var categoriesDbos = tournament.Categories.Select(x => x.ToDbo()).ToArray();
        
        tournamentsIdsParameter.TypedValue = categoriesDbos.Select(x => tournamentId).ToArray();
        namesParameter.TypedValue = categoriesDbos.Select(x => x.Name).ToArray();

        var command = connection.CreateCommand();
        command.CommandText = sqlCommandSetCategories;
        command.Parameters.Add(tournamentsIdsParameter);
        command.Parameters.Add(namesParameter);

        var reader = await command.ExecuteReaderAsync();
        
        var categoryId = new List<long>();
        while (await reader.ReadAsync())
        {
            categoryId.Add(reader.GetInt64(0));
        }
        
        reader.Dispose();
        
        const string sqlCommandSetCouples =
            """
            insert into "couples" ("tournament_id", "first_participant_full_name", "second_participant_full_name", "dance_organization_name", "first_trainer_full_name", "second_trainer_full_name")
            select * from unnest(@TournamentId, @FirstParticipantFullName, @SecondParticipantFullName, @DanceOrganizationName, @FirstTrainerFullName, @SecondTrainerFullName)
            """;
        
        tournamentsIdsParameter = new NpgsqlParameter<long[]>("TournamentId", NpgsqlDbType.Bigint | NpgsqlDbType.Array);
        var firstParticipantsFullNamesParameter = new NpgsqlParameter<string[]>("FirstParticipantFullName", NpgsqlDbType.Text | NpgsqlDbType.Array);
        var secondParticipantsFullNamesParameter = new NpgsqlParameter<string?[]>("SecondParticipantFullName", NpgsqlDbType.Text | NpgsqlDbType.Array);
        var danceOrganizationsNamesParameter = new NpgsqlParameter<string?[]>("DanceOrganizationName", NpgsqlDbType.Text | NpgsqlDbType.Array);
        var firstTrainersFullNamesParameter = new NpgsqlParameter<string?[]>("FirstTrainerFullName", NpgsqlDbType.Text | NpgsqlDbType.Array);
        var secondTrainersFullNamesParameter = new NpgsqlParameter<string?[]>("SecondTrainerFullName", NpgsqlDbType.Text | NpgsqlDbType.Array);

        var couplesDbos = tournament.Couples.Select(x => x.ToDbo()).ToArray();
        
        tournamentsIdsParameter.TypedValue = couplesDbos.Select(x => tournamentId).ToArray();
        firstParticipantsFullNamesParameter.TypedValue = couplesDbos.Select(x => x.FirstParticipantFullName).ToArray();
        secondParticipantsFullNamesParameter.TypedValue = couplesDbos.Select(x => x.SecondParticipantFullName).ToArray();
        danceOrganizationsNamesParameter.TypedValue = couplesDbos.Select(x => x.DanceOrganizationName).ToArray();
        firstTrainersFullNamesParameter.TypedValue = couplesDbos.Select(x => x.FirstTrainerFullName).ToArray();
        secondTrainersFullNamesParameter.TypedValue = couplesDbos.Select(x => x.SecondTrainerFullName).ToArray();

        command = connection.CreateCommand();
        command.CommandText = sqlCommandSetCouples;
        command.Parameters.Add(tournamentsIdsParameter);
        command.Parameters.Add(firstParticipantsFullNamesParameter);
        command.Parameters.Add(secondParticipantsFullNamesParameter);
        command.Parameters.Add(danceOrganizationsNamesParameter);
        command.Parameters.Add(firstTrainersFullNamesParameter);
        command.Parameters.Add(secondTrainersFullNamesParameter);

        await command.ExecuteNonQueryAsync(cancellationToken);

        const string sqlCommandSetCategoriesCouples =
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
        command.CommandText = sqlCommandSetCategoriesCouples;
        command.Parameters.Add(categoryIdsParameter);
        command.Parameters.Add(coupleIdsParameter);
        
        await command.ExecuteNonQueryAsync(cancellationToken);

        const string sqlCommandSetCategoryReferee =
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
        command.CommandText = sqlCommandSetCategoryReferee;
        command.Parameters.Add(categoryIdsParameter);
        command.Parameters.Add(refereeIdsParameter);
        
        await command.ExecuteNonQueryAsync(cancellationToken);

        const string sqlCommandSetCategoryDance =
            """
            insert into "categories_dances" ("category_id", "dance_id")
            select * from unnest(@CategoryId, @DanceId);
            """;
        
        categoryIdsParameter = new NpgsqlParameter<long[]>("CategoryId", NpgsqlDbType.Bigint | NpgsqlDbType.Array);
        var danceIdsParametr = new NpgsqlParameter<long[]>("DanceId", NpgsqlDbType.Bigint | NpgsqlDbType.Array);

        var categoriesDance = tournament.Categories
            .Select((category, i) => new { category, categoryId = categoryId[i] })
            .SelectMany(x => x.category.DancesIds.Select(danceId => new { Id = x.categoryId, danceId }))
            .ToArray();

        categoryIdsParameter.TypedValue = categoriesDance.Select(x => x.Id).ToArray();
        danceIdsParametr.TypedValue = categoriesDance.Select(x => x.danceId.Value).ToArray();
        
        command = connection.CreateCommand();
        command.CommandText = sqlCommandSetCategoryDance;
        command.Parameters.Add(categoryIdsParameter);
        command.Parameters.Add(danceIdsParametr);
        
        await command.ExecuteNonQueryAsync(cancellationToken);
        
        await transaction.CommitAsync(cancellationToken);

        return TournamentId.From(tournamentId).AsRequired();
    }

    public async Task UpdateAsync(Tournament tournament, CancellationToken cancellationToken)
    {
        await using var connection = await _connectionFactory.CreateAsync();
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);

        const string sqlCommandUpdateTournament =
            """
            update "tournaments" set
                "version" = @Version,
                "changed_at" = @ChangedAt,
                "user_id" = @UserId,
                "name" = @Name,
                "description" = @Description,
                "date" = @Date,
                "state" = @State,
                "registration_started_at" = @RegistrationStartedAt,
                "registration_finished_at" = @RegistrationFinishedAt,
                "started_at" = @StartedAt,
                "finished_at" = @FinishedAt
            where "id" = @Id;
            """;

        await connection.ExecuteAsync(sqlCommandUpdateTournament, tournament.ToDbo());

        const string sqlCommandUpdateCategories =
            """
            UPDATE categories AS c
            SET 
                name = v.name
            FROM (
                SELECT * FROM unnest(
                    @CategoryIds,         -- id
                    @Names                -- name
                ) AS v(id, name)
            ) AS v
            WHERE c.id = v.id;
            """;
        
        var categoryIdsParameter = new NpgsqlParameter<long[]>("CategoryIds", NpgsqlDbType.Bigint | NpgsqlDbType.Array);
        var namesParameter = new NpgsqlParameter<string[]>("Names", NpgsqlDbType.Text | NpgsqlDbType.Array);

        var categoriesDbos = tournament.Categories.Select(x => x.ToDbo()).ToArray();
        
        categoryIdsParameter.TypedValue = categoriesDbos.Select(x => x.Id).ToArray();
        namesParameter.TypedValue = categoriesDbos.Select(x => x.Name).ToArray();

        var command = connection.CreateCommand();
        command.CommandText = sqlCommandUpdateCategories;
        command.Parameters.Add(categoryIdsParameter);
        command.Parameters.Add(namesParameter);

        await command.ExecuteNonQueryAsync(cancellationToken);

        const string sqlCommandUpdateCouples =
            """
            INSERT INTO couples (
                id,
                tournament_id,
                first_participant_full_name,
                second_participant_full_name,
                dance_organization_name,
                first_trainer_full_name,
                second_trainer_full_name
            )
            SELECT * FROM unnest(
                @CoupleIds,
                @TournamentIds,                 
                @FirstParticipantFullName,   
                @SecondParticipantFullName,  
                @DanceOrganizationName,      
                @FirstTrainerFullName,       
                @SecondTrainerFullName       
            ) AS v(id, tournament_id, first_participant_full_name, second_participant_full_name, dance_organization_name, first_trainer_full_name, second_trainer_full_name)
            ON CONFLICT (id) DO UPDATE SET
                tournament_id = EXCLUDED.tournament_id,
                first_participant_full_name = EXCLUDED.first_participant_full_name,
                second_participant_full_name = EXCLUDED.second_participant_full_name,
                dance_organization_name = EXCLUDED.dance_organization_name,
                first_trainer_full_name = EXCLUDED.first_trainer_full_name,
                second_trainer_full_name = EXCLUDED.second_trainer_full_name;
            """;
        
        var coupleIdsParameter = new NpgsqlParameter<long[]>("CoupleIds", NpgsqlDbType.Bigint | NpgsqlDbType.Array);
        var tournamentIdsParameter = new NpgsqlParameter<long[]>("TournamentIds", NpgsqlDbType.Bigint | NpgsqlDbType.Array);
        var firstParticipantsFullNamesParameter = new NpgsqlParameter<string[]>("FirstParticipantFullName", NpgsqlDbType.Text | NpgsqlDbType.Array);
        var secondParticipantsFullNamesParameter = new NpgsqlParameter<string?[]>("SecondParticipantFullName", NpgsqlDbType.Text | NpgsqlDbType.Array);
        var danceOrganizationsNamesParameter = new NpgsqlParameter<string?[]>("DanceOrganizationName", NpgsqlDbType.Text | NpgsqlDbType.Array);
        var firstTrainersFullNamesParameter = new NpgsqlParameter<string?[]>("FirstTrainerFullName", NpgsqlDbType.Text | NpgsqlDbType.Array);
        var secondTrainersFullNamesParameter = new NpgsqlParameter<string?[]>("SecondTrainerFullName", NpgsqlDbType.Text | NpgsqlDbType.Array);

        var couplesDbos = tournament.Couples.Select(x => x.ToDbo()).ToArray();
        
        coupleIdsParameter.TypedValue = couplesDbos.Select(x => x.Id).ToArray();
        tournamentIdsParameter.TypedValue = couplesDbos.Select(x => x.TournamentId).ToArray();
        firstParticipantsFullNamesParameter.TypedValue = couplesDbos.Select(x => x.FirstParticipantFullName).ToArray();
        secondParticipantsFullNamesParameter.TypedValue = couplesDbos.Select(x => x.SecondParticipantFullName).ToArray();
        danceOrganizationsNamesParameter.TypedValue = couplesDbos.Select(x => x.DanceOrganizationName).ToArray();
        firstTrainersFullNamesParameter.TypedValue = couplesDbos.Select(x => x.FirstTrainerFullName).ToArray();
        secondTrainersFullNamesParameter.TypedValue = couplesDbos.Select(x => x.SecondTrainerFullName).ToArray();

        command = connection.CreateCommand();
        command.CommandText = sqlCommandUpdateCouples;
        command.Parameters.Add(coupleIdsParameter);
        command.Parameters.Add(tournamentIdsParameter);
        command.Parameters.Add(firstParticipantsFullNamesParameter);
        command.Parameters.Add(secondParticipantsFullNamesParameter);
        command.Parameters.Add(danceOrganizationsNamesParameter);
        command.Parameters.Add(firstTrainersFullNamesParameter);
        command.Parameters.Add(secondTrainersFullNamesParameter);

        await command.ExecuteNonQueryAsync(cancellationToken);
        
        const string sqlCommandDeleteCategoriesCouples =
            """
            DELETE FROM "categories_couples"
            WHERE "category_id" = ANY(@CategoryIds);
            """;

        var categoryIdsToDeleteParameter = new NpgsqlParameter<long[]>("CategoryIds", NpgsqlDbType.Bigint | NpgsqlDbType.Array);
        
        categoryIdsToDeleteParameter.TypedValue = tournament.Categories.Select(x => x.Id.Value).ToArray();

        var deleteCommand = connection.CreateCommand();
        deleteCommand.CommandText = sqlCommandDeleteCategoriesCouples;
        deleteCommand.Parameters.Add(categoryIdsToDeleteParameter);

        await deleteCommand.ExecuteNonQueryAsync(cancellationToken);
        
        const string sqlCommandSetCategoriesCouples =
            """
            INSERT INTO "categories_couples" ("category_id", "couple_id")
            SELECT * FROM unnest(@CategoryIds, @CoupleIds);
            """;
        
        categoryIdsParameter = new NpgsqlParameter<long[]>("CategoryIds", NpgsqlDbType.Bigint | NpgsqlDbType.Array);
        coupleIdsParameter = new NpgsqlParameter<long[]>("CoupleIds", NpgsqlDbType.Bigint | NpgsqlDbType.Array);
        
        var categoriesCouples = tournament.Categories
            .SelectMany(category => category.CouplesIds.Select(coupleId => new { Id = category.Id, coupleId }))
            .ToArray();

        categoryIdsParameter.TypedValue = categoriesCouples.Select(x => x.Id.Value).ToArray();
        coupleIdsParameter.TypedValue = categoriesCouples.Select(x => x.coupleId.Value).ToArray();

        var insertCommand = connection.CreateCommand();
        insertCommand.CommandText = sqlCommandSetCategoriesCouples;
        insertCommand.Parameters.Add(categoryIdsParameter);
        insertCommand.Parameters.Add(coupleIdsParameter);

        await insertCommand.ExecuteNonQueryAsync(cancellationToken);

        const string sqlCommandDeleteCategoryReferee =
            """
            DELETE FROM "categories_referees"
            WHERE "category_id" = ANY(@CategoryIds);
            """;
        
        categoryIdsToDeleteParameter = new NpgsqlParameter<long[]>("CategoryIds", NpgsqlDbType.Bigint | NpgsqlDbType.Array);
        
        categoryIdsToDeleteParameter.TypedValue = tournament.Categories.Select(x => x.Id.Value).ToArray();
        
        deleteCommand = connection.CreateCommand();
        deleteCommand.CommandText = sqlCommandDeleteCategoryReferee;
        deleteCommand.Parameters.Add(categoryIdsToDeleteParameter);
        
        const string sqlCommandSetCategoryReferee =
            """
            INSERT INTO "categories_referees" ("category_id", "referee_id")
            SELECT * FROM unnest(@CategoryIds, @RefereeIds);
            """;
        
        categoryIdsParameter = new NpgsqlParameter<long[]>("CategoryIds", NpgsqlDbType.Bigint | NpgsqlDbType.Array);
        var refereeIdsParameter = new NpgsqlParameter<long[]>("RefereeIds", NpgsqlDbType.Bigint | NpgsqlDbType.Array);
        
        var categoriesReferees = tournament.Categories
            .SelectMany(category => category.RefereesIds.Select(refereeId => new { Id = category.Id, refereeId }))
            .ToArray();

        categoryIdsParameter.TypedValue = categoriesReferees.Select(x => x.Id.Value).ToArray(); 
        refereeIdsParameter.TypedValue = categoriesReferees.Select(x => x.refereeId.Value).ToArray();

        insertCommand = connection.CreateCommand();
        insertCommand.CommandText = sqlCommandSetCategoryReferee;
        insertCommand.Parameters.Add(categoryIdsParameter);
        insertCommand.Parameters.Add(refereeIdsParameter);

        await insertCommand.ExecuteNonQueryAsync(cancellationToken);
        
        const string sqlCommandDeleteCategoryDance =
            """
            DELETE FROM "categories_dances"
            WHERE "category_id" = ANY(@CategoryIds);
            """;
        
        categoryIdsToDeleteParameter = new NpgsqlParameter<long[]>("CategoryIds", NpgsqlDbType.Bigint | NpgsqlDbType.Array);
        
        categoryIdsToDeleteParameter.TypedValue = tournament.Categories.Select(x => x.Id.Value).ToArray();
        
        deleteCommand = connection.CreateCommand();
        deleteCommand.CommandText = sqlCommandDeleteCategoryDance;
        deleteCommand.Parameters.Add(categoryIdsToDeleteParameter);
        
        const string sqlCommandSetCategoryDance =
            """
            INSERT INTO "categories_dances" ("category_id", "dance_id")
            SELECT * FROM unnest(@CategoryIds, @DanceIds);
            """;
        
        categoryIdsParameter = new NpgsqlParameter<long[]>("CategoryIds", NpgsqlDbType.Bigint | NpgsqlDbType.Array);
        var danceIdsParameter = new NpgsqlParameter<long[]>("DanceIds", NpgsqlDbType.Bigint | NpgsqlDbType.Array);
        
        var categoriesDances = tournament.Categories
            .SelectMany(category => category.DancesIds.Select(danceId => new { Id = category.Id, danceId }))
            .ToArray();

        categoryIdsParameter.TypedValue = categoriesDances.Select(x => x.Id.Value).ToArray(); 
        danceIdsParameter.TypedValue = categoriesDances.Select(x => x.danceId.Value).ToArray();

        insertCommand = connection.CreateCommand();
        insertCommand.CommandText = sqlCommandSetCategoryDance;
        insertCommand.Parameters.Add(categoryIdsParameter);
        insertCommand.Parameters.Add(danceIdsParameter);

        await insertCommand.ExecuteNonQueryAsync(cancellationToken);
        
        await transaction.CommitAsync(cancellationToken);
    }

    public async Task<Tournament?> FindByIdAsync(TournamentId tournamentId, CancellationToken cancellationToken)
    {
        await using var connection = await _connectionFactory.CreateAsync();
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);

        const string sqlCommandGetTournament =
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

        var tournamentDbo = await connection.QuerySingleOrDefaultAsync<TournamentDbo>(sqlCommandGetTournament, new { id = tournamentId.Value });
        if (tournamentDbo == null)
            return null;
        
        const string sqlCommandGetCategories =
            """
            select 
                "id",
                "tournament_id",
                "name"
            from "categories"
            where "tournament_id" = @TournamentId;
            """;
        
        var categories = await connection.QueryAsync<CategoryDbo>(sqlCommandGetCategories, new { TournamentId = tournamentId.Value });

        const string sqlCommandGetCouples =
            """
            select
                "id",
                "tournament_id",
                "first_participant_full_name",
                "second_participant_full_name",
                "dance_organization_name",
                "first_trainer_full_name",
                "second_trainer_full_name"
            from "couples"
            where "tournament_id" = @TournamentId;
            """;
        
        var couples = await connection.QueryAsync<CoupleDbo>(sqlCommandGetCouples, new { TournamentId = tournamentId.Value });

        const string sqlCommandGetCoupleIdsForCategories =
            """
            select
                "category_id",
                "couple_id"
            from "categories_couples"
            where "category_id" = ANY(@CategoryIds);
            """;
    
        var coupleIdsForCategories = await connection.QueryAsync<(long CategoryId, long CoupleId)>(sqlCommandGetCoupleIdsForCategories, new { CategoryIds = categories.Select(c => c.Id).ToArray() });

        var coupleIdsGroupedByCategory = coupleIdsForCategories
            .GroupBy(x => x.CategoryId)
            .ToDictionary(g => g.Key, g => g.Select(x => CoupleId.From(x.CoupleId).AsRequired()).ToList());

        const string sqlCommandGetReferencesIdsForCategories =
            """
            select 
                "referee_id",
                "category_id"
            from "categories_referees"
            where "category_id" = ANY(@CategoryIds);
            """;
        
        var refereeIdsForCategories = await connection.QueryAsync<(long CategoryId, long RefereeId)>(sqlCommandGetReferencesIdsForCategories, new { CategoryIds = categories.Select(c => c.Id).ToArray() });

        var refereeIdsGroupedByCategory = refereeIdsForCategories
            .GroupBy(x => x.CategoryId)
            .ToDictionary(g => g.Key, g => g.Select(x => RefereeId.From(x.RefereeId).AsRequired()).ToList());

        const string sqlCommandGetDancesIdsForCategories =
            """
            select
                "dance_id",
                "category_id"
            from "categories_dances"
            where "category_id" = ANY(@CategoryIds);
            """;
        
        var danceIdsForCategories = await connection.QueryAsync<(long CategoryId, long DanceId)>(sqlCommandGetDancesIdsForCategories, new { CategoryIds = categories.Select(c => c.Id).ToArray() });
        
        var danceIdsGroupedByCategory = danceIdsForCategories
            .GroupBy(x => x.CategoryId)
            .ToDictionary(g => g.Key, g => g.Select(x => DanceId.From(x.DanceId).AsRequired()).ToList());
        
        await transaction.CommitAsync(cancellationToken);

        return tournamentDbo?.ToDomain(categories, couples, danceIdsGroupedByCategory, refereeIdsGroupedByCategory, coupleIdsGroupedByCategory);
    }
}