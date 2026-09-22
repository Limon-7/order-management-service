# Command Handler Pattern Implementation Guide

This document outlines the standard pattern for implementing Command Handlers, Services, and Validators in this project.

## 1. Command Definition
Commands should be immutable and use modern C# features.
- Inherit from `Command`.
- Use `required` and `init` for properties.
- Located in `src/Application/Commands/[Feature]/`.

```csharp
public class MyCommand : Command
{
    public required Guid EntityId { get; init; }
    public string? Notes { get; init; }
}
```

## 2. Command Validator
Centralize basic input validation using FluentValidation.
- Inherit from `AbstractCommandValidator<TCommand>`.
- Located in `src/Application/CommandHandlers/Validators/`.

```csharp
public class MyCommandValidator : AbstractCommandValidator<MyCommand>
{
    public MyCommandValidator()
    {
        RuleFor(x => x.EntityId).NotEmpty();
    }
}
```

## 3. Command Service (Service Layer)
Encapsulate complex business logic, aggregate retrieval, and state-dependent checks.
- Define an interface in `src/Application/CommandServices/Abstractions/`.
- Implement in `src/Application/CommandServices/`.
- Inject into Command Handlers.

```csharp
public interface IMyCommandService
{
    Task<MyAggregate?> GetEntityAsync(Guid id);
    Task ProcessComplexLogicAsync(MyAggregate entity, ...);
}
```

## 4. Command Handler
Handlers should focus on orchestrating the flow, delegating logic to services.
- Implement `ICommandHandlerAsync<TCommand>`.
- Use **Primary Constructor** for dependencies.
- Override or implement the execution method.
- Use `GetValidator()` to return the command-specific validator.
- Implement `ValidateBusinessRulesAsync` for business-level checks (e.g., existence, status guards).
- Located in `src/Application/CommandHandlers/[Feature]/`.

```csharp
public sealed class MyCommandHandler(
    ILogger<MyCommandHandler> logger,
    IMyCommandService myService,
    IClientNotifier clientNotifier) : 
    ICommandHandlerAsync<MyCommand>
{
    public async Task<CommandResponse> HandleAsync(MyCommand command)
    {
        var (isSuccess, entity) = await ValidateBusinessRulesAsync(command);
        if (!isSuccess || entity is null) return new() { IsSuccess = false };

        await myService.ProcessComplexLogicAsync(entity, ...);
        
        // Publish events, notify success, etc.
        return new() { IsSuccess = true };
    }

    private async Task<(bool IsSuccess, MyAggregate? Entity)> ValidateBusinessRulesAsync(MyCommand command)
    {
        var entity = await myService.GetEntityAsync(command.EntityId);
        if (entity is null)
        {
            await NotifyBusinessViolationAsync(BusinessViolationCodes.EntityNotFound, ...);
            return (false, null);
        }
        return (true, entity);
    }
}
```

## Summary of Rules
1. **No direct repository calls** in handlers for complex state; use a Service.
2. **No manual validation** of simple inputs; use `GetValidator()`.
3. **Primary Constructors** only for dependency injection.
4. **Always implement `ICommandHandlerAsync<TCommand>`** to standardize responses and violation notifications.
