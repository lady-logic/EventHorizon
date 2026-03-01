using System.Text.Json.Serialization;

namespace EventHorizon.Infrastructure
{
    public record NeoWsResponse(
    [property: JsonPropertyName("near_earth_objects")]
    Dictionary<string, List<NeoWsObject>> NearEarthObjects
);

    public record NeoWsObject(
        [property: JsonPropertyName("id")]
    string Id,
        [property: JsonPropertyName("name")]
    string Name,
        [property: JsonPropertyName("estimated_diameter")]
    EstimatedDiameter EstimatedDiameter,
        [property: JsonPropertyName("is_potentially_hazardous_asteroid")]
    bool IsPotentiallyHazardous,
        [property: JsonPropertyName("close_approach_data")]
    List<CloseApproachData> CloseApproachData
    );

    public record EstimatedDiameter(
        [property: JsonPropertyName("kilometers")]
    DiameterRange Kilometers
    );

    public record DiameterRange(
        [property: JsonPropertyName("estimated_diameter_min")]
    double Min,
        [property: JsonPropertyName("estimated_diameter_max")]
    double Max
    );

    public record CloseApproachData(
        [property: JsonPropertyName("close_approach_date")]
    string CloseApproachDate,
        [property: JsonPropertyName("relative_velocity")]
    RelativeVelocity RelativeVelocity,
        [property: JsonPropertyName("miss_distance")]
    MissDistance MissDistance
    );

    public record RelativeVelocity(
        [property: JsonPropertyName("kilometers_per_second")]
    string KilometersPerSecond
    );

    public record MissDistance(
        [property: JsonPropertyName("kilometers")]
    string Kilometers
    );
}
