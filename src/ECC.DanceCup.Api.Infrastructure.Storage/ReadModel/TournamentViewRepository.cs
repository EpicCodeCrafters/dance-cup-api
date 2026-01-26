using System.Text.Json;
using Dapper;
using ECC.DanceCup.Api.Application.Abstractions.Models.Views;
using ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel;
using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using ECC.DanceCup.Api.Domain.Model.UserAggregate;
using ECC.DanceCup.Api.Infrastructure.Storage.Tools;

namespace ECC.DanceCup.Api.Infrastructure.Storage.ReadModel;

public class TournamentViewRepository : ITournamentViewRepository
{
    private readonly IPostgresConnectionFactory _connectionFactory;

    public TournamentViewRepository(IPostgresConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyCollection<TournamentView>> FindAllAsync(UserId userId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        await using var connection = await _connectionFactory.CreateAsync();

        const string sqlCommand =
            """
            select "id"
                 , "user_id"
                 , "name"
                 , "description"
                 , "date"
                 , "state"
              from "tournaments"
             where "user_id" = @UserId
             order by "id" desc
             limit @Limit
            offset @Offset;
            """;
        
        var result = await connection.QueryAsync<TournamentView>(
            sqlCommand, 
            new
            {
                UserId = userId.Value,
                Limit = pageSize,
                Offset = (pageNumber - 1) * pageSize
            });

        return result.ToArray();
    }

    public async Task<IReadOnlyCollection<TournamentRegistrationResultView>> GetRegistrationResultAsync(TournamentId tournamentId, CancellationToken cancellationToken)
    {
        await using var connection = await _connectionFactory.CreateAsync();

        const string sqlCommand =
            """
            select 
                c.name as category_name,
                cp.first_participant_full_name,
                cp.second_participant_full_name,
                cp.dance_organization_name,
                cp.first_trainer_full_name,
                cp.second_trainer_full_name
            from 
                categories c
            join 
                categories_couples cc on c.id = cc.category_id
            join 
                couples cp on cc.couple_id = cp.id
            where 
                c.tournament_id = @TournamentId
            order by 
                c.id,
                cp.id;
            """;
        
        var resultOfRegistration = await connection.QueryAsync<TournamentRegistrationResultView>(sqlCommand, new { TournamentId = tournamentId.Value });

        return resultOfRegistration.ToArray();
    }

    public async Task<string?> GetTournamentAttachmentNameAsync(TournamentId tournamentId, int attachmentNumber, CancellationToken cancellationToken)
    {
        await using var connection = await _connectionFactory.CreateAsync();

        const string sqlCommand =
            """
            select t."attachments"
            from "tournaments" as t
            where t."id" = @TournamentId;
            """;
        
        var attachmentsSerialized = await connection.QueryFirstOrDefaultAsync<string>(sqlCommand, new { TournamentId = tournamentId.Value });
        if (attachmentsSerialized is null)
        {
            return null;
        }
        
        var attachments = JsonSerializer.Deserialize<TournamentAttachment[]>(attachmentsSerialized);

        return attachments?.FirstOrDefault(x => x.Number == attachmentNumber)?.Name;
    }
}