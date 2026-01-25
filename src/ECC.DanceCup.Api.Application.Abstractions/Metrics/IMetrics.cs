namespace ECC.DanceCup.Api.Application.Abstractions.Metrics;

public interface IMetrics
{
    void IncDanceCacheHit();
    
    void IncDanceCacheMiss();
}