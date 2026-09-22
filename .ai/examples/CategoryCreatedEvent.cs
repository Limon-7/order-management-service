using Platform.Infrastructure.Core.Events;

namespace {{RootNamespace}}.Domain.Events.Category;

public sealed class CategoryCreatedEvent : Event
{
    public Guid CategoryId { get; init; }
    public string CategoryName { get; init; } = null!;
    public string CategoryCode { get; init; } = null!;
    public Guid ParentEntityId { get; init; }
    public bool IsActive { get; init; }
    public int Order { get; init; }
}
