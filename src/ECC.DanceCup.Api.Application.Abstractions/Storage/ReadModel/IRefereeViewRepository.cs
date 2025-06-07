using ECC.DanceCup.Api.Application.Abstractions.Models.Views;
using ECC.DanceCup.Api.Domain.Model.RefereeAggregate;

namespace ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel;

public interface IRefereeViewRepository
{
    Task<IReadOnlyCollection<RefereeView>> FindAllAsync(RefereeFullName? refereeFullName, int pageNumber, int pageSize, CancellationToken cancellationToken);
}