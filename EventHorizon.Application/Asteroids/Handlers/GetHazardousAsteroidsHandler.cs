using EventHorizon.Application.Asteroids.Queries;
using EventHorizon.Application.Interfaces;
using EventHorizon.Domain.ReadModels;
using MediatR;

namespace EventHorizon.Application.Asteroids.Handlers;

public class GetHazardousAsteroidsHandler : IRequestHandler<GetHazardousAsteroidsQuery, IReadOnlyList<AsteroidSummary>>
{
    private readonly IAsteroidRepository _repository;

    public GetHazardousAsteroidsHandler(IAsteroidRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<AsteroidSummary>> Handle(
        GetHazardousAsteroidsQuery request,
        CancellationToken cancellationToken)
    {
        return await _repository.GetHazardousAsync(cancellationToken);
    }
}