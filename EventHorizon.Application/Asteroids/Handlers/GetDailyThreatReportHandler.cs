using EventHorizon.Application.Asteroids.Queries;
using EventHorizon.Application.Interfaces;
using EventHorizon.Domain.ReadModels;
using MediatR;

namespace EventHorizon.Application.Asteroids.Handlers;

public class GetDailyThreatReportHandler
    : IRequestHandler<GetDailyThreatReportQuery, DailyThreatReport?>
{
    private readonly IAsteroidRepository _repository;

    public GetDailyThreatReportHandler(IAsteroidRepository repository)
    {
        _repository = repository;
    }

    public async Task<DailyThreatReport?> Handle(
        GetDailyThreatReportQuery request,
        CancellationToken cancellationToken)
    {
        return await _repository.GetDailyThreatReportAsync(request.Date, cancellationToken);
    }
}