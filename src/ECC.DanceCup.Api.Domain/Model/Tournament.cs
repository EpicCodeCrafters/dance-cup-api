using ECC.DanceCup.Api.Domain.Core;

namespace ECC.DanceCup.Api.Domain.Model;

/// <summary>
/// Турнир
/// </summary>
public class Tournament : AggregateRoot<TournamentId>
{
    private readonly List<Category> _categories;
    
    public Tournament(
        TournamentId id, 
        DateTime createdAt, 
        DateTime changedAt, 
        UserId userId,
        TournamentName name,
        TournamentDate date,
        TournamentState state,
        DateTime? startedAt,
        DateTime? finishedAt,
        List<Category> categories)
        : base(id, createdAt, changedAt)
    {
        UserId = userId;
        Name = name;
        Date = date;
        State = state;
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
    public TournamentState State { get; }
    
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
}