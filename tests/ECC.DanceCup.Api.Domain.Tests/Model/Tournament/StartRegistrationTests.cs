using AutoFixture;
using ECC.DanceCup.Api.Domain.Errors;
using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using ECC.DanceCup.Api.Tests.Common.Attributes;
using ECC.DanceCup.Api.Tests.Common.Extensions;

namespace ECC.DanceCup.Api.Domain.Tests.Model.Tournament;

public class StartRegistrationTests
{
    [Theory]
    [InlineAutoMoqData(TournamentState.Created)]
    public void Invoke_CorrectTournamentState_ShouldSuccess(
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
    public void Invoke_IncorrectTournamentState_ShouldFail(
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