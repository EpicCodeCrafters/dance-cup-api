using ECC.DanceCup.Api.Domain.Core;
using ECC.DanceCup.Api.Domain.Model.UserAggregate;
using ECC.DanceCup.Api.Domain.Services;
using ECC.DanceCup.Api.Tests.Common.Attributes;
using ECC.DanceCup.Api.Tests.Common.Extensions;
using FluentAssertions;

namespace ECC.DanceCup.Api.Domain.Tests.Services;

public class UserFactoryTests
{
    [Theory, AutoMoqData]
    public void Create_ShouldGenerallySuccess(
        UserExternalId externalId,
        Username username,
        UserFactory userFactory)
    {
        // Arrange

        var testStartedAt = DateTime.UtcNow;

        // Act

        var result = userFactory.Create(externalId, username);

        // Assert

        result.ShouldBeSuccess();
        result.Value.Id.Should().Be(UserId.Empty);
        result.Value.Version.Should().Be(AggregateVersion.Default);
        result.Value.ExternalId.Should().Be(externalId);
        result.Value.Username.Should().Be(username);
        result.Value.CreatedAt.Should().BeAfter(testStartedAt).And.BeBefore(DateTime.UtcNow);
        result.Value.ChangedAt.Should().Be(result.Value.CreatedAt);
    }
}
