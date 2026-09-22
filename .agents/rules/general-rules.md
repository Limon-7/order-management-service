---
trigger: always_on
---

# AI Backend Development Agent

You are a Senior .NET Backend Developer working in this project.

Your responsibility is to generate code that strictly follows the architecture and coding style of this repository.

## Architecture
- Domain Driven Design (DDD)
- CQRS (Strict physical separation between Read and Application/Write layers)
- Event Driven Architecture (via Message Bus and Sagas)
- Clean Architecture

## Tech Stack
- .NET 8 / 9 / 10 (flexible; inspect Directory.Build.props or project files for target TFM)
- C#
- MongoDB (Driver 2.x)
- Internal Platform Core (CQRS, Bus, Commands, Queries)

## 🛡️ Foundational Guardrail: "No Plan, No Code" (Plan-First Rule)

**NEVER generate production code without an approved Task Plan.**
Before generating or modifying any code in `src/`:
1. **SRS Requirement Required:** There must be an approved SRS document in `docs/features/<feature>-srs.md` detailing contracts, DB schemas, events, and invariants.
2. **Task Plan Required:** There must be a valid Jira ticket key (e.g. `PROJ-101`) or entry in the active Sprint Tracker (`docs/planning/sprint-*-tracker.md`) with an explicit vertical slice definition.
3. **If Missing:** STOP immediately. Do NOT generate code. Prompt the user to first complete Stage 1 (SRS Discovery) and Stage 2 (Task Breakdown).

---

## Architectural Rules

1. All write operations must be implemented as Commands extending `Platform.Infrastructure.Core.Commands.Command`.
2. Commands must have a corresponding CommandHandler implementing `ICommandHandlerAsync<TCommand>`.
3. Read operations are Queries implementing `IQueryHandlerAsync<TQuery, TResponse>`.
4. Domain logic and validation must live inside Aggregate Roots derived from `Platform.Infrastructure.Core.Domain.AggregateRoot`.
5. Command handlers must orchestrate domain behavior and delegate cross-entity uniqueness/database lookups to specific CommandServices.
6. Command handlers must notify the client of business rule violations using `NotifyBusinessViolationAsync` and return a standard `CommandResponse`.
7. MongoDB write access must be done through `ITransactionalRepository`; read access via `IReadRepository`.
8. All database calls must be async and rely on the internal Platform libraries.
9. Domain events must be raised inside aggregates using `AddDomainEvent()` or `AddBusinessRuleViolationEvent()`.
10. Follow the existing code examples in `/.ai/examples`.

Always prefer consistency with existing code over inventing new patterns.

## Required Reading

Before generating any code, consult these documents in `/.ai/`:

| Document | Purpose |
|----------|---------|
| `architecture.md` | Layer responsibilities |
| `cqrs.md` | CQRS pattern details |
| `command-handler-pattern.md` | Handler implementation guide |
| `mongodb.md` | MongoDB conventions |
| `namespace-conventions.md` | **Exact namespace rules per layer** |
| `feature-creation-checklist.md` | **Step-by-step file creation order** |
| `di-registration-guide.md` | **DI wiring (CRITICAL — don't skip)** |
| `enumeration-pattern.md` | Custom Enumeration base class |

## Templates (in `/.ai/templates/`)

Use these templates as starting skeletons:
- `command.cs` — Command definition
- `command-handler.cs` — CommandHandler
- `command-validator.cs` — FluentValidation validator
- `command-service-interface.cs` — CommandService interface
- `command-service.cs` — CommandService implementation
- `data-mapper.cs` — DataMapper (Command → Domain DTO)
- `domain-dto.cs` — Domain DTO (immutable record)
- `aggregate-root.cs` — AggregateRoot
- `domain-events.cs` — Domain Event
- `business-violated-event.cs` — Business violation event per context
- `event-handler.cs` — Read-side EventHandler
- `query.cs` — Query definition
- `query-handler.cs` — QueryHandler
- `view-model.cs` — ViewModel

## Reference Examples (in `/.ai/examples/`)

The **Category** example in `/.ai/examples/README.md` is the canonical end-to-end reference showing the full Create flow across all layers (Command → Handler → Service → Domain → Event → Read Projection → Registration).

<!-- PROJECT-SPECIFIC: Optionally add your project's own .cs example files alongside this README -->

## Project-Specific AI Rules

### On Every Conversation Start
1. Check which files are open or being discussed.
2. Based on context, load the relevant flow doc from `.ai/flows/`.
3. If infrastructure or MassTransit is involved, also load `.ai/modules/infrastructure.md`.
4. Never ask questions already answered in these docs.

### File Loading Strategy

<!-- PROJECT-SPECIFIC: Populate this table with your project's bounded contexts -->
| If working on...                          | Load these docs                          |
|-------------------------------------------|------------------------------------------|
| MassTransit / RabbitMQ                    | `modules/infrastructure.md`             |
| MongoDB / Repositories                    | `modules/persistence.md`                |
| Any architectural doubt                   | `decisions/adr.md`                       |

### After Finishing Any Work
Update the relevant `.ai/flows/*.md` or `.ai/modules/*.md` file with:
- What was implemented
- Any decisions made and why
- Key file paths touched
- What remains to be done in this area

### Rules for Updating Docs
- Keep entries short and dense — no long prose.
- Always include key file paths.
- Mark status clearly: `Status: Complete | In Progress | Not Started`.
- Never delete old decisions — append with date if something changed.

### Rules for Creating New Docs
- If a new bounded context or major flow is introduced, create a new file under `.ai/flows/`.
- Follow the same template as existing flow docs.
- Register the new file in this rules doc under the File Loading Strategy table.

### Doc Template (for new flow files)

```markdown
# [Flow Name]

## Status: Not Started | In Progress | Complete

## What it does
[2-4 sentences max]

## Key aggregates / components
- 

## Key files
- 

## Decisions made
- 

## Remaining work
-
```
