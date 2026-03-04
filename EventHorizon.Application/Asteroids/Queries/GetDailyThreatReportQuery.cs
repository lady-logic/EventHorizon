using EventHorizon.Domain.ReadModels;
using MediatR;

namespace EventHorizon.Application.Asteroids.Queries;

public record GetDailyThreatReportQuery(DateOnly Date) : IRequest<DailyThreatReport?>;