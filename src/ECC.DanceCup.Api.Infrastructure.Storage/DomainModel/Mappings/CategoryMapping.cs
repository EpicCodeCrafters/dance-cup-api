using ECC.DanceCup.Api.Domain.Model.DanceAggregate;
using ECC.DanceCup.Api.Domain.Model.RefereeAggregate;
using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using ECC.DanceCup.Api.Infrastructure.Storage.DomainModel.Dbo;
using ECC.DanceCup.Api.Utils.Extensions;

namespace ECC.DanceCup.Api.Infrastructure.Storage.DomainModel.Mappings;

internal static class CategoryMapping
{
    public static CategoryDbo ToDbo(this Category category)
    {
        return new CategoryDbo
        {
            Id = category.Id.Value,
            TournamentId = category.TournamentId.Value,
            Name = category.Name.Value
        };
    }
    
    public static List<Category> ToDomain(this IEnumerable<CategoryDbo> categories)
    {
        return categories.Select(c =>
            new Category(
                id: CategoryId.From(c.Id).AsRequired(),
                tournamentId: TournamentId.From(c.TournamentId).AsRequired(),
                name: CategoryName.From(c.Name).AsRequired(),
                dancesIds: new List<DanceId>(),
                refereesIds: new List<RefereeId>(),
                couplesIds: new List<CoupleId>()
            )).ToList().AsRequired();
    }
}