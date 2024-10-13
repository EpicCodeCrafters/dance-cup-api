using ECC.DanceCup.Api.Domain.Model.RefereeAggregate;
using ECC.DanceCup.Api.Domain.Services;
using ECC.DanceCup.Api.Tests.Common.Attributes;
using ECC.DanceCup.Api.Tests.Common.Extensions;

namespace ECC.DanceCup.Api.Domain.Tests.Services;

public class RefereeFactoryTests
{
    [Theory, AutoMoqData]
    public void Create_ShouldGenerallySuccess(
        RefereeFullName fullName,
        RefereeFactory refereeFactory)
    {
        // Arrange

        // Act

        var result = refereeFactory.Create(fullName);

        // Assert

        result.ShouldBeSuccess();
    }
}