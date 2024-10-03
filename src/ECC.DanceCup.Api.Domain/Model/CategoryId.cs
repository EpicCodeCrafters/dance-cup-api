﻿using ECC.DanceCup.Api.Domain.Core;

namespace ECC.DanceCup.Api.Domain.Model;

/// <summary>
/// Идентификатор категории
/// </summary>
public readonly record struct CategoryId : IValueObject<CategoryId, long>
{
    private CategoryId(long value)
    {
        Value = value;
    }

    /// <inheritdoc />
    public long Value { get; }
    
    /// <inheritdoc />
    public static CategoryId? From(long value)
    {
        if (value <= 0)
        {
            return null;
        }

        return new CategoryId(value);
    }
}