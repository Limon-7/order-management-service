using {{RootNamespace}}.Application.CommandServices.Abstractions;
using {{RootNamespace}}.Domain.Aggregates;
using Platform.Infrastructure.Core.Domain;

namespace {{RootNamespace}}.Application.CommandServices;

public class CategoryCommandService(
    IStateRepository stateRepository,
    IAggregateRootRepository<CategoryAggregate> categoryRepository) : ICategoryCommandService
{
    private readonly IStateRepository _stateRepository = stateRepository;
    private readonly IAggregateRootRepository<CategoryAggregate> _categoryRepository = categoryRepository;

    public async Task<CategoryAggregate?> GetCategoryAsync(Guid categoryId)
    {
        return await _categoryRepository.GetByFilterAsync(
            x => !x.IsMarkedToDelete && x.Id == categoryId);
    }

    public async Task<bool> IsDuplicateNameAsync(Guid parentEntityId, string name, Guid? currentCategoryId = null)
    {
        var nameIdx = name.Trim().ToLowerInvariant().Replace(" ", string.Empty);

        var duplicate = await _categoryRepository.GetByFilterAsync(x =>
            !x.IsMarkedToDelete &&
            x.ParentEntityId == parentEntityId &&
            x.Id != currentCategoryId &&
            x.CategoryNameIdx == nameIdx);

        return duplicate is not null;
    }

    public async Task<bool> IsDuplicateCodeAsync(Guid parentEntityId, string code, Guid? currentCategoryId = null)
    {
        var cleanCode = code.Trim().ToUpperInvariant().Replace(" ", string.Empty);

        var duplicate = await _categoryRepository.GetByFilterAsync(x =>
            !x.IsMarkedToDelete &&
            x.ParentEntityId == parentEntityId &&
            x.Id != currentCategoryId &&
            x.CategoryCode == cleanCode);

        return duplicate is not null;
    }

    public async Task<int> GetNextOrderAsync(Guid parentEntityId)
    {
        var items = await _stateRepository.GetItemsAsync<CategoryAggregate>(
            x => !x.IsMarkedToDelete && x.ParentEntityId == parentEntityId);
        return items.Count != 0 ? items.Max(t => t.Order) + 1 : 1;
    }
}
