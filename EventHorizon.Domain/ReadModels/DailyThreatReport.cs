namespace EventHorizon.Domain.ReadModels;

public class DailyThreatReport
{
    public string Id { get; set; } = default!;  // das Datum, z.B. "2026-03-01"
    public DateOnly Date { get; set; }
    public int TotalAsteroidsDetected { get; set; }
    public int HazardousCount { get; set; }
    public double? ClosestApproachKm { get; set; }
    public string? ClosestAsteroidName { get; set; }
    public List<string> HazardousAsteroidNames { get; set; } = [];
}