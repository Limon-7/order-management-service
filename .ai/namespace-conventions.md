# Namespace Conventions

All namespaces follow the `{{RootNamespace}}.*` root. Below are the exact patterns per layer.

> **Rule**: Always match the exact namespace pattern. Do NOT invent new sub-namespaces.

## Application Layer (Write Side)

| Artifact | Namespace | Folder Structure |
|----------|-----------|------------------|
| Commands | `{{RootNamespace}}.Application.Commands.[Feature]` | Per-feature subfolder |
| Command Handlers | `{{RootNamespace}}.Application.CommandHandlers.[Feature]` | Per-feature subfolder |
| Command Validators | `{{RootNamespace}}.Application.CommandHandlers.Validators` | **Flat** — all validators in one folder |
| CommandService Interface | `{{RootNamespace}}.Application.CommandServices.Abstractions` | **Flat** |
| CommandService Implementation | `{{RootNamespace}}.Application.CommandServices` | **Flat** |
| DataMappers | `{{RootNamespace}}.Application.DataMappers` | **Flat** |

## Domain Layer

| Artifact | Namespace | Folder Structure |
|----------|-----------|------------------|
| Aggregates (simple) | `{{RootNamespace}}.Domain.Aggregates` | Root of Aggregates/ |
| Aggregates (complex/partial) | `{{RootNamespace}}.Domain.Aggregates.[Feature]` | Per-feature subfolder |
| Aggregate Validators | `{{RootNamespace}}.Domain.Aggregates.Validators` | **Flat** |
| Domain Events | `{{RootNamespace}}.Domain.Events.[Feature]` | Per-feature subfolder |
| Error/Violation Events | `{{RootNamespace}}.Domain.Events.ErrorEvents` | **Flat** |
| Domain Models (DTOs) | `{{RootNamespace}}.Domain.Models` | **Flat** |
| Value Objects | `{{RootNamespace}}.Domain.ValueObjects` | **Flat** |
| Domain Services | `{{RootNamespace}}.Domain.DomainServices` | **Flat** |
| Entities | `{{RootNamespace}}.Domain.Entities` | **Flat** |

## Read Layer

| Artifact | Namespace | Folder Structure |
|----------|-----------|------------------|
| Queries | `{{RootNamespace}}.Read.Queries.[Feature]` | Per-feature subfolder |
| Query Handlers | `{{RootNamespace}}.Read.QueryHandlers.[Feature]` | Per-feature subfolder |
| Event Handlers | `{{RootNamespace}}.Read.EventHandlers.[Feature]` | Per-feature subfolder |
| ViewModels | `{{RootNamespace}}.Read.ViewModels` | **Flat** |

## Shared Layer

| Artifact | Namespace | Folder Structure |
|----------|-----------|------------------|
| Constants | `{{RootNamespace}}.Shared.Common.Constants` | **Flat** |
| Enums | `{{RootNamespace}}.Shared.Common.Enums` | **Flat** |
| Extensions | `{{RootNamespace}}.Shared.Common.Extensions` | **Flat** |
| Helpers | `{{RootNamespace}}.Shared.Common.Helpers` | **Flat** |
| SharedDto | `{{RootNamespace}}.Shared.SharedDto.[Feature]` | Per-feature subfolder |

## Infrastructure Layer

| Artifact | Namespace | Folder Structure |
|----------|-----------|------------------|
| Infrastructure | `{{RootNamespace}}.Infrastructure.Infrastructure` | Infrastructure implementations |

## Existing Feature Names

<!-- PROJECT-SPECIFIC: List your project's bounded context names here -->
<!-- Example: `Booking`, `Tower`, `Room`, `Payment`, `Invoice` -->
