using System.Text.Json;

namespace EventHorizon.Infrastructure
{
    public class NasaApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly ILogger<NasaApiClient> _logger;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public NasaApiClient(HttpClient httpClient, IConfiguration configuration, ILogger<NasaApiClient> logger)
        {
            _httpClient = httpClient;
            _apiKey = configuration["Nasa:ApiKey"] ?? "DEMO_KEY";
            _logger = logger;
        }

        public async Task<NeoWsResponse?> GetNearEarthObjectsAsync(DateOnly startDate, DateOnly endDate)
        {
            var url = $"https://api.nasa.gov/neo/rest/v1/feed" +
                      $"?start_date={startDate:yyyy-MM-dd}" +
                      $"&end_date={endDate:yyyy-MM-dd}" +
                      $"&api_key={_apiKey}";

            _logger.LogInformation("Fetching NEOs from NASA API for {StartDate} to {EndDate}", startDate, endDate);

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<NeoWsResponse>(content, JsonOptions);
        }
    }
}
