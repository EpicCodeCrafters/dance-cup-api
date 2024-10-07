using ECC.DanceCup.Api.Domain.Model;
using FluentResults;

namespace ECC.DanceCup.Api.Domain.Services;

/// <summary>
/// Фабрика турниров
/// </summary>
public interface ITournamentFactory
{
    /// <summary>
    /// Создаёт турнир
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="name">Название турнира</param>
    /// <param name="date">Дата турнира</param>
    /// <param name="createCategoryModels">Список описаний категорий</param>
    /// <returns></returns>
    Result<Tournament> Create(
        UserId userId,
        TournamentName name,
        TournamentDate date,
        IReadOnlyCollection<CreateCategoryModel> createCategoryModels
    );
}

/// <summary>
/// Описание категории при создании
/// </summary>
/// <param name="Name">Название категории</param>
/// <param name="DancesIds">Список идентификаторов танцев категории</param>
/// <param name="RefereesIds">Список идентификаторов судей категории</param>
public record CreateCategoryModel(
    CategoryName Name,
    IReadOnlyCollection<DanceId> DancesIds,
    IReadOnlyCollection<RefereeId> RefereesIds
);