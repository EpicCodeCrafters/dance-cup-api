using AutoFixture;
using ECC.DanceCup.Api.Domain.Model;
using ECC.DanceCup.Api.Tests.Common.Extensions;
using ECC.DanceCup.Api.Utils.Extensions;

namespace ECC.DanceCup.Api.Tests.Common.Customizations;

public class DomainCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<Category>(composer =>
            composer.FromFactory(() => fixture.CreateCategory())
        );
        
        fixture.Customize<CategoryId>(composer =>
            composer.FromFactory(() => CategoryId.From(fixture.Create<long>()).AsRequired())
        );
        
        fixture.Customize<CategoryName>(composer =>
            composer.FromFactory(() => CategoryName.From(fixture.Create<string>()).AsRequired())
        );
        
        fixture.Customize<DanceId>(composer =>
            composer.FromFactory(() => DanceId.From(fixture.Create<long>()).AsRequired())
        );
        
        fixture.Customize<RefereeId>(composer =>
            composer.FromFactory(() => RefereeId.From(fixture.Create<long>()).AsRequired())
        );
        
        fixture.Customize<CategoryId>(composer =>
            composer.FromFactory(() => CategoryId.From(fixture.Create<long>()).AsRequired())
        );
        
        fixture.Customize<Tournament>(composer =>
            composer.FromFactory(() => fixture.CreateTournament())
        );
        
        fixture.Customize<TournamentDate>(composer =>
            composer.FromFactory(() => TournamentDate.From(fixture.Create<DateTime>()).AsRequired())
        );
        
        fixture.Customize<TournamentId>(composer =>
            composer.FromFactory(() => TournamentId.From(fixture.Create<long>()).AsRequired())
        );
        
        fixture.Customize<TournamentName>(composer =>
            composer.FromFactory(() => TournamentName.From(fixture.Create<string>()).AsRequired())
        );
        
        fixture.Customize<UserId>(composer =>
            composer.FromFactory(() => UserId.From(fixture.Create<long>()).AsRequired())
        );
    }
}