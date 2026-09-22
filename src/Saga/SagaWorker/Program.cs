using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Platform.Infrastructure.Core.Extensions;
using Platform.Infrastructure.Repository.MongoDb;
using Bits.OrderManagement.Saga.SagaWorker.Extensions;

// ── MongoDB serializer setup ─────────────────────────────────────────────────
var objectSerializer = new ObjectSerializer(ObjectSerializer.AllAllowedTypes);
BsonSerializer.RegisterSerializer(objectSerializer);
BsonSerializer.RegisterSerializer(new DecimalSerializer(BsonType.Decimal128));

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

// ── Environment-aware configuration ─────────────────────────────────────────
var currentEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var currentAppSettingsFileName = string.IsNullOrWhiteSpace(currentEnvironment)
    ? "appsettings.json"
    : $"appsettings.{currentEnvironment}.json";

builder.Configuration.AddJsonFile(currentAppSettingsFileName, optional: false, reloadOnChange: false);

// ── Core services ─────────────────────────────────────────────────────────────
builder.Services.AddInMemoryBusServices();
builder.Services.AddStateRepository();

// ── Register saga services ────────────────────────────────────────────────────
builder.Services.AddSagaServices();

// ── Register saga state machine + RabbitMQ transport ─────────────────────────
builder.Services.RegisterSagaServiceBus(builder.Configuration);

// ── Build & run ───────────────────────────────────────────────────────────────
var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("OrderManagement SagaWorker started successfully.");

app.Run();
