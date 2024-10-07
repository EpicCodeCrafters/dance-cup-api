using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using ECC.DanceCup.Api.Tests.Common.Customizations;

namespace ECC.DanceCup.Api.Tests.Common.Attributes;

public class AutoMoqDataAttribute : AutoDataAttribute
{
    public AutoMoqDataAttribute()
        : base(() => new Fixture().Customize(new AutoMoqCustomization()).Customize(new DomainCustomization()))
    {
    }
}