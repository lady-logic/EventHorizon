using EventHorizon.Application.Asteroids.Queries;
using EventHorizon.Application.Interfaces;
using EventHorizon.Domain.Aggregates;
using MediatR;

namespace EventHorizon.Application.Asteroids.Handlers;

public class GetAsteroidAtPointInTimeHandler
    : IRequestHandler<GetAsteroidAtPointInTimeQuery, NearEarthObject?>
{
    private readonly IAsteroidRepository _repository;

    public GetAsteroidAtPointInTimeHandler(IAsteroidRepository repository)
    {
        _repository = repository;
    }

    public async Task<NearEarthObject?> Handle(
        GetAsteroidAtPointInTimeQuery request,
        CancellationToken cancellationToken)
    {
        return await _repository.GetAtPointInTimeAsync(
            request.NasaId,
            request.At,
            cancellationToken);
    }
}