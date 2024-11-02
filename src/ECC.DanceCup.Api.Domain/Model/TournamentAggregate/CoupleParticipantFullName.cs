using ECC.DanceCup.Api.Domain.Core;

namespace ECC.DanceCup.Api.Domain.Model.TournamentAggregate;

/// <summary>
/// Полное имя участника пары
/// </summary>
public readonly record struct CoupleParticipantFullName : IValueObject<CoupleParticipantFullName, string>
{
    private CoupleParticipantFullName(string value)
    {
        Value = value;
    }

    /// <inheritdoc />
    public string Value { get; }

    /// <inheritdoc />
    public static CoupleParticipantFullName? From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return new CoupleParticipantFullName(value);
    }
}