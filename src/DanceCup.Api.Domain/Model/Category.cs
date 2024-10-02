using DanceCup.Api.Domain.Core;

namespace DanceCup.Api.Domain.Model;

public class Category : Entity<CategoryId>
{
    public Category(CategoryId id, DateTime createdAt, DateTime changedAt) : base(id, createdAt, changedAt)
    {
    }
}