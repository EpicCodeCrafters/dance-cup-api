using ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel;
using ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel.Views;

namespace ECC.DanceCup.Api.Infrastructure.Storage.ReadModel;

public class DanceViewRepository : IDanceViewRepository
{
    public async Task<IReadOnlyCollection<DanceView>> FindAllAsync(CancellationToken cancellationToken)
    {
        // TODO Брать из БД
        var fakeDancesList = new[]
        {
            new DanceView
            {
                Id = 1,
                ShortName = "Vw",
                Name = "Венский вальс"
            },
            new DanceView
            {
                Id = 2,
                ShortName = "Ch",
                Name = "Ча-ча-ча"
            }
        };

        return fakeDancesList;
    }
}