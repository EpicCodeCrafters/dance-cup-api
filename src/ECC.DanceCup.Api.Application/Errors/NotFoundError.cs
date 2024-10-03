using FluentResults;

namespace ECC.DanceCup.Api.Application.Errors;

public abstract class NotFoundError : Error
{
    protected NotFoundError(string message)
    {
        Message = message;
    }
}