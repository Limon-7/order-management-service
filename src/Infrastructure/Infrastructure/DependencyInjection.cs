using Microsoft.Extensions.DependencyInjection;
using Platform.Infrastructure.Repository.MongoDb;

namespace Bits.OrderManagement.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Registers infrastructure services for the write (state) side.
    /// Call this from CommandWorker and SagaWorker Program.cs.
    /// </summary>
    public static IServiceCollection AddInfrastructureForState(this IServiceCollection services)
    {
        // Add additional infrastructure registrations here
        // e.g. services.AddScoped<IMongoUnitOfWork, MongoUnitOfWork>();
        return services;
    }
}
