using EventHorizon.Api.Domain.Projections;
using EventHorizon.Api.Domain.ReadModels;
using EventHorizon.BackgroundServices;
using EventHorizon.Infrastructure;
using JasperFx.Events;
using JasperFx.Events.Daemon;
using JasperFx.Events.Projections;
using Marten;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Postgres")!);
    opts.DatabaseSchemaName = "eventhorizon";
    opts.Events.StreamIdentity = StreamIdentity.AsString;
    opts.Projections.Add<AsteroidSummaryProjection>(ProjectionLifecycle.Inline);
}).AddAsyncDaemon(DaemonMode.HotCold) 
  .UseLightweightSessions();

builder.Services.AddHttpClient<NasaApiClient>();
builder.Services.AddHostedService<NeoWsPollingService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapGet("/", () => "EventHorizon is watching the skies...");
app.MapGet("/asteroids", async (IQuerySession session) =>
{
    var events = await session.Events.QueryAllRawEvents()
        .ToListAsync();

    var result = events.Select(e => new
    {
        StreamId = e.StreamKey,
        EventType = e.EventTypeName,
        Timestamp = e.Timestamp,
        Data = e.Data
    });

    return Results.Ok(result);
});
app.MapGet("/asteroids/summaries", async (IQuerySession session) =>
{
    var summaries = await session.Query<AsteroidSummary>()
        .ToListAsync();
    return Results.Ok(summaries);
});

app.MapGet("/asteroids/hazardous", async (IQuerySession session) =>
{
    var hazardous = await session.Query<AsteroidSummary>()
        .Where(a => a.IsPotentiallyHazardous)
        .ToListAsync();
    return Results.Ok(hazardous);
});

app.Run();