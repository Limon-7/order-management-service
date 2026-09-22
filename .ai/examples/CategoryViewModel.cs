using Platform.Infrastructure.Core.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace {{RootNamespace}}.Read.ViewModels;

[BsonIgnoreExtraElements]
public class CategoryViewModel : ViewModelBase
{
    public string CategoryName { get; set; } = null!;
    public string CategoryCode { get; set; } = null!;
    public Guid ParentEntityId { get; set; }
    public bool IsActive { get; set; } = true;
    public int Order { get; set; }

    public void Map(CategoryCreatedEvent @event)
    {
        Id = @event.AggregateRootId;
        CategoryName = @event.CategoryName;
        CategoryCode = @event.CategoryCode;
        ParentEntityId = @event.ParentEntityId;
        IsActive = @event.IsActive;
        Order = @event.Order;
    }
}
