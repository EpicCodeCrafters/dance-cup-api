using ECC.DanceCup.Api.Domain.Core;

namespace ECC.DanceCup.Api.Domain.Model.TournamentAggregate;

/// <summary>
/// Название организации, в которой пара занимается танцами
/// </summary>
public readonly record struct CoupleDanceOrganizationName : IValueObject<CoupleDanceOrganizationName, string>
{
    private CoupleDanceOrganizationName(string value)
    {
        Value = value;
    }

    /// <inheritdoc />
    public string Value { get; }
    
    /// <inheritdoc />
    public static CoupleDanceOrganizationName? From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return new CoupleDanceOrganizationName(value);
    }
}