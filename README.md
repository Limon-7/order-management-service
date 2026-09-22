# Backend Repo Template

Pre-configured scaffold for new business-oriented backend repositories on the modernization platform.

## What's Inside

```
backend-repo-template/
  .agents/          ← Agentic system: roles, rules, skills, workflows, docs
    roles/          ← Persona definitions (architect, planner, dev, tester, reviewer, bug-fixer)
    rules/          ← Guardrails & always-on constraints (general, architecture, security, stack, coding standards, permissions)
    skills/         ← Executable runbooks (create-pull-request, post-pr-review)
    workflows/      ← End-to-end SDLC processes (backend-development-workflow, complete-project-creation-guide, catalog-walkthrough-example, bug-resolution-procedure)
    docs/           ← Authoring guides & templates (skill-authoring-guide, sprint-tracking-template)
    mcp_config.json.example ← Template for MCP tools (MongoDB, GitHub, GitLab, Jira)
  .ai/              ← Architecture ground truth, patterns, templates for AI coding assistants
    templates/      ← Canonical C# file skeletons
    examples/       ← Reference end-to-end implementation (Category)
    decisions/      ← Architecture Decision Records (ADR)
    modules/        ← Infrastructure and persistence guides
  scratch/          ← .NET 10 solution scaffold (copy this into a fresh repo)
  AGENTS.md         ← Universal entry point for AI agents (Antigravity, Codex, Cursor, Copilot)
  CLAUDE.md         ← Claude Code instructions and development runbooks
```

## How to Use

> **Do not clone this repo and develop in it.** It's a template source only.

### Starting a New Backend Service

1. Create a fresh empty GitLab repository for your service
2. Open a new chat session in Antigravity (or Claude Code)
3. Say:

```
Set up a new backend repository for me.
GitLab URL: <your-new-repo-url>
Refer to: platform-ai-content/backend-repo-setup/SETUP.md
```

The agent will:
- Ask you clarifying questions (project name, namespace, connection strings, environments)
- Clone your repo
- Copy and configure the scaffold
- Run `dotnet restore` and `dotnet build` to verify
- Push the initial commit

### Copying AI Context Only (no code scaffold)

If you have an existing project and just want the `.ai/` and `.agents/` AI context files:

```bash
cp -r .agents/ /path/to/your-repo/
cp -r .ai/ /path/to/your-repo/
```

Then replace `{{RootNamespace}}` in the copied files with your project's actual root namespace.

---

## Scaffold Contents (`scratch/`)

| Path | Purpose |
|------|---------|
| `ProjectTemplate.sln` | Solution file — renamed to `{{ProjectName}}.sln` by agent |
| `Directory.Packages.props` | **Centralized NuGet versions** — no `Version=` in any `.csproj` |
| `Directory.Build.props` | Shared TFM (`net10.0`), Nullable, ImplicitUsings |
| `global.json` | Pins .NET SDK to `10.x` |
| `docker-compose.yml` | App-only compose (workers + API) |
| `docker-compose.infra.yml` | Local infrastructure compose (MongoDB + RabbitMQ with Management UI) |
| `src/Application/CommandWorker/` | Write-side worker host |
| `src/Application/CommandHandlers/` | `ICommandHandlerAsync<T>` implementations |
| `src/Application/Commands/` | Command message definitions |
| `src/Application/CommandServices/` | Domain orchestration services |
| `src/Application/DataMappers/` | Write-side data mapping |
| `src/Domain/Aggregates/` | Aggregate roots |
| `src/Domain/DomainServices/` | Domain services |
| `src/Domain/Entities/` | Non-root entities |
| `src/Domain/Events/` | Domain event definitions |
| `src/Domain/Models/` | Domain model classes |
| `src/Domain/ValueObjects/` | Value object types |
| `src/Infrastructure/Infrastructure/` | `ITransactionalRepository`, `IMongoUnitOfWork` |
| `src/Read/BusinessApiService/` | ASP.NET Core Web API (query entry point) |
| `src/Read/EventHandlers/` | `IEventHandlerAsync<T>` implementations |
| `src/Read/EventWorker/` | Read-side worker host |
| `src/Read/QueryHandlers/` | `IQueryHandlerAsync<T, R>` implementations |
| `src/Read/ViewModels/` | Read-side DTOs |
| `src/Saga/SagaService/` | Saga state machines (optional) |
| `src/Saga/SagaWorker/` | Saga worker host (optional) |
| `src/Shared/Common/` | Cross-cutting helpers, extensions |
| `src/Shared/SharedDto/` | Shared DTOs |
| `tests/Aggregates.UnitTest/` | Aggregate unit tests (xUnit) |
| `tests/CommandHandlers.UnitTest/` | Command handler unit tests |
| `tests/EventHandlers.UnitTest/` | Event handler unit tests |
| `tests/ProjectTemplate.IntegrationTest/` | Integration tests |

## Environment appsettings

Each runnable project has a full set of environment files:

| File | Used when |
|------|----------|
| `appsettings.json` | Base/fallback |
| `appsettings.Development.json` | Local Windows development |
| `appsettings.DockerLocal.json` | Running in Docker locally via `docker-compose up` |
| `appsettings.dev.json` | Dev server environment |
| `appsettings.qa.json` | QA environment |
| `appsettings.stg.json` | Staging environment |
| `appsettings.prod.json` | Production |

## Target Repositories

This template is for **business-oriented backends** only:
- Hotel Management System (HMS)
- SELP
- MyToken / MyLunch
- BEP, RFI Indicator, etc.

**Not** for platform infrastructure services (genericcommand, genericquery, push-notification, etc.) — those have a different structure.

## Tech Stack

- **.NET 10** 
- **Platform.Infrastructure.Core** (CQRS, Commands, Queries, Events)
- **MongoDB** via `ITransactionalRepository` / `IReadRepository`
- **MassTransit + RabbitMQ** for event-driven communication
- **Scrutor** for assembly scanning DI registration
- **Central Package Management** via `Directory.Packages.props`
