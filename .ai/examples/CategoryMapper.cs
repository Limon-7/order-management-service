using {{RootNamespace}}.Application.Commands.Category;
using {{RootNamespace}}.Domain.Models;

namespace {{RootNamespace}}.Application.DataMappers;

public static class CategoryMapper
{
    public static CategoryCreationDto ToDomainDto(this CreateCategoryCommand command, int order)
    {
        return new CategoryCreationDto(
            CategoryId: command.CategoryId,
            CategoryName: command.CategoryName,
            CategoryCode: command.CategoryCode,
            ParentEntityId: command.ParentEntityId,
            IsActive: command.IsActive,
            Order: order,
            UserContext: command.UserContext,
            CorrelationId: command.CorrelationId);
    }
}
