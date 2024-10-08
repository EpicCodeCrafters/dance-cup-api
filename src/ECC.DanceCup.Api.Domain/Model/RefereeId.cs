﻿using ECC.DanceCup.Api.Domain.Core;

namespace ECC.DanceCup.Api.Domain.Model;

/// <summary>
/// Идентификатор судьи
/// </summary>
public readonly record struct RefereeId : IValueObject<RefereeId, long>
{
    private RefereeId(long value)
    {
        Value = value;
    }

    /// <inheritdoc />
    public long Value { get; }

    /// <inheritdoc />
    public static RefereeId? From(long value)
    {
        if (value <= 0)
        {
            return null;
        }

        return new RefereeId(value);
    }
}

