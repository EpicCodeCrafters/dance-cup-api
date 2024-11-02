using ECC.DanceCup.Api.Domain.Core;

namespace ECC.DanceCup.Api.Domain.Model.TournamentAggregate;

/// <summary>
/// Полное имя тренера пары
/// </summary>
public readonly record struct CoupleTrainerFullName : IValueObject<CoupleTrainerFullName, string>
{
    private CoupleTrainerFullName(string value)
    {
        Value = value;
    }

    /// <inheritdoc />
    public string Value { get; }
    
    /// <inheritdoc />
    public static CoupleTrainerFullName? From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return new CoupleTrainerFullName(value);
    }
}