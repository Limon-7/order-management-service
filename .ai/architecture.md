# Project Architecture

This project follows Domain Driven Design and CQRS with strict physical separation.

## Layers

- Api
- Application
- Domain
- Infrastructure
- Read
- Shared

## Responsibilities

Api
- Handles HTTP requests and integrates with the Application and Read layers.

Application (Write Side)
- Contains Commands and Command Handlers implementing `ICommandHandlerAsync<TCommand>`.
- Contains Command Services encapsulating cross-entity database validations (e.g. uniqueness checks).
- Validates Commands (e.g., using FluentValidation via `GetValidator()`).
- Dispatches Bus Messages and handles Command execution.

Read (Read Side)
- Contains Queries, ViewModels, and Query Handlers (`IQueryHandlerAsync<TQuery, TRes>`).
- Subscribes to Events (`EventHandlers` / `EventWorker`) to update Read Projections.

Domain
- Contains Business Logic encapsulated into Aggregate Roots (inheriting `AggregateRoot`).
- Defines Value Objects, Exceptions, and Domain Events.
- Aggregates handle their own invariant rules and validation logic.

Infrastructure
- Implements Write repositories via `ITransactionalRepository` and Unit of Work (`IMongoUnitOfWork`).
- Integrates MongoDB via Database Contexts.

Shared
- Core Kernel patterns, DTOs (`SharedDto`), structured enums (`Enumeration`).
