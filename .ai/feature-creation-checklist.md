# Feature Creation Checklist

When creating a new feature/entity `[EntityName]` in `[Feature]` context, create all applicable files in this order.

> **CRITICAL**: Do NOT skip the Registration steps at the bottom. Missing registrations cause silent runtime failures.

## Step 1 — Domain Layer (`src/Domain/`)

### 1.1 Domain DTO (record)
**File**: `src/Domain/Models/[EntityName]CreationDto.cs`
- Immutable `record` type.
- Always include `UserContext` and `CorrelationId` as last params.
- Namespace: `{{RootNamespace}}.Domain.Models`

### 1.2 Aggregate Root
**File**: `src/Domain/Aggregates/[EntityName]Aggregate.cs` (or `src/Domain/Aggregates/[Feature]/[EntityName]Aggregate.cs` for complex aggregates)
- Inherit from `AggregateRoot`.
- Annotate with `[BsonIgnoreExtraElements]`.
- Private setters, state mutation via encapsulating methods only.
- Call `UpdateVersion()` after every successful state mutation.
- Namespace: `{{RootNamespace}}.Domain.Aggregates` or `{{RootNamespace}}.Domain.Aggregates.[Feature]`

### 1.3 Domain Events
**File**: `src/Domain/Events/[Feature]/[EntityName]CreatedEvent.cs`
- Extend `Platform.Infrastructure.Core.Events.Event`.
- Use `sealed class` with `init` properties.
- Carry enough data for Read-side projection building.
- Namespace: `{{RootNamespace}}.Domain.Events.[Feature]`

### 1.4 Business Rule Violation Event
**File**: `src/Domain/Events/ErrorEvents/[EntityName]BusinessViolatedEvent.cs` (if a new aggregate context)
- Extend `BusinessRuleViolatedEvent`.
- Skip if reusing an existing violation event.
- Namespace: `{{RootNamespace}}.Domain.Events.ErrorEvents`

### 1.5 Value Objects (if needed)
**File**: `src/Domain/ValueObjects/[ValueObjectName].cs`
- Use `record` types for simple value objects.
- Namespace: `{{RootNamespace}}.Domain.ValueObjects`

### 1.6 Domain Validators (if needed)
**File**: `src/Domain/Aggregates/Validators/Create[EntityName]Validator.cs`
- FluentValidation validators used inside aggregates.
- Namespace: `{{RootNamespace}}.Domain.Aggregates.Validators`

---

## Step 2 — Application Layer / Write Side (`src/Application/`)

### 2.1 Command
**File**: `src/Application/Commands/[Feature]/Create[EntityName]Command.cs`
- Extend `Platform.Infrastructure.Core.Commands.Command`.
- Use `init` properties. Data only — no logic.
- Namespace: `{{RootNamespace}}.Application.Commands.[Feature]`

### 2.2 Command Validator
**File**: `src/Application/CommandHandlers/Validators/Create[EntityName]CommandValidator.cs`
- Extend `AbstractCommandValidator<Create[EntityName]Command>`.
- Use `GetEventMessage(BusinessViolationCodes.XXX, nameof(...))` for `.WithState(...)`.
- Namespace: `{{RootNamespace}}.Application.CommandHandlers.Validators` (flat — all validators here)

### 2.3 DataMapper
**File**: `src/Application/DataMappers/[EntityName]Mapper.cs`
- Static class with extension methods: `ToDomainDto()`.
- Maps from Command → Domain DTO (record).
- Always pass `command.UserContext` and `command.CorrelationId`.
- Namespace: `{{RootNamespace}}.Application.DataMappers` (flat)

### 2.4 CommandService Interface
**File**: `src/Application/CommandServices/Abstractions/I[EntityName]CommandService.cs`
- Interface defining cross-entity validation methods.
- Common methods: `Get[ParentEntity]Async`, `Get[EntityName]Async`, `IsDuplicateNameAsync`, etc.
- Namespace: `{{RootNamespace}}.Application.CommandServices.Abstractions` (flat)

### 2.5 CommandService Implementation
**File**: `src/Application/CommandServices/[EntityName]CommandService.cs`
- Inject `IStateRepository` and/or `IAggregateRootRepository<T>`.
- Use Primary Constructor.
- Always filter with `!x.IsMarkedToDelete`.
- Namespace: `{{RootNamespace}}.Application.CommandServices` (flat)

### 2.6 Command Handler
**File**: `src/Application/CommandHandlers/[Feature]/Create[EntityName]CommandHandler.cs`
- Implement `ICommandHandlerAsync<Create[EntityName]Command>`.
- Use **Primary Constructor** for DI.
- Override or implement `GetValidator()` → return the validator (or `null`).
- Implement `ExecuteAsync()` or `HandleAsync()` with the orchestration flow.
- Implement `ValidateBusinessRulesAsync()` as a private method for existence/uniqueness checks.
- Namespace: `{{RootNamespace}}.Application.CommandHandlers.[Feature]`

---

## Step 3 — Read Layer (`src/Read/`)

### 3.1 ViewModel
**File**: `src/Read/ViewModels/[EntityName]ViewModel.cs`
- Extend `ViewModelBase`.
- Annotate with `[BsonIgnoreExtraElements]`.
- Public setters (for MongoDB deserialization and event mapping).
- Add `Map([EntityName]CreatedEvent @event)` method.
- Namespace: `{{RootNamespace}}.Read.ViewModels` (flat)

### 3.2 Event Handler (Read Projection)
**File**: `src/Read/EventHandlers/[Feature]/[EntityName]CreatedEventHandler.cs`
- Implement `IEventHandlerAsync<[EntityName]CreatedEvent>`.
- Use Primary Constructor. Inject `IReadRepository`, `ILogger`, `IClientNotifier`.
- Create/update the ViewModel via `_readRepository.CreateAsync(view)`.
- Send success notification via `_clientNotifier.NotifySuccessAsync(...)`.
- Namespace: `{{RootNamespace}}.Read.EventHandlers.[Feature]`

### 3.3 Query
**File**: `src/Read/Queries/[Feature]/Get[EntityName]Query.cs`
- Extend `Query<[EntityName]ViewModel?>`.
- Contains only filter/criteria properties.
- Namespace: `{{RootNamespace}}.Read.Queries.[Feature]`

### 3.4 Query Handler
**File**: `src/Read/QueryHandlers/[Feature]/Get[EntityName]QueryHandler.cs`
- Implement `IQueryHandlerAsync<Get[EntityName]Query, [EntityName]ViewModel?>`.
- Inject `IReadRepository` and `ILogger`.
- Return `QueryResponse<[EntityName]ViewModel?>`.
- Namespace: `{{RootNamespace}}.Read.QueryHandlers.[Feature]`

---

## Step 4 — Shared Layer (`src/Shared/`)

### 4.1 BusinessViolationCodes
**File**: `src/Shared/Common/Constants/BusinessViolationCodes.cs`
- **MODIFY** (not create): Add new violation code constants for the entity.
- Pattern: `public const string [EntityName]NotFound = nameof([EntityName]NotFound);`

### 4.2 NotificationKeys
**File**: `src/Shared/Common/Constants/NotificationKeys.cs`
- **MODIFY** (not create): Add new notification keys.
- Pattern: `public const string [EntityName]Created = nameof([EntityName]Created);`

### 4.3 SharedDto (if external DTOs needed)
**File**: `src/Shared/SharedDto/[Feature]/[DtoName].cs`
- Used for API-facing DTOs shared between layers.
- Namespace: `{{RootNamespace}}.Shared.SharedDto.[Feature]`

---

## Step 5 — Registration (⚠️ CRITICAL)

### 5.1 Register Command Consumer
**File**: `src/Application/CommandWorker/Program.cs`
- **MODIFY**: Add inside the `AddServiceBusProvider` lambda:
```csharp
e.Consumer(() => new CommandConsumerAdapter<Create[EntityName]Command>());
```
- Add the `using` for the command namespace.

### 5.2 Register CommandService
**File**: `src/Application/CommandServices/ServiceCollectionExtensions.cs`
- **MODIFY**: Add:
```csharp
services.AddScoped<I[EntityName]CommandService, [EntityName]CommandService>();
```

### 5.3 Register Event Consumer (Read Side)
**File**: `src/Read/EventWorker/Program.cs`
- **MODIFY**: Add inside the `AddServiceBusProvider` lambda block:
```csharp
e.Consumer(() => new EventConsumerAdapter<[EntityName]CreatedEvent>());
```
- Add the `using` for the domain events namespace (e.g. `{{RootNamespace}}.Domain.Events.[Feature]`).

---

## Quick Reference — File Count per Feature

| Type | Min Files | Notes |
|------|-----------|-------|
| Simple CRUD (Create only) | ~14 | All steps above |
| Full CRUD (Create/Update/Toggle) | ~20 | Add Update command/handler/mapper/event/eventhandler |
| Saga-based (complex workflows) | ~25+ | Add saga state machine, saga service, saga events |
