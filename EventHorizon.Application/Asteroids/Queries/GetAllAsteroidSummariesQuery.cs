using EventHorizon.Domain.ReadModels;
using MediatR;

namespace EventHorizon.Application.Asteroids.Queries;

public record GetAllAsteroidSummariesQuery() : IRequest<IReadOnlyList<AsteroidSummary>>;