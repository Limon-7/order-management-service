using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Platform.Infrastructure.Core.Extensions;
using Platform.Infrastructure.MassTransit.Bus.RabbitMQ;
using Platform.Infrastructure.MassTransit.Bus.RabbitMQ.Extensions;
using Platform.Infrastructure.Repository.MongoDb;
using Bits.OrderManagement.Read.EventWorker.Extensions;

// ── MongoDB serializer setup ─────────────────────────────────────────────────
var objectSerializer = new ObjectSerializer(ObjectSerializer.AllAllowedTypes);
BsonSerializer.RegisterSerializer(objectSerializer);
BsonSerializer.RegisterSerializer(new DecimalSerializer(BsonType.Decimal128));

var builder = Host.CreateApplicationBuilder(args);

// ── Environment-aware configuration ─────────────────────────────────────────
var currentEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var currentAppSettingsFileName = string.IsNullOrWhiteSpace(currentEnvironment)
    ? "appsettings.json"
    : $"appsettings.{currentEnvironment}.json";

builder.Configuration.AddJsonFile(currentAppSettingsFileName, optional: false, reloadOnChange: false);

// ── In-process bus ────────────────────────────────────────────────────────────
builder.Services.AddInMemoryBusServices();

// ── Repositories ──────────────────────────────────────────────────────────────
builder.Services.AddMemoryCache();
builder.Services.AddReadRepository();

// ── Event handlers (auto-registered via Scrutor) ─────────────────────────────
builder.Services.AddHandlers();

// ── RabbitMQ service bus ──────────────────────────────────────────────────────
builder.Services.AddServiceBusProvider(builder.Configuration, e =>
{
    // Register event consumers here:
    // e.Consumer(() => new EventConsumerAdapter<ExampleCreatedEvent>());
});

// ── Build & run ───────────────────────────────────────────────────────────────
var app = builder.Build();

var hostLifeTime = app.Services.GetRequiredService<IHostApplicationLifetime>();
hostLifeTime.ApplicationStarted.Register(() =>
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("OrderManagement EventWorker started successfully.");
});

app.Run();
