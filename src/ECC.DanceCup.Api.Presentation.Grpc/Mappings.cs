﻿using ECC.DanceCup.Api.Application.Abstractions.Models.Views;
using ECC.DanceCup.Api.Application.UseCases.CreateReferee;
using ECC.DanceCup.Api.Application.UseCases.CreateTournament;
using ECC.DanceCup.Api.Application.UseCases.GetDances;
using ECC.DanceCup.Api.Application.UseCases.StartTournamentRegistration;
using ECC.DanceCup.Api.Application.UseCases.FinishTournamentRegistration;
using ECC.DanceCup.Api.Application.UseCases.RegisterCoupleForTournament;
using ECC.DanceCup.Api.Application.UseCases.ReopenTournamentRegistration;
using ECC.DanceCup.Api.Application.UseCases.GetReferees;
using ECC.DanceCup.Api.Application.UseCases.GetTournamentRegistrationResult;
using ECC.DanceCup.Api.Application.UseCases.GetTournaments;
using ECC.DanceCup.Api.Domain.Model.DanceAggregate;
using ECC.DanceCup.Api.Domain.Model.RefereeAggregate;
using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using ECC.DanceCup.Api.Domain.Model.UserAggregate;
using ECC.DanceCup.Api.Utils.Extensions;
using Google.Protobuf.WellKnownTypes;

namespace ECC.DanceCup.Api.Presentation.Grpc;

internal static class Mappings
{
    public static GetDancesUseCase.Query ToInternal(this GetDancesRequest request)
    {
        return new GetDancesUseCase.Query();
    }

    public static GetRefereesUseCase.Query ToInternal(this GetRefereesRequest request)
    {
        return new GetRefereesUseCase.Query(request.FullName is null ? null : RefereeFullName.From(request.FullName).AsRequired(), request.PageNumber, request.PageSize);
    }

    public static GetDancesResponse ToGrpc(this GetDancesUseCase.QueryResponse response)
    {
        return new GetDancesResponse
        {
            Dances = { response.Dances.Select(ToGrpc) }
        };
    }

    public static GetRefereesResponse ToGrpc(this GetRefereesUseCase.QueryResponse response)
    {
        return new GetRefereesResponse
        {
            Referees = { response.Referees.Select(ToGrpc) }
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

    private static Referee ToGrpc(this RefereeView referee)
    {
        return new Referee
        {
            Id = referee.Id,
            FullName = referee.FullName
        };
    }

    private static GetTournamentRegistrationResultResponse.Types.TournamentRegistrationResultItem ToGrpc(this TournamentRegistrationResultView couple)
    {
        return new GetTournamentRegistrationResultResponse.Types.TournamentRegistrationResultItem
        {
            CategoryName = couple.CategoryName,
            FirstParticipantFullName = couple.FirstParticipantFullName,
            SecondParticipantFullName = couple.SecondParticipantFullName,
            DanceOrganizationName = couple.DanceOrganizationName,
            FirstTrainerFullName = couple.FirstTrainerFullName,
            SecondTrainerFullName = couple.SecondTrainerFullName
        };
    }
    
    public static CreateRefereeUseCase.Command ToInternal(this CreateRefereeRequest request)
    {
        return new CreateRefereeUseCase.Command(
            FullName: RefereeFullName.From(request.FullName).AsRequired()
        );
    }

    public static CreateRefereeResponse ToGrpc(this CreateRefereeUseCase.CommandResponse response)
    {
        return new CreateRefereeResponse
        {
            RefereeId = response.RefereeId.Value
        };
    }

    public static CreateTournamentUseCase.Command ToInternal(this CreateTournamentRequest request)
    {
        return new CreateTournamentUseCase.Command(
            UserId: UserId.From(request.UserId).AsRequired(),
            Name: TournamentName.From(request.Name).AsRequired(),
            Description: TournamentDescription.From(request.Description).AsRequired(),
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

    public static GetTournamentsUseCase.Query ToInternal(this GetTournamentsRequest request)
    {
        return new GetTournamentsUseCase.Query(
            UserId: UserId.From(request.UserId).AsRequired(),
            PageSize: request.PageSize,
            PageNumber: request.PageNumber
        );
    }

    public static GetTournamentsResponse ToGrpc(this GetTournamentsUseCase.QueryResponse response)
    {
        return new GetTournamentsResponse
        {
            Tournaments = { response.Tournaments.Select(ToGrpc) }
        };
    }

    private static GetTournamentsResponse.Types.GetTournamentsItem ToGrpc(this TournamentView tournamentView)
    {
        return new GetTournamentsResponse.Types.GetTournamentsItem
        {
            Id = tournamentView.Id,
            UserId = tournamentView.UserId,
            Name = tournamentView.Name,
            Description = tournamentView.Description,
            Date = DateTime.SpecifyKind(tournamentView.Date, DateTimeKind.Utc).ToTimestamp(),
            State = tournamentView.State
        };
    }

    public static GetTournamentRegistrationResultUseCase.Query ToInternal(this GetTournamentRegistrationResultRequest request)
    {
        return new GetTournamentRegistrationResultUseCase.Query(
            TournamentId: TournamentId.From(request.TournamentId).AsRequired()
        );
    }

    public static GetTournamentRegistrationResultResponse ToGrpc(this GetTournamentRegistrationResultUseCase.QueryResponse response)
    {
        return new GetTournamentRegistrationResultResponse
        {
            Items = {  response.ResultOfRegistration.Select(ToGrpc) }
        };
    }

    public static StartTournamentRegistrationUseCase.Command ToInternal(this StartTournamentRegistrationRequest request)
    {
        return new StartTournamentRegistrationUseCase.Command(
            TournamentId: TournamentId.From(request.TournamentId).AsRequired()
        );
    }

    public static FinishTournamentRegistrationUseCase.Command ToInternal(this FinishTournamentRegistrationRequest request)
    {
        return new FinishTournamentRegistrationUseCase.Command(
            TournamentId: TournamentId.From(request.TournamentId).AsRequired()
        );
    }

    public static ReopenTournamentRegistrationUseCase.Command ToInternal(this ReopenTournamentRegistrationRequest request)
    {
        return new ReopenTournamentRegistrationUseCase.Command(
            TournamentId: TournamentId.From(request.TournamentId).AsRequired()
        );
    }

    public static RegisterCoupleForTournamentUseCase.Command ToInternal(this RegisterCoupleForTournamentRequest request)
    {
        return new RegisterCoupleForTournamentUseCase.Command(
            TournamentId: TournamentId.From(request.TournamentId).AsRequired(),
            FirstParticipantFullName: CoupleParticipantFullName.From(request.FirstParticipantFullName).AsRequired(),
            SecondParticipantFullName: request.SecondParticipantFullName is null ? null : CoupleParticipantFullName.From(request.SecondParticipantFullName).AsRequired(),
            DanceOrganizationName: request.DanceOrganizationName is null ? null : CoupleDanceOrganizationName.From(request.DanceOrganizationName).AsRequired(),
            FirstTrainerFullName: request.FirstTrainerFullName is null ? null : CoupleTrainerFullName.From(request.FirstTrainerFullName).AsRequired(),
            SecondTrainerFullName: request.SecondTrainerFullName is null ? null : CoupleTrainerFullName.From(request.SecondTrainerFullName).AsRequired(),
            CategoriesIds: request.CategoriesIds.Select(categoryId => CategoryId.From(categoryId).AsRequired()).ToArray()
        );
    }
    
    
}