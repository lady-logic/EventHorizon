using EventHorizon.Application.Asteroids.Queries;
using EventHorizon.Application.Interfaces;
using EventHorizon.Domain.ReadModels;
using MediatR;

namespace EventHorizon.Application.Asteroids.Handlers;

public class GetAllAsteroidSummariesHandler : IRequestHandler<GetAllAsteroidSummariesQuery, IReadOnlyList<AsteroidSummary>>
{
    private readonly IAsteroidRepository _repository;

    public GetAllAsteroidSummariesHandler(IAsteroidRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<AsteroidSummary>> Handle(
        GetAllAsteroidSummariesQuery request,
        CancellationToken cancellationToken)
    {
        return await _repository.GetAllAsync(cancellationToken);
    }
}