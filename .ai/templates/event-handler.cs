using {{RootNamespace}}.Domain.Events.[ContextName];
using {{RootNamespace}}.Shared.Common.Constants;
using {{RootNamespace}}.Shared.Common.Helpers;
using Microsoft.Extensions.Logging;
using Platform.Infrastructure.Core.Events;
using Platform.Infrastructure.Core.Queries;

namespace {{RootNamespace}}.Read.EventHandlers.[ContextName];

/// <summary>
/// Template for an Event Handler (Read Projections).
/// Standard responsibilities:
/// 1. Catch Domain Events raised from the Write side.
/// 2. Build or update Read-side ViewModels using IReadRepository.
/// 3. Safely handle updates inside a try-catch block.
/// 4. Optionally broadcast generic success notifications via IClientNotifier.
/// </summary>
public sealed class [EventName]EventHandler(
    IReadRepository readRepository,
    ILogger<[EventName]EventHandler> logger,
    IClientNotifier clientNotifier) : IEventHandlerAsync<[EventName]Event>
{
    private readonly IReadRepository _readRepository = readRepository;
    private readonly ILogger<[EventName]EventHandler> _logger = logger;
    private readonly IClientNotifier _clientNotifier = clientNotifier;

    public async Task HandleAsync([EventName]Event @event)
    {
        _logger.LogInformation("Handling [EventName]Event for CorrelationId: {CorrelationId}", @event.CorrelationId);

        try
        {
            await HandleProjectionUpdateAsync(@event);
            
            // Optional: Send Notifications to connected clients
            await SendNotificationsAsync(@event);

            _logger.LogInformation("Successfully completed handling [EventName]Event for CorrelationId: {CorrelationId}", @event.CorrelationId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling [EventName]Event for CorrelationId: {CorrelationId}", @event.CorrelationId);
            throw;
        }
    }

    private async Task HandleProjectionUpdateAsync([EventName]Event @event)
    {
        // Example: creating a read-side projection mapping
        // var view = new [ViewModelName]();
        // view.Map(@event);
        // await _readRepository.CreateAsync(view);
        //
        // _logger.LogInformation("Added [ViewModelName] View Data for CorrelationId: {CorrelationId}", @event.CorrelationId);
        
        await Task.CompletedTask;
    }

    private async Task SendNotificationsAsync([EventName]Event @event)
    {
        _logger.LogInformation("Sending successful notifications for [EventName]Event. CorrelationId: {CorrelationId}", @event.CorrelationId);

        // Notify specific user who initiated the command
        // await _clientNotifier.NotifySuccessAsync(
        //     userContext: @event.UserContext,
        //     correlationId: @event.CorrelationId,
        //     responseValue: NotificationKeys.[SpecificNotificationKey]);

        // Broadcast to all relevant online users
        // await _clientNotifier.BroadCastAsync(
        //     userContext: @event.UserContext,
        //     correlationId: @event.CorrelationId,
        //     responseValue: NotificationKeys.[SpecificNotificationKey]);
    }
}
