using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using ECC.DanceCup.Api.Tests.Common.Customizations;

namespace ECC.DanceCup.Api.Tests.Common.Attributes;

public class InlineAutoMoqDataAttribute : InlineAutoDataAttribute
{
    public InlineAutoMoqDataAttribute(params object?[] objects)
        : base(() => new Fixture().Customize(new AutoMoqCustomization()).Customize(new DomainCustomization()), objects)
    {
    }
}