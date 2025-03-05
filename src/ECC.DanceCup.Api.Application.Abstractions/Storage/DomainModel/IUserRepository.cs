using ECC.DanceCup.Api.Domain.Model.UserAggregate;

namespace ECC.DanceCup.Api.Application.Abstractions.Storage.DomainModel;

public interface IUserRepository
{
    Task<UserId> InsertAsync(User user, CancellationToken cancellationToken);
}