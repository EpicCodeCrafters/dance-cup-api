using AutoFixture;
using ECC.DanceCup.Api.Domain.Core;
using ECC.DanceCup.Api.Domain.Model.DanceAggregate;
using ECC.DanceCup.Api.Domain.Model.RefereeAggregate;
using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using ECC.DanceCup.Api.Domain.Model.UserAggregate;
using ECC.DanceCup.Api.Utils.Extensions;

namespace ECC.DanceCup.Api.Tests.Common.Customizations;

public class DomainCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<AggregateVersion>(composer =>
            composer.FromFactory(() => AggregateVersion.From(fixture.Create<int>()).AsRequired())
        );
        
        fixture.Customize<UserId>(composer =>
            composer.FromFactory(() => UserId.From(fixture.Create<long>()).AsRequired())
        );
        
        fixture.Customize<DanceId>(composer =>
            composer.FromFactory(() => DanceId.From(fixture.Create<long>()).AsRequired())
        );
                
        fixture.Customize<RefereeId>(composer =>
            composer.FromFactory(() => RefereeId.From(fixture.Create<long>()).AsRequired())
        );
        
        fixture.Customize<RefereeFullName>(composer =>
            composer.FromFactory(() => RefereeFullName.From(fixture.Create<string>()).AsRequired())
        );
        
        fixture.Customize<TournamentId>(composer =>
            composer.FromFactory(() => TournamentId.From(fixture.Create<long>()).AsRequired())
        );
        
        fixture.Customize<TournamentName>(composer =>
            composer.FromFactory(() => TournamentName.From(fixture.Create<string>()).AsRequired())
        );
        
        fixture.Customize<TournamentDate>(composer =>
            composer.FromFactory(() => TournamentDate.From(fixture.Create<DateTime>()).AsRequired())
        );
        
        fixture.Customize<CategoryId>(composer =>
            composer.FromFactory(() => CategoryId.From(fixture.Create<long>()).AsRequired())
        );
        
        fixture.Customize<TournamentDescription>(composer =>
            composer.FromFactory(() => TournamentDescription.From(fixture.Create<string>()).AsRequired())
        );
    }
}