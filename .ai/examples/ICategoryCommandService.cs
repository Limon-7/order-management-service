using {{RootNamespace}}.Domain.Aggregates;

namespace {{RootNamespace}}.Application.CommandServices.Abstractions;

public interface ICategoryCommandService
{
    Task<CategoryAggregate?> GetCategoryAsync(Guid categoryId);
    Task<bool> IsDuplicateNameAsync(Guid parentEntityId, string name, Guid? currentCategoryId = null);
    Task<bool> IsDuplicateCodeAsync(Guid parentEntityId, string code, Guid? currentCategoryId = null);
    Task<int> GetNextOrderAsync(Guid parentEntityId);
}
