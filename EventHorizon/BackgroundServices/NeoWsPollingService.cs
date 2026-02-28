using EventHorizon.Domain.Events;
using EventHorizon.Infrastructure;
using Marten;
using System.Globalization;

namespace EventHorizon.BackgroundServices
{
    public class NeoWsPollingService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<NeoWsPollingService> _logger;

        public NeoWsPollingService(IServiceScopeFactory scopeFactory, ILogger<NeoWsPollingService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("EventHorizon is watching the skies...");

            await PollNasaApiAsync();
        }

        private async Task PollNasaApiAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            var nasaClient = scope.ServiceProvider.GetRequiredService<NasaApiClient>();
            var documentStore = scope.ServiceProvider.GetRequiredService<IDocumentStore>();

            var startDate = DateOnly.FromDateTime(DateTime.Today);
            var endDate = startDate.AddDays(7);

            var response = await nasaClient.GetNearEarthObjectsAsync(startDate, endDate);

            if (response is null)
            {
                _logger.LogWarning("NASA API returned no data");
                return;
            }

            await using var session = documentStore.LightweightSession();

            foreach (var (date, objects) in response.NearEarthObjects)
            {
                foreach (var neo in objects)
                {
                    _logger.LogInformation("Processing {Name}", neo.Name);

                    session.Events.Append(
                        neo.Id,
                        new NearEarthObjectDetected(
                            neo.Id,
                            neo.Name,
                            neo.EstimatedDiameter.Kilometers.Min,
                            neo.EstimatedDiameter.Kilometers.Max,
                            neo.IsPotentiallyHazardous,
                            DateTimeOffset.UtcNow
                        )
                    );

                    var approach = neo.CloseApproachData.FirstOrDefault();
                    if (approach is not null)
                    {
                        session.Events.Append(
                            neo.Id,
                            new ApproachDataUpdated(
                                neo.Id,
                                double.Parse(approach.MissDistance.Kilometers, CultureInfo.InvariantCulture),
                                double.Parse(approach.RelativeVelocity.KilometersPerSecond, CultureInfo.InvariantCulture),
                                DateTimeOffset.Parse(approach.CloseApproachDate)
                            )
                        );
                    }
                }
            }

            await session.SaveChangesAsync();
            _logger.LogInformation("Saved all NEO events to Marten");
        }
    }
}
