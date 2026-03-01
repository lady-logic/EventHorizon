using EventHorizon.Application.Interfaces;
using EventHorizon.Domain.ReadModels;
using Marten;

namespace EventHorizon.Infrastructure.Persistence;

public class AsteroidRepository : IAsteroidRepository
{
    private readonly IQuerySession _session;

    public AsteroidRepository(IQuerySession session)
    {
        _session = session;
    }

    public async Task<IReadOnlyList<AsteroidSummary>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _session.Query<AsteroidSummary>()
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<AsteroidSummary>> GetHazardousAsync(CancellationToken cancellationToken)
    {
        return await _session.Query<AsteroidSummary>()
            .Where(a => a.IsPotentiallyHazardous)
            .ToListAsync(cancellationToken);
    }
}