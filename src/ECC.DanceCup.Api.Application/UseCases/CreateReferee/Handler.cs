using ECC.DanceCup.Api.Application.Abstractions.Storage.DomainModel;
using ECC.DanceCup.Api.Domain.Services;
using FluentResults;
using MediatR;

namespace ECC.DanceCup.Api.Application.UseCases.CreateReferee;

public static partial class CreateRefereeUseCase
{
    public class CommandHandler : IRequestHandler<Command, Result<CommandResponse>>
    {
        private readonly IRefereeFactory _refereeFactory;
        private readonly IRefereeRepository _refereeRepository;

        public CommandHandler(IRefereeFactory refereeFactory, IRefereeRepository refereeRepository)
        {
            _refereeFactory = refereeFactory;
            _refereeRepository = refereeRepository;
        }

        public async Task<Result<CommandResponse>> Handle(Command command, CancellationToken cancellationToken)
        {
            var createRefereeResult = _refereeFactory.Create(command.FullName);
            if (createRefereeResult.IsFailed)
            {
                return createRefereeResult.ToResult();
            }

            var referee = createRefereeResult.Value;
            var refereeId = await _refereeRepository.AddAsync(referee, cancellationToken);

            return new CommandResponse(refereeId);
        }
    }
}