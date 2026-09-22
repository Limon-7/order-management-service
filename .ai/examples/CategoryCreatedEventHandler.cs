using {{RootNamespace}}.Domain.Events.Category;
using {{RootNamespace}}.Read.ViewModels;
using {{RootNamespace}}.Shared.Common.Constants;
using Platform.Infrastructure.Core.Events;
using Platform.Infrastructure.Core.Queries;
using Microsoft.Extensions.Logging;

namespace {{RootNamespace}}.Read.EventHandlers.Category;

public sealed class CategoryCreatedEventHandler(
    IReadRepository readRepository,
    ILogger<CategoryCreatedEventHandler> logger,
    IClientNotifier clientNotifier) : IEventHandlerAsync<CategoryCreatedEvent>
{
    private readonly IReadRepository _readRepository = readRepository;
    private readonly ILogger<CategoryCreatedEventHandler> _logger = logger;
    private readonly IClientNotifier _clientNotifier = clientNotifier;

    public async Task HandleAsync(CategoryCreatedEvent @event)
    {
        var existing = await _readRepository.GetItemAsync<CategoryViewModel>(
            x => x.Id == @event.AggregateRootId);

        if (existing is not null)
        {
            _logger.LogWarning("Category already exists with Id: {Id}", @event.AggregateRootId);
            return;
        }

        var view = new CategoryViewModel { Id = @event.AggregateRootId };
        view.Map(@event);
        await _readRepository.CreateAsync(view);

        await _clientNotifier.NotifySuccessAsync(
            userContext: @event.UserContext,
            correlationId: @event.CorrelationId,
            responseValue: NotificationKeys.CategoryCreated);
    }
}
