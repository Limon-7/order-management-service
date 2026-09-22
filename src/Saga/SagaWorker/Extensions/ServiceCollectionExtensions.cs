using Microsoft.Extensions.DependencyInjection;

namespace Bits.OrderManagement.Saga.SagaWorker.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Register saga-specific services (e.g. HTTP clients, external API adapters).
    /// </summary>
    public static IServiceCollection AddSagaServices(this IServiceCollection services)
    {
        // TODO: Register saga services
        // services.AddScoped<IExternalApiService, ExternalApiService>();
        return services;
    }

    /// <summary>
    /// Configures MassTransit with the saga state machine and RabbitMQ transport.
    /// Replace with your actual saga type.
    /// </summary>
    public static IServiceCollection RegisterSagaServiceBus(
        this IServiceCollection services,
        Microsoft.Extensions.Configuration.IConfiguration configuration)
    {
        // TODO: Configure MassTransit with your saga state machine
        // services.AddMassTransit(x =>
        // {
        //     x.AddSagaStateMachine<ExampleSaga, ExampleSagaState>()
        //         .MongoDbRepository(r =>
        //         {
        //             r.Connection = configuration["MongoDb:ConnectionString"];
        //             r.DatabaseName = configuration["MongoDb:DatabaseName"];
        //         });
        //
        //     x.UsingRabbitMq((ctx, cfg) =>
        //     {
        //         cfg.Host(configuration["BusConfig:ConnectionString"]);
        //         cfg.ConfigureEndpoints(ctx);
        //     });
        // });
        return services;
    }
}
