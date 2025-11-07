using AutoFixture.Xunit2;
using ECC.DanceCup.Api.Application.Abstractions.Storage.DomainModel;
using ECC.DanceCup.Api.Application.UseCases.CreateUser;
using ECC.DanceCup.Api.Domain.Model.UserAggregate;
using ECC.DanceCup.Api.Domain.Services;
using ECC.DanceCup.Api.Tests.Common.Attributes;
using ECC.DanceCup.Api.Tests.Common.Errors;
using ECC.DanceCup.Api.Tests.Common.Extensions;
using FluentAssertions;
using Moq;

namespace ECC.DanceCup.Api.Application.Tests.UseCases;

public class CreateUserTests
{
    [Theory, AutoMoqData]
    public async Task Handle_ShouldGenerallySuccess(
        UserExternalId externalId,
        Username username,
        User user,
        UserId userId,
        [Frozen] Mock<IUserFactory> userFactoryMock,
        [Frozen] Mock<IUserRepository> userRepositoryMock,
        CreateUserUseCase.CommandHandler handler)
    {
        // Arrange

        userFactoryMock
            .Setup(userFactory => userFactory.Create(externalId, username))
            .Returns(user);

        userRepositoryMock
            .Setup(userRepository => userRepository.InsertAsync(user, It.IsAny<CancellationToken>()))
            .ReturnsAsync(userId);

        var command = new CreateUserUseCase.Command(externalId, username);

        // Act

        var result = await handler.Handle(command, CancellationToken.None);

        // Assert

        result.ShouldBeSuccess();
        result.Value.UserId.Should().Be(userId);

        userFactoryMock.Verify(
            userFactory => userFactory.Create(externalId, username),
            Times.Once
        );
        userFactoryMock.VerifyNoOtherCalls();

        userRepositoryMock.Verify(
            userRepository => userRepository.InsertAsync(user, It.IsAny<CancellationToken>()),
            Times.Once
        );
        userRepositoryMock.VerifyNoOtherCalls();
    }

    [Theory, AutoMoqData]
    public async Task Handle_UserFactoryCreateFailed_ShouldFail(
        UserExternalId externalId,
        Username username,
        [Frozen] Mock<IUserFactory> userFactoryMock,
        [Frozen] Mock<IUserRepository> userRepositoryMock,
        CreateUserUseCase.CommandHandler handler)
    {
        // Arrange

        userFactoryMock
            .Setup(userFactory => userFactory.Create(externalId, username))
            .Returns(new TestError());

        var command = new CreateUserUseCase.Command(externalId, username);

        // Act

        var result = await handler.Handle(command, CancellationToken.None);

        // Assert

        result.ShouldBeFailWith<TestError>();

        userFactoryMock.Verify(
            userFactory => userFactory.Create(externalId, username),
            Times.Once
        );
        userFactoryMock.VerifyNoOtherCalls();

        userRepositoryMock.VerifyNoOtherCalls();
    }
}
