using ECC.DanceCup.Api.Domain.Model.RefereeAggregate;
using FluentResults;

namespace ECC.DanceCup.Api.Domain.Services;

/// <summary>
/// Фабрика судей
/// </summary>
public interface IRefereeFactory
{
    /// <summary>
    /// Создаёт судью
    /// </summary>
    /// <param name="fullName">Полное имя судьи</param>
    /// <returns></returns>
    Result<Referee> Create(RefereeFullName fullName);
}