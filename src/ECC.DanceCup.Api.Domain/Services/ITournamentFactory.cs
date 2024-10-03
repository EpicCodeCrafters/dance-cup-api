﻿using ECC.DanceCup.Api.Domain.Model;
using FluentResults;

namespace ECC.DanceCup.Api.Domain.Services;

/// <summary>
/// Фабрика турниров
/// </summary>
public interface ITournamentFactory
{
    /// <summary>
    /// Создаёт турнир
    /// </summary>
    /// <returns></returns>
    Result<Tournament> Create();
}