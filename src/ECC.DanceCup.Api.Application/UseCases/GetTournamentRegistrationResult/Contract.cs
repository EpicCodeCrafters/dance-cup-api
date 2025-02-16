using ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel.Views;
using ECC.DanceCup.Api.Application.UseCases.GetDances;
using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using FluentResults;
using MediatR;

namespace ECC.DanceCup.Api.Application.UseCases.GetTournamentRegistrationResult;

public static partial class GetTournamentRegistrationResultUseCase
{
    public record Query(TournamentId TournamentId) : IRequest<Result<QueryResponse>>;
    
    public record QueryResponse(IReadOnlyCollection<TournamentRegistrationResultView> Couples);
}