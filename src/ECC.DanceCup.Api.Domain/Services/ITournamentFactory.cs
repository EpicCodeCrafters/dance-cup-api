using ECC.DanceCup.Api.Domain.Core;
using ECC.DanceCup.Api.Domain.Model.DanceAggregate;
using ECC.DanceCup.Api.Domain.Model.RefereeAggregate;
using ECC.DanceCup.Api.Domain.Model.TournamentAggregate;
using ECC.DanceCup.Api.Domain.Model.UserAggregate;
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
    /// <param name="description">Описание турнира</param>
    /// <param name="date">Дата турнира</param>
    /// <param name="createCategoryModels">Список описаний категорий</param>
    /// <returns></returns>
    Result<Tournament> Create(
        UserId userId,
        TournamentName name,
        TournamentDescription description,
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
    TournamentDescription Name,
    IReadOnlyCollection<DanceId> DancesIds,
    IReadOnlyCollection<RefereeId> RefereesIds
) : IValueObject<CreateCategoryModel>;