using System.Text.Json.Serialization;

namespace GeoapifyImporter.Models;

public class GeoapifyLocality
{
    public string Name { get; set; } = string.Empty;

    public GeoapifyAddress? Address { get; set; }

    public string? Type { get; set; }

    [JsonPropertyName("osm_id")]
    public long OsmId { get; set; }
}

public class GeoapifyAddress
{
    public string? Country { get; set; }

    [JsonPropertyName("country_code")]
    public string? CountryCode { get; set; }

    public string? State { get; set; }

    [JsonPropertyName("state_district")]
    public string? StateDistrict { get; set; }

    public string? County { get; set; }

    public string? City { get; set; }

    public string? Town { get; set; }

    public string? Village { get; set; }

    public string? Hamlet { get; set; }

    public string? Municipality { get; set; }
}