using EventHorizon.Domain.Events;

namespace EventHorizon.Domain.Aggregates
{
    public class NearEarthObject
    {
        public string Id { get; private set; } = default!;
        public string Name { get; private set; } = default!;
        public double DiameterMinKm { get; private set; }
        public double DiameterMaxKm { get; private set; }
        public bool IsPotentiallyHazardous { get; private set; }
        public double? MissDistanceKm { get; private set; }
        public double? RelativeVelocityKmPerSecond { get; private set; }
        public DateTimeOffset? CloseApproachDate { get; private set; }
        public DateTimeOffset DetectedAt { get; private set; }

        public static NearEarthObject Create(NearEarthObjectDetected @event)
        {
            var neo = new NearEarthObject();
            neo.Apply(@event);
            return neo;
        }

        public void Apply(NearEarthObjectDetected @event)
        {
            Id = @event.NasaId;
            Name = @event.Name;
            DiameterMinKm = @event.DiameterMinKm;
            DiameterMaxKm = @event.DiameterMaxKm;
            IsPotentiallyHazardous = @event.IsPotentiallyHazardous;
            DetectedAt = @event.DetectedAt;
        }

        public void Apply(ApproachDataUpdated @event)
        {
            MissDistanceKm = @event.MissDistanceKm;
            RelativeVelocityKmPerSecond = @event.RelativeVelocityKmPerSecond;
            CloseApproachDate = @event.CloseApproachDate;
        }
    }
}
