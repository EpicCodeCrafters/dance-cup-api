namespace ECC.DanceCup.Api.Application.Abstractions.TgApi;

public interface ITgApi
{
    Task SendMessageAsync(string message, CancellationToken cancellationToken);
}