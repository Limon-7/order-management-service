# CQRS Pattern

## Overview
This project does NOT use MediatR for CQRS. Instead, it uses custom internal platform libraries (`Platform.Infrastructure.Core`). Note the physical separation between Write (`src/Application`) and Read (`src/Read`) layers.

## Commands (Write Operations)
- **Commands**: 
  - Represent write intents.
  - Must extend `Platform.Infrastructure.Core.Commands.Command`.
  - Should only contain Data/State, no logic.
- **Command Handlers**:
  - Implement `ICommandHandlerAsync<TCommand>`.
  - Provide the FluentValidator via `GetValidator()`.
  - Return `Task<CommandResponse>`.
  - **Responsibilities**:
    1. Delegate cross-entity validation to injected CommandServices and notify violations via `NotifyBusinessViolationAsync`.
    2. Invoke Domain Aggregates.
    3. Persist state Changes using `ITransactionalRepository` and `IMongoUnitOfWork`.
    4. Orchestrate Sagas or Outbox Events via `IBusMessageDispatcher.PublishAsync`.

## Queries (Read Operations)
- **Queries**:
  - Request data models (`ViewModels`) without altering system state.
  - Handled in `src/Read/QueryHandlers`.
- **Query Handlers**:
  - Implement `IQueryHandlerAsync<TQuery, TResponse>`.
  - Query via `IReadRepository.GetItemAsync` or `GetItemsAsync`.
  - Return `Task<QueryResponse<TResponse>>`.
