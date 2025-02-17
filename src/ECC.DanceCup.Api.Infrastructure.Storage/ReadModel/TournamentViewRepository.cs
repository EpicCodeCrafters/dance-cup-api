using Dapper;
using ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel;
using ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel.Views;
using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using ECC.DanceCup.Api.Infrastructure.Storage.Tools;

namespace ECC.DanceCup.Api.Infrastructure.Storage.ReadModel;

public class TournamentViewRepository: ITournamentViewRepository
{
    private readonly IPostgresConnectionFactory _connectionFactory;

    public TournamentViewRepository(IPostgresConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyCollection<TournamentRegistrationResultView>> FindAllAsync(TournamentId tournamentId,CancellationToken cancellationToken)
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
        
        var couples = await connection.QueryAsync<TournamentRegistrationResultView>(sqlCommand, new { TournamentId = tournamentId.Value });

        return couples.ToArray();
    }
}