﻿using ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel.Views;
using ECC.DanceCup.Api.Application.UseCases.GetDances;

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
}