using BenchmarkDotNet.Attributes;
using ECC.DanceCup.Api.Application.Abstractions.Models.Views;
using ECC.DanceCup.Api.Infrastructure.Caching.Services;
using ECC.DanceCup.Api.Infrastructure.Storage.Options;
using ECC.DanceCup.Api.Infrastructure.Storage.ReadModel;
using ECC.DanceCup.Api.Infrastructure.Storage.Tools;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace ECC.DanceCup.Api.Benchmark;

public class RedisBenchmark
{
    [Benchmark(Baseline = true)]
    public async Task<IReadOnlyCollection<DanceView>> GetDancesWithoutCaching()
    {
        var storageOptionsMock = new Mock<IOptions<StorageOptions>>();
        storageOptionsMock
            .SetupGet(o => o.Value)
            .Returns(new StorageOptions
            {
                ConnectionString = "Host=localhost; Port=15432; Database=dance-cup-api; Username=postgres-user; Password=postgres;",
            });

        var postgresConnectionFactory = new PostgresConnectionFactory(storageOptionsMock.Object);
        var danceViewRepository = new DanceViewRepository(postgresConnectionFactory);
        
        var danceViews = await danceViewRepository.FindAllAsync(CancellationToken.None);
        
        return danceViews;
    }

    [Benchmark]
    public async Task<IReadOnlyCollection<DanceView>> GetDancesWithCaching()
    {
        var storageOptionsMock = new Mock<IOptions<StorageOptions>>();
        storageOptionsMock
            .SetupGet(o => o.Value)
            .Returns(new StorageOptions
            {
                ConnectionString = "Host=localhost; Port=15432; Database=dance-cup-api; Username=postgres-user; Password=postgres;",
            });

        var postgresConnectionFactory = new PostgresConnectionFactory(storageOptionsMock.Object);
        var danceViewRepository = new DanceViewRepository(postgresConnectionFactory);
        
        var redisOptions = new RedisCacheOptions
        {
            Configuration = "localhost:16379"
        };
        var optionsAccessor = Options.Create(redisOptions);
        IDistributedCache distributedCache = new RedisCache(optionsAccessor);

        var danceViewCache = new DanceViewCache(distributedCache, new Mock<ILogger<DanceViewCache>>(MockBehavior.Loose).Object);
        
        var dances = await danceViewCache.FindAllAsync(CancellationToken.None);
        if (dances is null)
        {
            dances = await danceViewRepository.FindAllAsync(CancellationToken.None);
            await danceViewCache.InsertRangeAsync(dances, CancellationToken.None);
        }

        return dances;
    }
}