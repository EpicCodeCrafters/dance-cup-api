using ECC.DanceCup.Api.Application.Abstractions.Storage.DomainModel;
using ECC.DanceCup.Api.Domain.Services;
using FluentResults;
using MediatR;

namespace ECC.DanceCup.Api.Application.UseCases.CreateUser;

public static partial class CreateUserUseCase
{
    public class CommandHandler : IRequestHandler<Command, Result<CommandResponse>>
    {
        private readonly IUserFactory _userFactory;
        private readonly IUserRepository _userRepository;

        public CommandHandler(IUserFactory userFactory, IUserRepository userRepository)
        {
            _userFactory = userFactory;
            _userRepository = userRepository;
        }

        public async Task<Result<CommandResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            var createUserResult = _userFactory.Create(request.ExternalId, request.Username);
            if (createUserResult.IsFailed)
            {
                return createUserResult.ToResult();
            }
            
            var user = createUserResult.Value;
            var userId = await _userRepository.InsertAsync(user, cancellationToken);

            return new CommandResponse(userId);
        }
    }
}