using EventHorizon.BackgroundServices;
using EventHorizon.Infrastructure;
using JasperFx.Events;
using JasperFx.Events.Daemon;
using Marten;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Postgres")!);
    opts.DatabaseSchemaName = "eventhorizon";
    opts.Events.StreamIdentity = StreamIdentity.AsString;
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

app.Run();