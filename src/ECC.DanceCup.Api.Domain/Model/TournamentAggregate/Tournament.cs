using ECC.DanceCup.Api.Domain.Core;
using ECC.DanceCup.Api.Domain.Errors;
using ECC.DanceCup.Api.Domain.Model.UserAggregate;
using FluentResults;

namespace ECC.DanceCup.Api.Domain.Model.TournamentAggregate;

/// <summary>
/// Турнир
/// </summary>
public class Tournament : AggregateRoot<TournamentId>
{
    private readonly List<Category> _categories;
    private readonly List<Couple> _couples;
    private readonly List<TournamentAttachment> _attachments;
    
    public Tournament(
        TournamentId id, 
        AggregateVersion version,
        DateTime createdAt, 
        DateTime changedAt,
        UserId userId,
        TournamentName name,
        TournamentDescription description,
        TournamentDate date,
        TournamentState state,
        DateTime? registrationStartedAt,
        DateTime? registrationFinishedAt,
        DateTime? startedAt,
        DateTime? finishedAt,
        List<Category> categories,
        List<Couple> couples, 
        List<TournamentAttachment> attachments
    ) : base(id, version, createdAt, changedAt)
    {
        UserId = userId;
        Name = name;
        Description = description;
        Date = date;
        State = state;
        RegistrationStartedAt = registrationStartedAt;
        RegistrationFinishedAt = registrationFinishedAt;
        StartedAt = startedAt;
        FinishedAt = finishedAt;
        _categories = categories;
        _couples = couples;
        _attachments = attachments;
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
    /// Описание турнира
    /// </summary>
    public TournamentDescription Description { get; }
    
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
    /// Количество категорий турнира
    /// </summary>
    public int CategoriesCount => _categories.Count;

    /// <summary>
    /// Список пар, участвующих в турнире
    /// </summary>
    public IReadOnlyCollection<Couple> Couples => _couples;
    
    /// <summary>
    /// Прикреплённые к турниру файлы
    /// </summary>
    public IReadOnlyCollection<TournamentAttachment> Attachments => _attachments;

    /// <summary>
    /// Количество пар, участвующих в турнире
    /// </summary>
    public int CouplesCount => _couples.Count;

    /// <summary>
    /// Начинает процесс регистрации на турнир
    /// </summary>
    /// <returns></returns>
    public Result StartRegistration()
    {
        if (State is not TournamentState.Created)
        {
            return new TournamentShouldBeInStatusError(TournamentState.Created);
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
            return new TournamentShouldBeInStatusError(TournamentState.RegistrationInProgress);
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
            return new TournamentShouldBeInStatusError(TournamentState.RegistrationFinished);
        }

        State = TournamentState.RegistrationInProgress;
        RegistrationFinishedAt = null;
        RegisterChange();
        
        return Result.Ok();
    }

    /// <summary>
    /// Регистрирует пару на турнир
    /// </summary>
    /// <param name="coupleId">Идентификатор пары</param>
    /// <param name="firstParticipantFullName">Полное имя первого участика пары</param>
    /// <param name="secondParticipantFullName">Полное имя второго участика пар</param>
    /// <param name="danceOrganizationName">Название танцевальной организации пыры</param>
    /// <param name="firstTrainerFullName">Полное имя первого тренера пары</param>
    /// <param name="secondTrainerFullName">Полное имя второго тренера пары</param>
    /// <param name="categoriesIds">Список идентификаторов категорий, в которые регистрируется пара</param>
    /// <returns></returns>
    public Result RegisterCouple(
        CoupleId coupleId,
        CoupleParticipantFullName firstParticipantFullName,
        CoupleParticipantFullName? secondParticipantFullName, 
        CoupleDanceOrganizationName? danceOrganizationName, 
        CoupleTrainerFullName? firstTrainerFullName, 
        CoupleTrainerFullName? secondTrainerFullName,
        IReadOnlyCollection<CategoryId> categoriesIds)
    {
        if (State is not TournamentState.RegistrationInProgress)
        {
            return new TournamentShouldBeInStatusError(TournamentState.RegistrationInProgress);
        }
        
        if (_couples.Any(couple => couple.Id == coupleId))
        {
            return new CoupleAlreadyRegisteredForTournamentError();
        }

        if (categoriesIds.Count == 0)
        {
            return new CoupleCategoriesShouldNotBeEmptyError();
        }
        
        var categories = _categories
            .Where(category => categoriesIds.Contains(category.Id))
            .ToArray();

        var notFoundCategoriesIds = categoriesIds
            .Except(categories.Select(category => category.Id))
            .ToArray();
        if (notFoundCategoriesIds.Length != 0)
        {
            return new CategoriesNotFountError(notFoundCategoriesIds);
        }

        foreach (var category in categories)
        {
            var registerCoupleInCategoryResult = category.RegisterCouple(coupleId);
            if (registerCoupleInCategoryResult.IsFailed)
            {
                return registerCoupleInCategoryResult;
            }
        }

        var couple = new Couple(
            id: coupleId,
            tournamentId: Id,
            firstParticipantFullName: firstParticipantFullName,
            secondParticipantFullName: secondParticipantFullName,
            danceOrganizationName: danceOrganizationName,
            firstTrainerFullName: firstTrainerFullName,
            secondTrainerFullName: secondTrainerFullName
        );
        _couples.Add(couple);
        
        RegisterChange();
        
        return Result.Ok();
    }

    /// <summary>
    /// Добавляет прикрепление
    /// </summary>
    /// <param name="attachmentName"></param>
    /// <returns></returns>
    public Result<TournamentAttachment> AddAttachment(string attachmentName)
    {
        var attachment = new TournamentAttachment(_attachments.Count + 1, attachmentName);
        _attachments.Add(attachment);
        
        return attachment;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="attachmentNumber"></param>
    /// <returns></returns>
    public Result<string> RemoveAttachment(int attachmentNumber)
    {
        var attachment = _attachments.FirstOrDefault(attachment => attachment.Number == attachmentNumber);
        if (attachment is null)
        {
            return Result.Fail("Attachment not found");
        }
        
        _attachments.Remove(attachment);
        RegisterChange();

        return attachment.Name;
    }
}