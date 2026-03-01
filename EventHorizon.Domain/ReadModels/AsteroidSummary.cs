namespace EventHorizon.Domain.ReadModels;

public class AsteroidSummary
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public double DiameterMinKm { get; set; }
    public double DiameterMaxKm { get; set; }
    public bool IsPotentiallyHazardous { get; set; }
    public double? MissDistanceKm { get; set; }
    public double? RelativeVelocityKmPerSecond { get; set; }
    public DateTimeOffset? CloseApproachDate { get; set; }
    public DateTimeOffset DetectedAt { get; set; }
    public DateTimeOffset? LastUpdatedAt { get; set; }
}