# DI Registration Guide

> **⚠️ WARNING**: Forgetting these registrations is the #1 cause of runtime errors. Always complete these steps after creating new Commands or Services.

## 1. Registering a New Command Consumer

When you create a new Command (e.g., `Create[EntityName]Command`), you MUST register it in the message bus.

**File**: `src/Application/CommandWorker/Program.cs`

1. Add the `using` statement for the command namespace:
```csharp
using {{RootNamespace}}.Application.Commands.[Feature];
```

2. Add the consumer registration inside the `AddServiceBusProvider` lambda block:
```csharp
e.Consumer(() => new CommandConsumerAdapter<Create[EntityName]Command>());
```

3. Place it in the appropriate section (commands are loosely grouped by feature).

### Example
```csharp
// Inside builder.Services.AddServiceBusProvider(builder.Configuration, e => { ... })
e.Consumer(() => new CommandConsumerAdapter<CreateEntityCommand>());
e.Consumer(() => new CommandConsumerAdapter<UpdateEntityCommand>());
e.Consumer(() => new CommandConsumerAdapter<ToggleEntityStatusCommand>());
```

---

## 2. Registering a New CommandService

When you create a new CommandService (interface + implementation), register it in DI.

**File**: `src/Application/CommandServices/ServiceCollectionExtensions.cs`

Add inside the `AddCommandServices` method:
```csharp
services.AddScoped<I[EntityName]CommandService, [EntityName]CommandService>();
```

### Example
```csharp
public static IServiceCollection AddCommandServices(this IServiceCollection services)
{
    // ... existing registrations ...
    services.AddScoped<IEntityCommandService, EntityCommandService>();
    // Add your new service here
    return services;
}
```

---

## 3. Registering a New Event Consumer (Read Side)

When you create a new domain event (e.g., `[EntityName]CreatedEvent`) and its corresponding read-side `IEventHandlerAsync<[EntityName]CreatedEvent>`, you MUST register the event with the service bus provider in the Event Worker project so that it is subscribed to and processed.

**File**: `src/Read/EventWorker/Program.cs`

1. Add the `using` statement for the domain events namespace if not already present:
```csharp
using {{RootNamespace}}.Domain.Events.[Feature];
```

2. Add the consumer registration inside the `AddServiceBusProvider` lambda block:
```csharp
e.Consumer(() => new EventConsumerAdapter<[EntityName]CreatedEvent>());
```

### Example
```csharp
// Inside builder.Services.AddServiceBusProvider(builder.Configuration, e => { ... }) in src/Read/EventWorker/Program.cs
e.Consumer(() => new EventConsumerAdapter<EntityCreatedEvent>());
e.Consumer(() => new EventConsumerAdapter<EntityUpdatedEvent>());
```

---

## 4. Platform Auto-Discovery (No Manual Registration Needed)

The following are auto-discovered by the Platform libraries via assembly scanning:
- **Command Handlers** (`ICommandHandlerAsync<T>`) — discovered automatically
- **Event Handlers** (`IEventHandlerAsync<T>`) — discovered automatically (but note: the event itself MUST be registered in `src/Read/EventWorker/Program.cs` as shown in Section 3)
- **Query Handlers** (`IQueryHandlerAsync<TQuery, TResponse>`) — discovered automatically

You do NOT need to manually register these.

---

## 5. Repository Services (Already Registered)

These are registered once in `Program.cs` and are available everywhere:
- `IStateRepository` — via `builder.Services.AddStateRepository()`
- `IReadRepository` — via `builder.Services.AddReadRepository()`
- `IAggregateRootRepository<T>` — via `builder.Services.AddAggregateRootRepository()`
- `IClientNotifier` — via `builder.Services.AddScoped<IClientNotifier, ClientNotifier>()`

---

## Checklist

When adding a new feature, verify:
- [ ] Command consumer registered in `CommandWorker/Program.cs`
- [ ] CommandService registered in `ServiceCollectionExtensions.cs`
- [ ] Event consumer registered in `EventWorker/Program.cs` (for all read-side event handlers)
- [ ] `using` statements added for new namespaces
