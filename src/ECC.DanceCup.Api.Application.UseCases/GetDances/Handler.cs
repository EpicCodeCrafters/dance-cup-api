using ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel.Views;
using FluentResults;
using MediatR;

namespace ECC.DanceCup.Api.Application.UseCases.GetDances;

public static partial class GetDancesUseCase
{
    public class QueryHandler : IRequestHandler<Query, Result<QueryResponse>>
    {
        public async Task<Result<QueryResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            // TODO брать из БД
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

            return new QueryResponse(fakeDancesList);
        }
    }
}