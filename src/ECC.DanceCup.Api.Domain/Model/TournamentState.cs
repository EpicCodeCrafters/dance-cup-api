namespace ECC.DanceCup.Api.Domain.Model;

/// <summary>
/// Состояние турнира
/// </summary>
public enum TournamentState
{
    /// <summary>
    /// Создан
    /// </summary>
    Created,
    
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