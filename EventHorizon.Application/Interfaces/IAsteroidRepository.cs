using EventHorizon.Domain.Aggregates;
using EventHorizon.Domain.ReadModels;

namespace EventHorizon.Application.Interfaces;

public interface IAsteroidRepository
{
    Task<IReadOnlyList<AsteroidSummary>> GetAllAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<AsteroidSummary>> GetHazardousAsync(CancellationToken cancellationToken);
    Task<NearEarthObject?> GetAtPointInTimeAsync(string nasaId, DateTimeOffset at, CancellationToken cancellationToken);
}