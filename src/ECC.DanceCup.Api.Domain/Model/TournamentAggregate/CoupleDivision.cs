using ECC.DanceCup.Api.Domain.Core;

namespace ECC.DanceCup.Api.Domain.Model.TournamentAggregate;

/// <summary>
/// Отделение организации, в которой пара занимается танцами
/// </summary>
public readonly record struct CoupleDivision : IValueObject<CoupleDivision, string>
{
    private CoupleDivision(string value)
    {
        Value = value;
    }

    /// <inheritdoc />
    public string Value { get; }
    
    /// <inheritdoc />
    public static CoupleDivision? From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return new CoupleDivision(value);
    }
}
