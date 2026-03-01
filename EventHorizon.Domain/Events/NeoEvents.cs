namespace EventHorizon.Domain.Events
{
    public record NearEarthObjectDetected(
    string NasaId,
    string Name,
    double DiameterMinKm,
    double DiameterMaxKm,
    bool IsPotentiallyHazardous,
    DateTimeOffset DetectedAt
);

    public record ApproachDataUpdated(
        string NasaId,
        double MissDistanceKm,
        double RelativeVelocityKmPerSecond,
        DateTimeOffset CloseApproachDate
    );
}
