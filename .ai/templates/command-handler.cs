using {{RootNamespace}}.Application.Commands.[ContextName];
using {{RootNamespace}}.Application.CommandServices.Abstractions;
using {{RootNamespace}}.Domain.Aggregates.[ContextName];
using {{RootNamespace}}.Domain.Events.ErrorEvents;
using {{RootNamespace}}.Shared.Common.Constants;
using {{RootNamespace}}.Shared.Common.Helpers;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Platform.Infrastructure.Core.Bus;
using Platform.Infrastructure.Core.Commands;
using Platform.Infrastructure.Core.Domain;

namespace {{RootNamespace}}.Application.CommandHandlers.[ContextName];

/// <summary>
/// Template for a CommandHandler.
/// Standard responsibilities:
/// 1. Implement ICommandHandlerAsync&lt;TCommand&gt;.
/// 2. Provide the FluentValidator via GetValidator().
/// 3. Inject I[ActionName]CommandService to handle cross-entity validation logic.
/// 4. Inject IAggregateRootRepository&lt;TAggregate&gt; to load/save the aggregate.
/// 5. Implement HandleAsync() containing the orchestration logic.
/// </summary>
public sealed class [ActionName]CommandHandler(
    ILogger<[ActionName]CommandHandler> logger,
    IClientNotifier notificationSender,
    IBusMessageDispatcher messageDispatcher,
    I[ActionName]CommandService commandService,
    IAggregateRootRepository<[ActionName]Aggregate> aggregateRepository)
    : ICommandHandlerAsync<[ActionName]Command>
{
    private readonly ILogger<[ActionName]CommandHandler> _logger = logger;
    private readonly IClientNotifier _notificationSender = notificationSender;
    private readonly IBusMessageDispatcher _messageDispatcher = messageDispatcher;
    private readonly I[ActionName]CommandService _commandService = commandService;
    private readonly IAggregateRootRepository<[ActionName]Aggregate> _aggregateRepository = aggregateRepository;

    protected AbstractValidator<[ActionName]Command>? GetValidator()
    {
        // Example:
        // return new [ActionName]CommandValidator();
        return null;
    }

    public async Task<CommandResponse> HandleAsync([ActionName]Command command)
    {
        _logger.LogInformation(
            "Executing [ActionName] logic. CorrelationId: {CorrelationId}",
            command.CorrelationId);

        // 1. Cross-entity validation (Database lookups via CommandService)
        // if (await _commandService.IsDuplicateNameAsync(command.ParentId, command.Name))
        // {
        //     await NotifyBusinessViolationAsync(BusinessViolationCodes.ResourceAlreadyExists, command.CorrelationId, command.UserContext);
        //     return new CommandResponse { IsSuccess = false };
        // }

        // 2. Aggregate Operations
        // var aggregate = new [ActionName]Aggregate();
        // aggregate.Create(command.ToDomainDto());

        // 3. Save
        // await _aggregateRepository.SaveAsync(aggregate);

        return new CommandResponse
        {
            IsSuccess = true
        };
    }
}
