using Microsoft.Extensions.DependencyInjection;
using Platform.Infrastructure.Core.Events;
using Scrutor;

namespace Bits.OrderManagement.Read.EventWorker.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Auto-registers all IEventHandlerAsync implementations from the EventHandlers assembly.
    /// Replace the anchor type with any handler class in your EventHandlers project.
    /// </summary>
    public static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        // TODO: Replace 'object' with an actual event handler class from EventHandlers project
        // e.g. .FromAssemblyOf<ExampleCreatedEventHandler>()
        services.Scan(scan => scan
            .FromAssemblyOf<object>() // ← replace with actual handler class
            .AddClasses(classes => classes.AssignableTo(typeof(IEventHandlerAsync<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

        return services;
    }
}
