using ECC.DanceCup.Api.Domain.Core;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace ECC.DanceCup.Api.Domain.Model;

/// <summary>
/// Категория
/// </summary>
public class Category : Entity<CategoryId>
{
    private readonly List<DanceId> _dances;

    public Category(
        CategoryId id,
        DateTime createdAt,
        DateTime changedAt,
        List<DanceId> dances)
        : base(id, createdAt, changedAt)
    {
        _dances = dances;
    }

    /// <summary>
    /// Список танцев категории
    /// </summary>
    public IReadOnlyCollection<DanceId> Dances => _dances;
}