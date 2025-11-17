using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using ECC.DanceCup.Api.Infrastructure.Storage.DomainModel.Dbo;
using ECC.DanceCup.Api.Utils.Extensions;

namespace ECC.DanceCup.Api.Infrastructure.Storage.DomainModel.Mappings;

internal static class RoundMapping
{
    public static RoundDbo ToDbo(this Round round)
    {
        return new RoundDbo
        {
            Id = round.Id.Value,
            CategoryId = round.CategoryId.Value,
            OrderNumber = round.OrderNumber.Value
        };
    }
    
    public static List<Round> ToDomain(this IEnumerable<RoundDbo> rounds, Dictionary<long, List<long>> roundsCouplesMap)
    {
        return rounds.Select(r => 
            new Round(
                id: RoundId.From(r.Id).AsRequired(),
                categoryId: CategoryId.From(r.CategoryId).AsRequired(),
                orderNumber: RoundOrderNumber.From(r.OrderNumber).AsRequired(),
                couplesIds: roundsCouplesMap.TryGetValue(r.Id, out var coupleIds)
                    ? coupleIds.Select(id => CoupleId.From(id).AsRequired()).ToList()
                    : new List<CoupleId>()
            )).ToList().AsRequired();
    }
}
