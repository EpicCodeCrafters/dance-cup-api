using FluentAssertions;
using FluentResults;

namespace ECC.DanceCup.Api.Tests.Common.Extensions;

public static class ResultExtensions
{
    public static void ShouldBeSuccess(this ResultBase result)
    {
        result.IsSuccess.Should().BeTrue();
    }
    
    public static void ShouldBeFail(this ResultBase result)
    {
        result.IsSuccess.Should().BeFalse();
    }

    public static void ShouldBeFailWith<TError>(this ResultBase result)
    {
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainItemsAssignableTo<TError>();
    }
}