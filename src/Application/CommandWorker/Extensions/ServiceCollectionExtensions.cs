using Microsoft.Extensions.DependencyInjection;
using Platform.Infrastructure.Core.Commands;
using Platform.Infrastructure.Core.Events;
using Scrutor;

namespace Bits.OrderManagement.Application.CommandWorker.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Auto-registers all ICommandHandlerAsync and IEventHandlerAsync implementations
    /// from the CommandHandlers assembly using Scrutor assembly scanning.
    /// Replace the anchor type with any class in your CommandHandlers project.
    /// </summary>
    public static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        // TODO: Replace ExampleCommandHandlerAnchor with any handler class in your CommandHandlers project
        // e.g. .FromAssemblyOf<CreateExampleCommandHandler>()
        services.Scan(scan => scan
            .FromAssemblyOf<object>() // ← replace 'object' with an actual handler class
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandlerAsync<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(IEventHandlerAsync<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

        return services;
    }

    /// <summary>
    /// Register application-layer command services.
    /// Add your ICommandService → implementation registrations here,
    /// or use assembly scanning similar to AddHandlers.
    /// </summary>
    public static IServiceCollection AddCommandServices(this IServiceCollection services)
    {
        // TODO: Register command services
        // services.AddScoped<IExampleService, ExampleService>();
        return services;
    }
}
