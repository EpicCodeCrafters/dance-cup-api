using ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel.Views;
using ECC.DanceCup.Api.Application.UseCases.CreateTournament;
using ECC.DanceCup.Api.Application.UseCases.GetDances;
using ECC.DanceCup.Api.Domain.Model;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Linq;

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
            UserId: UserId.From(request.UserId)!.Value,
            Name: TournamentName.From(request.Name)!.Value,
            Date: TournamentDate.From(request.Date.ToDateTime())!.Value,
            CreateCategoryModels: request.CreateCategoryModels.Select(ToInternal).ToArray()
            );
    }

    private static ECC.DanceCup.Api.Domain.Services.CreateCategoryModel ToInternal(this CreateCategoryModel createCategoryModel)
    {
        return new Domain.Services.CreateCategoryModel(
          CategoryName: CategoryName.From(createCategoryModel.CategoryName)!.Value,
          DancesIds: createCategoryModel.DancesIds.Select(danceId=>DanceId.From(danceId)!.Value).ToArray(),
          RefereesIds: createCategoryModel.RefereesIds.Select(refereeId => RefereeId.From(refereeId)!.Value).ToArray()
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