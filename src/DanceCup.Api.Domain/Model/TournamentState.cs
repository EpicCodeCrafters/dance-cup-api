namespace DanceCup.Api.Domain.Model;

/// <summary>
/// Состояние турнира
/// </summary>
public enum TournamentState
{
    /// <summary>
    /// Регистрация
    /// </summary>
    RegistrationInProgress,
    
    /// <summary>
    /// Регистрация закончена
    /// </summary>
    RegistrationFinished,
    
    /// <summary>
    /// Проводится
    /// </summary>
    InProgress,
    
    /// <summary>
    /// Завершён
    /// </summary>
    Finished
}