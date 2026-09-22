using Platform.Infrastructure.Core.Commands;

namespace {{RootNamespace}}.Application.Commands.Category;

public class CreateCategoryCommand : Command
{
    public Guid CategoryId { get; init; }
    public string CategoryName { get; init; } = null!;
    public string CategoryCode { get; init; } = null!;
    public Guid ParentEntityId { get; init; }
    public bool IsActive { get; init; } = true;
}
