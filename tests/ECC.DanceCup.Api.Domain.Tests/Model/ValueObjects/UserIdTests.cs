﻿using ECC.DanceCup.Api.Domain.Model.UserAggregate;
using FluentAssertions;

namespace ECC.DanceCup.Api.Domain.Tests.Model.ValueObjects;


public class UserIdTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(int.MaxValue)]
    [InlineData(long.MaxValue)]
    public void Create_FromValidValue_ShouldNotBeNull(long value)
    {
        // Arrange

        // Act

        var userId = UserId.From(value);

        // Assert

        userId.Should().NotBeNull();
        userId.Value.Value.Should().Be(value);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    [InlineData(long.MinValue)]
    public void Create_FromInvalidValue_ShouldBeNull(long value)
    {
        // Arrange

        // Act

        var userId = UserId.From(value);

        // Assert

        userId.Should().BeNull();
    }
}