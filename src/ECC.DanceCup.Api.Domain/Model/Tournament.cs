using ECC.DanceCup.Api.Domain.Core;
using ECC.DanceCup.Api.Domain.Error;
using FluentResults;

namespace ECC.DanceCup.Api.Domain.Model;

/// <summary>
/// Турнир
/// </summary>
public class Tournament : AggregateRoot<TournamentId>
{
    private readonly List<Category> _categories;
    
    public Tournament(
        TournamentId id, 
        int version,
        DateTime createdAt, 
        DateTime changedAt,
        UserId userId,
        TournamentName name,
        TournamentDate date,
        TournamentState state,
        DateTime? registrationStartedAt,
        DateTime? registrationFinishedAt,
        DateTime? startedAt,
        DateTime? finishedAt,
        List<Category> categories
    ) : base(id, version, createdAt, changedAt)
    {
        UserId = userId;
        Name = name;
        Date = date;
        State = state;
        RegistrationStartedAt = registrationStartedAt;
        RegistrationFinishedAt = registrationFinishedAt;
        StartedAt = startedAt;
        FinishedAt = finishedAt;
        _categories = categories;
    }
    
    /// <summary>
    /// Идентификатор создателя турнира
    /// </summary>
    public UserId UserId { get; }
    
    /// <summary>
    /// Название турнира
    /// </summary>
    public TournamentName Name { get; }
    
    /// <summary>
    /// Дата турнира
    /// </summary>
    public TournamentDate Date { get; }
    
    /// <summary>
    /// Состояние турнира
    /// </summary>
    public TournamentState State { get; private set; }
    
    /// <summary>
    /// Время начала регистрации
    /// </summary>
    public DateTime? RegistrationStartedAt { get; private set; }
    
    /// <summary>
    /// Время окончания регистрации
    /// </summary>
    public DateTime? RegistrationFinishedAt { get; private set; }
    
    /// <summary>
    /// Время начала турнира
    /// </summary>
    public DateTime? StartedAt { get; private set; }
    
    /// <summary>
    /// Время окончания турнира
    /// </summary>
    public DateTime? FinishedAt { get; private set; }

    /// <summary>
    /// Список категорий турнира
    /// </summary>
    public IReadOnlyCollection<Category> Categories => _categories;

    /// <summary>
    /// Начинает процесс регистрации на турнир
    /// </summary>
    /// <returns></returns>
    public Result StartRegistration()
    {
        if (State is not TournamentState.Created)
        {
            return new TournamentShouldBeInStatusError(Id, TournamentState.Created);
        }

        State = TournamentState.RegistrationInProgress;
        RegistrationStartedAt = DateTime.UtcNow;
        RegisterChange();
        
        return Result.Ok();
    }
    
    /// <summary>
    /// Завершает процесс регистрации на турнир
    /// </summary>
    /// <returns></returns>
    public Result FinishRegistration()
    {
        if (State is not TournamentState.RegistrationInProgress)
        {
            return new TournamentShouldBeInStatusError(Id, TournamentState.RegistrationInProgress);
        }

        State = TournamentState.RegistrationFinished;
        RegistrationFinishedAt = DateTime.UtcNow;
        RegisterChange();
        
        return Result.Ok();
    }

    /// <summary>
    /// Возобновляет процесс регистрации на турнир
    /// </summary>
    /// <returns></returns>
    public Result ReopenRegistration()
    {
        if (State is not TournamentState.RegistrationFinished)
        {
            return new TournamentShouldBeInStatusError(Id, TournamentState.RegistrationFinished);
        }

        State = TournamentState.RegistrationInProgress;
        RegistrationFinishedAt = null;
        RegisterChange();
        
        return Result.Ok();
    }
}