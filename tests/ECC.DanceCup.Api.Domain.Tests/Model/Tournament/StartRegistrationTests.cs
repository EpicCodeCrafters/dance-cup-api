using AutoFixture;
using ECC.DanceCup.Api.Domain.Error;
using ECC.DanceCup.Api.Domain.Model;
using ECC.DanceCup.Api.Tests.Common.Attributes;
using ECC.DanceCup.Api.Tests.Common.Extensions;
using FluentAssertions;

namespace ECC.DanceCup.Api.Domain.Tests.Model.Tournament;

public class StartRegistrationTests
{
    [Theory]
    [InlineAutoMoqData(TournamentState.Created)]
    public void StartRegistration_CorrectTournamentState_ShouldSuccess(
        TournamentState tournamentState,
        IFixture fixture)
    {
        // Arrange

        var tournament = fixture.CreateTournament(
            state: tournamentState
        );

        // Act

        var result = tournament.StartRegistration();

        // Assert

        result.ShouldBeSuccess();
    }
    
    [Theory]
    [InlineAutoMoqData(TournamentState.RegistrationInProgress)]
    [InlineAutoMoqData(TournamentState.RegistrationFinished)]
    [InlineAutoMoqData(TournamentState.InProgress)]
    [InlineAutoMoqData(TournamentState.Finished)]
    public void StartRegistration_IncorrectTournamentState_ShouldFail(
        TournamentState tournamentState,
        IFixture fixture)
    {
        // Arrange

        var tournament = fixture.CreateTournament(
            state: tournamentState
        );

        // Act

        var result = tournament.StartRegistration();

        // Assert

        result.ShouldBeFailWith<TournamentShouldBeInStatusError>();
    }
}