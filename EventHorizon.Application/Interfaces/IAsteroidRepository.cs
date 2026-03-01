using EventHorizon.Domain.ReadModels;

namespace EventHorizon.Application.Interfaces;

public interface IAsteroidRepository
{
    Task<IReadOnlyList<AsteroidSummary>> GetAllAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<AsteroidSummary>> GetHazardousAsync(CancellationToken cancellationToken);
}