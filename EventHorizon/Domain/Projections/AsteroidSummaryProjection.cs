using EventHorizon.Api.Domain.Events;
using EventHorizon.Api.Domain.ReadModels;
using Marten.Events.Aggregation;

namespace EventHorizon.Api.Domain.Projections;

public class AsteroidSummaryProjection : SingleStreamProjection<AsteroidSummary, string>
{
    public AsteroidSummary Create(NearEarthObjectDetected @event)
    {
        return new AsteroidSummary
        {
            Id = @event.NasaId,
            Name = @event.Name,
            DiameterMinKm = @event.DiameterMinKm,
            DiameterMaxKm = @event.DiameterMaxKm,
            IsPotentiallyHazardous = @event.IsPotentiallyHazardous,
            DetectedAt = @event.DetectedAt
        };
    }

    public void Apply(AsteroidSummary summary, ApproachDataUpdated @event)
    {
        summary.MissDistanceKm = @event.MissDistanceKm;
        summary.RelativeVelocityKmPerSecond = @event.RelativeVelocityKmPerSecond;
        summary.CloseApproachDate = @event.CloseApproachDate;
        summary.LastUpdatedAt = DateTimeOffset.UtcNow;
    }
}