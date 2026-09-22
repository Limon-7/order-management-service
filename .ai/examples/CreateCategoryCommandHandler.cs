using {{RootNamespace}}.Application.CommandServices.Abstractions;
using {{RootNamespace}}.Application.Commands.Category;
using {{RootNamespace}}.Application.DataMappers;
using {{RootNamespace}}.Domain.Aggregates;
using {{RootNamespace}}.Domain.Events.ErrorEvents;
using {{RootNamespace}}.Shared.Common.Constants;
using Microsoft.Extensions.Logging;
using Platform.Infrastructure.Core.Commands;
using Platform.Infrastructure.Core.Domain;
using FluentValidation;

namespace {{RootNamespace}}.Application.CommandHandlers.Category;

public sealed class CreateCategoryCommandHandler(
    IClientNotifier clientNotifier,
    ILogger<CreateCategoryCommandHandler> logger,
    ICategoryCommandService categoryCommandService,
    IAggregateRootRepository<CategoryAggregate> categoryRepository)
    : ICommandHandlerAsync<CreateCategoryCommand>
{
    private readonly ICategoryCommandService _categoryCommandService = categoryCommandService;
    private readonly IAggregateRootRepository<CategoryAggregate> _categoryRepository = categoryRepository;

    public async Task<CommandResponse> HandleAsync(CreateCategoryCommand command)
    {
        var isValid = await ValidateBusinessRulesAsync(command);
        if (!isValid)
        {
            return new() { IsSuccess = false };
        }

        var nextOrder = await _categoryCommandService.GetNextOrderAsync(command.ParentEntityId);

        CategoryAggregate category = new();
        category.Create(command.ToDomainDto(nextOrder));

        await _categoryRepository.SaveAsync(category);

        return new() { IsSuccess = !category.Events.OfType<CategoryBusinessViolatedEvent>().Any() };
    }

    private async Task<bool> ValidateBusinessRulesAsync(CreateCategoryCommand command)
    {
        if (await _categoryCommandService.GetCategoryAsync(command.CategoryId) is not null)
        {
            await clientNotifier.NotifyBusinessViolationAsync(
                messageCode: BusinessViolationCodes.CategoryAlreadyExists,
                correlationId: command.CorrelationId,
                userContext: command.UserContext);
            return false;
        }

        if (await _categoryCommandService.IsDuplicateNameAsync(command.ParentEntityId, command.CategoryName))
        {
            await clientNotifier.NotifyBusinessViolationAsync(
                messageCode: BusinessViolationCodes.CategoryNameAlreadyExists,
                correlationId: command.CorrelationId,
                userContext: command.UserContext);
            return false;
        }

        if (await _categoryCommandService.IsDuplicateCodeAsync(command.ParentEntityId, command.CategoryCode))
        {
            await clientNotifier.NotifyBusinessViolationAsync(
                messageCode: BusinessViolationCodes.CategoryCodeAlreadyExists,
                correlationId: command.CorrelationId,
                userContext: command.UserContext);
            return false;
        }

        return true;
    }
}
