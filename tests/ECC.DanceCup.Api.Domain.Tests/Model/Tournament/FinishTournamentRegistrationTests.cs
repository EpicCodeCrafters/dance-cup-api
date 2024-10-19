using AutoFixture;
using ECC.DanceCup.Api.Domain.Error;
using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using ECC.DanceCup.Api.Tests.Common.Attributes;
using ECC.DanceCup.Api.Tests.Common.Extensions;

namespace ECC.DanceCup.Api.Domain.Tests.Model.Tournament;

public class FinishRegistrationTests
{
    [Theory]
    [InlineAutoMoqData(TournamentState.RegistrationInProgress)]
    public void Invoke_CorrectTournamentState_ShouldSuccess(
        TournamentState tournamentState,
        IFixture fixture)
    {
        //Arrange

        var tournament = fixture.CreateTournament(
            state: tournamentState
            );

        //Act

        var result = tournament.FinishRegistration();

        //Assert

        result.ShouldBeSuccess();
    }

    [Theory]
    [InlineAutoMoqData(TournamentState.Created)]
    [InlineAutoMoqData(TournamentState.RegistrationFinished)]
    [InlineAutoMoqData(TournamentState.Created)]
    [InlineAutoMoqData(TournamentState.Finished)]
    public void Invoke_CorrectTournamentState_ShoudFail(
        TournamentState tournamentState,
        IFixture fixture)
    {
        //Arrange

        var tournament = fixture.CreateTournament(
            state: tournamentState
            );

        //Act

        var result = tournament.FinishRegistration();

        //Assert

        result.ShouldBeFailWith<TournamentShouldBeInStatusError>();
    }
}