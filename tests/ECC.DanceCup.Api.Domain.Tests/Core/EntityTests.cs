using ECC.DanceCup.Api.Domain.Core;
using ECC.DanceCup.Api.Tests.Common.Attributes;
using FluentAssertions;

namespace ECC.DanceCup.Api.Domain.Tests.Core;

public class EntityTests
{
    private class TestEntity : Entity<long>
    {
        public TestEntity(long id) : base(id)
        {
        }
    }
    
    private class DifferentEntity : Entity<long>
    {
        public DifferentEntity(long id) : base(id)
        {
        }
    }

    [Theory, AutoMoqData]
    public void Entity_WithSameId_ShouldBeEqual(long id)
    {
        // Arrange
        var entity1 = new TestEntity(id);
        var entity2 = new TestEntity(id);

        // Act & Assert
        entity1.Equals(entity2).Should().BeTrue();
        entity1.Equals((IEntity<long>)entity2).Should().BeTrue();
        entity1.Equals((object)entity2).Should().BeTrue();
    }

    [Theory, AutoMoqData]
    public void Entity_WithDifferentId_ShouldNotBeEqual(long id1, long id2)
    {
        // Arrange
        var entity1 = new TestEntity(id1);
        var entity2 = new TestEntity(id2);

        // Act & Assert
        entity1.Equals(entity2).Should().BeFalse();
        entity1.Equals((IEntity<long>)entity2).Should().BeFalse();
        entity1.Equals((object)entity2).Should().BeFalse();
    }

    [Theory, AutoMoqData]
    public void Entity_EqualsNull_ShouldReturnFalse(long id)
    {
        // Arrange
        var entity = new TestEntity(id);

        // Act & Assert
        entity.Equals(null).Should().BeFalse();
        entity.Equals((IEntity<long>?)null).Should().BeFalse();
        entity.Equals((object?)null).Should().BeFalse();
    }

    [Theory, AutoMoqData]
    public void Entity_EqualsDifferentType_ShouldStillBeEqualById(long id)
    {
        // Arrange
        var entity1 = new TestEntity(id);
        var entity2 = new DifferentEntity(id);

        // Act & Assert
        // Note: Entity base class compares by ID only, so different types with same ID are considered equal
        entity1.Equals((object)entity2).Should().BeTrue();
    }

    [Theory, AutoMoqData]
    public void Entity_GetHashCode_ShouldReturnIdHashCode(long id)
    {
        // Arrange
        var entity = new TestEntity(id);

        // Act
        var hashCode = entity.GetHashCode();

        // Assert
        hashCode.Should().Be(id.GetHashCode());
    }

    [Theory, AutoMoqData]
    public void Entity_WithSameId_ShouldHaveSameHashCode(long id)
    {
        // Arrange
        var entity1 = new TestEntity(id);
        var entity2 = new TestEntity(id);

        // Act & Assert
        entity1.GetHashCode().Should().Be(entity2.GetHashCode());
    }
}
