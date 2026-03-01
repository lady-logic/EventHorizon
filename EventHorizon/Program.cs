using EventHorizon.Api.Domain.Projections;
using EventHorizon.Application.Asteroids.Queries;
using EventHorizon.Application.Interfaces;
using EventHorizon.Infrastructure;
using EventHorizon.Infrastructure.Persistence;
using JasperFx.Events;
using JasperFx.Events.Daemon;
using JasperFx.Events.Projections;
using Marten;
using MediatR;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Postgres")!);
    opts.DatabaseSchemaName = "eventhorizon";
    opts.Events.StreamIdentity = StreamIdentity.AsString;
    opts.Projections.Add<AsteroidSummaryProjection>(ProjectionLifecycle.Inline);
})
.AddAsyncDaemon(DaemonMode.HotCold)
.UseLightweightSessions()
.ApplyAllDatabaseChangesOnStartup();

builder.Services.AddHttpClient<NasaApiClient>();
builder.Services.AddScoped<IAsteroidRepository, AsteroidRepository>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(
    typeof(GetAllAsteroidSummariesQuery).Assembly));

builder.Services.AddHostedService<EventHorizon.Api.BackgroundServices.NeoWsPollingService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapGet("/", () => "EventHorizon is watching the skies... 🌌");

app.MapGet("/asteroids", async (IQuerySession session) =>
{
    var events = await session.Events.QueryAllRawEvents().ToListAsync();
    var result = events.Select(e => new
    {
        StreamId = e.StreamKey,
        EventType = e.EventTypeName,
        Timestamp = e.Timestamp,
        Data = e.Data
    });
    return Results.Ok(result);
});

app.MapGet("/asteroids/summaries", async (IMediator mediator) =>
{
    var result = await mediator.Send(new GetAllAsteroidSummariesQuery());
    return Results.Ok(result);
});

app.MapGet("/asteroids/hazardous", async (IMediator mediator) =>
{
    var result = await mediator.Send(new GetHazardousAsteroidsQuery());
    return Results.Ok(result);
});

app.Run();