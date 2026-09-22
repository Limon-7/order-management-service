using Platform.Infrastructure.Core.Events;

namespace {{RootNamespace}}.Domain.Events.ErrorEvents;

/// <summary>
/// Template for a Business Rule Violated Event per aggregate context.
/// Each aggregate context should have its own violation event.
/// Extends Platform.Infrastructure.Core.Events.BusinessRuleViolatedEvent.
/// Used as the TViolationEvent type parameter in command handlers.
/// </summary>
public class [EntityName]BusinessViolatedEvent : BusinessRuleViolatedEvent
{
    public string ActionName { get; init; } = null!;
}
