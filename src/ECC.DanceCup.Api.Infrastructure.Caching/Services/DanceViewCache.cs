using System.Text.Json;
using ECC.DanceCup.Api.Application.Abstractions.Caching;
using ECC.DanceCup.Api.Application.Abstractions.Models.Views;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace ECC.DanceCup.Api.Infrastructure.Caching.Services;

public class DanceViewCache : IDanceViewCache
{
    private const string CacheKey = "dances";
    
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<DanceViewCache> _logger;

    public DanceViewCache(
        IDistributedCache distributedCache, 
        ILogger<DanceViewCache> logger)
    {
        _distributedCache = distributedCache;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<DanceView>?> FindAllAsync(CancellationToken cancellationToken)
    {
        try
        {
            var dancesSerialized = await _distributedCache.GetStringAsync(CacheKey, cancellationToken);
            if (dancesSerialized == null)
            {
                return null;
            }
        
            var dances = JsonSerializer.Deserialize<IReadOnlyCollection<DanceView>>(dancesSerialized);
            
            _logger.LogDebug("Кэшированный список танцев получен");

            return dances;
        }
        catch (Exception exception)
        {
            _logger.LogError("Не удалось получить кэшированный список танцев: {reason}", exception.Message);
            return null;
        }
    }

    public async Task InsertRangeAsync(IReadOnlyCollection<DanceView> danceViews, CancellationToken cancellationToken)
    {
        try
        {
            var dancesSerialized = JsonSerializer.Serialize(danceViews);
            await _distributedCache.SetStringAsync(CacheKey, dancesSerialized, cancellationToken);
            
            _logger.LogDebug("Кэшированный список танцев сохранён");
        }
        catch (Exception exception)
        {
            _logger.LogError("Не удалось сохранить кэшированный список танцев: {reason}", exception.Message);
        }
    }
}