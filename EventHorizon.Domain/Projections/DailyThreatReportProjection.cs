using EventHorizon.Domain.Events;
using EventHorizon.Domain.ReadModels;
using Marten.Events.Aggregation;
using Marten.Events.Projections;

namespace EventHorizon.Domain.Projections;

public class DailyThreatReportProjection : MultiStreamProjection<DailyThreatReport, string>
{
    public DailyThreatReportProjection()
    {
        Identity<NearEarthObjectDetected>(e =>
            e.DetectedAt.ToString("yyyy-MM-dd"));

        Identity<ApproachDataUpdated>(e =>
            e.CloseApproachDate.ToString("yyyy-MM-dd"));
    }

    public void Apply(DailyThreatReport report, NearEarthObjectDetected @event)
    {
        report.Id = @event.DetectedAt.ToString("yyyy-MM-dd");
        report.Date = DateOnly.FromDateTime(@event.DetectedAt.UtcDateTime);
        report.TotalAsteroidsDetected++;

        if (@event.IsPotentiallyHazardous)
        {
            report.HazardousCount++;
            report.HazardousAsteroidNames.Add(@event.Name);
        }
    }

    public void Apply(DailyThreatReport report, ApproachDataUpdated @event)
    {
        if (report.ClosestApproachKm is null ||
            @event.MissDistanceKm < report.ClosestApproachKm)
        {
            report.ClosestApproachKm = @event.MissDistanceKm;
            report.ClosestAsteroidName = @event.NasaId;
        }
    }
}