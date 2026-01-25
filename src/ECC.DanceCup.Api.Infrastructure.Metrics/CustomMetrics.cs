using ECC.DanceCup.Api.Application.Abstractions.Metrics;
using Prometheus;

namespace ECC.DanceCup.Api.Infrastructure.Metrics;

public class CustomMetrics : IMetrics
{
    private readonly Counter _danceCacheHitCounter = Prometheus.Metrics.CreateCounter(
        name: "dance_cache_hits_count", 
        help: "Dance cache hits count"
    );

    private readonly Counter _danceCacheMissCounter = Prometheus.Metrics.CreateCounter(
        name: "dance_cache_misses_count",
        help: "Dance cache misses count"
    );
    
    public void IncDanceCacheHit()
    {
        _danceCacheHitCounter.Inc();
    }

    public void IncDanceCacheMiss()
    {
        _danceCacheMissCounter.Inc();
    }
}