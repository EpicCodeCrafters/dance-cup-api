using ECC.DanceCup.Api.Domain.Core;

namespace ECC.DanceCup.Api.Domain.Model.TournamentAggregate;

/// <summary>
/// Прикреплённый файл к турниру
/// </summary>
/// <param name="Number">Номер</param>
/// <param name="Name">Название</param>
public record TournamentAttachment(
    int Number, 
    string Name
) : IValueObject<TournamentAttachment>;