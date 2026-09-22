using {{RootNamespace}}.Domain.Events.[ContextName];
using {{RootNamespace}}.Domain.Events.ErrorEvents;
using {{RootNamespace}}.Shared.Common.Constants;
using {{RootNamespace}}.Shared.SharedDto.[ContextName];
using MongoDB.Bson.Serialization.Attributes;
using Platform.Infrastructure.Common.Security;
using Platform.Infrastructure.Core.Domain;
using Platform.Infrastructure.Core.Events;

namespace {{RootNamespace}}.Domain.Aggregates.[ContextName];

/// <summary>
/// Template for an Aggregate Root.
/// Standard responsibilities:
/// 1. Extend Platform.Infrastructure.Core.Domain.AggregateRoot.
/// 2. Use [BsonIgnoreExtraElements] for MongoDB compatibility.
/// 3. Contain entirely private setters for properties.
/// 4. Mutate state only through encapsulating methods (e.g., Create[AggregateName], Update[Something]).
/// 5. Emit Domain Events (.AddDomainEvent) or Violation Events (.AddBusinessRuleViolationEvent).
/// 6. Call UpdateVersion() after every successful state mutation.
/// </summary>
[BsonIgnoreExtraElements]
public class [AggregateName]Aggregate : AggregateRoot
{
    // Private setters to enforce invariants
    public string Name { get; private set; } = default!;
    
    // Optional: Private collections for children entities/Value Objects
    // private List<ChildEntity> _children = [];
    // public IReadOnlyCollection<ChildEntity> Children => _children.AsReadOnly();

    public void Create[AggregateName](
        [ContextName]CreationDto input, 
        Guid correlationId, 
        UserContext userContext)
    {
        // Example Validation checking
        // var validationResult = new [AggregateName]Validator().Validate(input);
        // if (validationResult.IsValid is false)
        // {
        //     AddBusinessRuleViolationEvent(
        //         validationResult: validationResult,
        //         userContext: userContext,
        //         correlationId: correlationId);
        //     return;
        // }

        Id = input.Id;
        Name = input.Name;
        
        // Orchestrate Side Effects via Domain Events
        // var @event = new [AggregateName]CreatedEvent 
        // { 
        //     Id = this.Id, 
        //     Name = this.Name,
        //     CorrelationId = correlationId,
        //     UserContext = userContext 
        // };
        // AddDomainEvent(@event);
        
        UpdateVersion();
    }
}
