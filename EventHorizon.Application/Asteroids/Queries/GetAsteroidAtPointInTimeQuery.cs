using EventHorizon.Domain.Aggregates;
using MediatR;

namespace EventHorizon.Application.Asteroids.Queries;

public record GetAsteroidAtPointInTimeQuery(
    string NasaId,
    DateTimeOffset At
) : IRequest<NearEarthObject?>;