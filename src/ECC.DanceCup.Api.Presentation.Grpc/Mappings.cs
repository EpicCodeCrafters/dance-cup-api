using ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel.Views;
using ECC.DanceCup.Api.Application.UseCases.CreateTournament;
using ECC.DanceCup.Api.Application.UseCases.GetDances;
using ECC.DanceCup.Api.Domain.Model;
using ECC.DanceCup.Api.Utils.Extensions;

namespace ECC.DanceCup.Api.Presentation.Grpc;

internal static class Mappings
{
    public static GetDancesUseCase.Query ToInternal(this GetDancesRequest request)
    {
        return new GetDancesUseCase.Query();
    }

    public static GetDancesResponse ToGrpc(this GetDancesUseCase.QueryResponse response)
    {
        return new GetDancesResponse
        {
            Dances = { response.Dances.Select(ToGrpc) }
        };
    }

    private static Dance ToGrpc(this DanceView dance)
    {
        return new Dance
        {
            Id = dance.Id,
            ShortName = dance.ShortName,
            Name = dance.Name
        };
    }

    public static CreateTournamentUseCase.Command ToInternal(this CreateTournamentRequest request)
    {
        return new CreateTournamentUseCase.Command(
            UserId: UserId.From(request.UserId).AsRequired(),
            Name: TournamentName.From(request.Name).AsRequired(),
            Date: TournamentDate.From(request.Date.ToDateTime()).AsRequired(),
            CreateCategoryModels: request.CreateCategoryModels.Select(ToInternal).ToArray()
        );
    }

    private static Domain.Services.CreateCategoryModel ToInternal(this CreateCategoryModel createCategoryModel)
    {
        return new Domain.Services.CreateCategoryModel(
            Name: CategoryName.From(createCategoryModel.Name).AsRequired(),
            DancesIds: createCategoryModel.DancesIds.Select(danceId => DanceId.From(danceId).AsRequired()).ToArray(),
            RefereesIds: createCategoryModel.RefereesIds.Select(refereeId => RefereeId.From(refereeId).AsRequired()).ToArray()
        );
    }

    public static CreateTournamentResponse ToGrpc(this CreateTournamentUseCase.CommandResponse response)
    {
        return new CreateTournamentResponse
        {
            TournamentId = response.TournamentId.Value
        };
    }
}