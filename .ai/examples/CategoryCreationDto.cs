using Platform.Infrastructure.Common.Security;

namespace {{RootNamespace}}.Domain.Models;

public record CategoryCreationDto(
    Guid CategoryId,
    string CategoryName,
    string CategoryCode,
    Guid ParentEntityId,
    bool IsActive,
    int Order,
    UserContext UserContext,
    Guid CorrelationId,
    Guid? ScopeCorrelationId = null);
