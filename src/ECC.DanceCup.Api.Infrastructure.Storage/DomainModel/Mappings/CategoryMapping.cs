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
    
    public static List<Category> ToDomain(this IEnumerable<CategoryDbo> categories, Dictionary<long, List<DanceId>> danceIdsGroupedByCategory, Dictionary<long, List<RefereeId>> refereeIdsGroupedByCategory, Dictionary<long, List<CoupleId>> coupleIdsGroupedByCategory)
    {
        return categories.Select(c =>
            new Category(
                id: CategoryId.From(c.Id).AsRequired(),
                tournamentId: TournamentId.From(c.TournamentId).AsRequired(),
                name: CategoryName.From(c.Name).AsRequired(),
                dancesIds: danceIdsGroupedByCategory.TryGetValue(c.Id, out List<DanceId> danceIds) ? danceIds : new List<DanceId>(),
                refereesIds: refereeIdsGroupedByCategory.TryGetValue(c.Id, out var refereeIds) ? refereeIds : new List<RefereeId>(),
                couplesIds: coupleIdsGroupedByCategory.TryGetValue(c.Id, out var coupleIds) ? coupleIds : new List<CoupleId>()
            )).ToList().AsRequired();
    }
}