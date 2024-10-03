using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace ECC.DanceCup.Api.Tests.Common;

public class InlineAutoMoqDataAttribute : InlineAutoDataAttribute
{
    public InlineAutoMoqDataAttribute(params object?[] objects)
        : base(() => new Fixture().Customize(new AutoMoqCustomization()), new AutoMoqDataAttribute(), objects)
    {
    }
}