namespace ECC.DanceCup.Api.Application.Abstractions.Storage.ReadModel.Views;

public class RefereeView
{
    public required long Id { get; init; }

    public required string FullName { get; init; }
}