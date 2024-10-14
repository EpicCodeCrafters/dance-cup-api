using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using ECC.DanceCup.Api.Domain.Model.UserAggregate;
using ECC.DanceCup.Api.Domain.Services;
using FluentResults;
using MediatR;

namespace ECC.DanceCup.Api.Application.UseCases.CreateTournament;

public static partial class CreateTournamentUseCase
{
    public record Command(
        UserId UserId,
        TournamentName Name,
        TournamentDescription Description,
        TournamentDate Date,
        IReadOnlyCollection<CreateCategoryModel> CreateCategoryModels
    ) : IRequest<Result<CommandResponse>>;

    public record CommandResponse(TournamentId TournamentId);
}