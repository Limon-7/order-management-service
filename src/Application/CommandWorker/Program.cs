using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Platform.Infrastructure.Core.Extensions;
using Platform.Infrastructure.MassTransit.Bus.RabbitMQ.Extensions;
using Platform.Infrastructure.Repository.MongoDb;
using Bits.OrderManagement.Application.CommandWorker.Extensions;

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

// ── In-process bus (for domain event dispatching) ───────────────────────────
builder.Services.AddInMemoryBusServices();

// ── Repositories ─────────────────────────────────────────────────────────────
builder.Services.AddStateRepository();
builder.Services.AddReadRepository();
builder.Services.AddAggregateRootRepository();

// ── Optional: Redis cache ─────────────────────────────────────────────────────
// builder.Services.UseRedisCache();
// builder.Services.AddMemoryCache();

// ── Command handlers (auto-registered via Scrutor) ───────────────────────────
builder.Services.AddHandlers();

// ── Command services ──────────────────────────────────────────────────────────
builder.Services.AddCommandServices();

// ── Optional: Quartz.NET scheduler ───────────────────────────────────────────
// builder.Services.AddQuartz(q =>
// {
//     q.UseSimpleTypeLoader();
//     q.UseInMemoryStore();
//     q.UseDefaultThreadPool(tp => tp.MaxConcurrency = 5);
// });
// builder.Services.AddQuartzHostedService(opt => opt.WaitForJobsToComplete = true);

// ── RabbitMQ service bus ──────────────────────────────────────────────────────
builder.Services.AddServiceBusProvider(builder.Configuration, e =>
{
    // Register command consumers here:
    // e.Consumer(() => new CommandConsumerAdapter<CreateExampleCommand>());
});

// ── Build & run ───────────────────────────────────────────────────────────────
var app = builder.Build();

var hostLifeTime = app.Services.GetRequiredService<IHostApplicationLifetime>();
hostLifeTime.ApplicationStarted.Register(() =>
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("OrderManagement CommandWorker started successfully.");
});

app.Run();
