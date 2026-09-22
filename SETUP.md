# Backend Repository Setup Guide

> **This file is the single source of truth for the AI agent setting up a new business-oriented backend repository from this template.**
> When starting a new service, point the agent here along with your new Git repository URL.
> Follow every step in order. Do not skip steps.

---

## What This Template Creates

A fully-structured .NET 10 backend solution following the platform's DDD / CQRS / Event-Driven architecture (matching patterns from `hms-back-end` and `selp-back-end`). Designed for **business-oriented microservices** (HMS, SELP, MyToken, etc.) — not platform infrastructure services.

### Architecture at a Glance
```
src/
  Application/
    Commands/            ← Command message definitions
    CommandHandlers/     ← ICommandHandlerAsync<TCommand> implementations
    CommandServices/     ← Domain orchestration services
    DataMappers/         ← Write-side data mapping
    CommandWorker/       ← Worker host: consumes commands from RabbitMQ
  Domain/
    Aggregates/          ← Aggregate roots (write-side state)
    DomainServices/      ← Domain logic not fitting into aggregates
    Entities/            ← Non-root entities
    Events/              ← Domain event definitions
    Models/              ← Domain model classes
    ValueObjects/        ← Value object types
  Infrastructure/
    Infrastructure/      ← ITransactionalRepository, IMongoUnitOfWork
  Read/
    EventHandlers/       ← IEventHandlerAsync<TEvent> implementations
    EventWorker/         ← Worker host: consumes events from RabbitMQ
    QueryHandlers/       ← IQueryHandlerAsync<TQuery, TResponse> implementations
    ViewModels/          ← Read-side DTOs / view models
    BusinessApiService/  ← ASP.NET Core Web API (query entry point)
  Saga/                  ← (Optional) Saga state machines
    SagaService/
    SagaWorker/
  Shared/
    Common/              ← Cross-cutting helpers, extensions, constants
    SharedDto/           ← Shared DTOs used across layers
tests/
  Aggregates.UnitTest/
  CommandHandlers.UnitTest/
  EventHandlers.UnitTest/
  {{ProjectName}}.IntegrationTest/
```

---

## Prerequisites

The user must provide:
1. Target Git repository URL (fresh, empty repository on GitLab or GitHub)
2. Answers to the clarifying questions in Step 1 below

The template scaffold is located in: `scratch/` (relative to this template root).

### 🚀 Quick Start: Ready-to-Run Prompt
You can trigger this entire setup in a single message by sending this prompt to Antigravity or Claude Code:
```text
Set up a new backend repository for me.
Refer to: SETUP.md

Here are my project details:
- GitLab URL: https://gitlab.bracits.com/my-group/hotel-management-service.git
- Project Name: HotelManagement
- Base Namespace: Bits.HotelManagement
- Service ID: hotel-management
- Service Display Name: Hotel Management Service
- Include Saga: No
- HTTP Port: 5010
```

---

## Step 1: Ask the User — Clarifying Questions

Before touching any files, ask the user the following questions in a single prompt. Record every answer.

### Required (Core Identity)

| # | Question | Example Answer | Token |
|---|----------|---------------|-------|
| 1 | **Project Name** (PascalCase, no spaces) | `MyService` | `{{ProjectName}}` |
| 2 | **Base Namespace** (e.g. `Bits.MyService` or `Platform.MyService`) | `Bits.MyService` | `{{BaseNamespace}}` |
| 3 | **Service ID** (short lowercase, used in config + Docker image names) | `myservice` | `{{ServiceId}}` |
| 4 | **Service Display Name** (human-readable) | `My Service` | `{{ServiceName}}` |
| 5 | **Include Saga layer?** (Yes/No) | Yes or No | — |
| 6 | **Service Registration file path on your machine** | `C:\Config\ServiceRegistrations.json` | `{{ServiceRegPath_Dev}}` |
| 7 | **Authorization Server Address** — (default: `https://login-mdn.bracits.com/realms/mdndev`). Confirm or provide new URL | Confirm or provide URL | `{{AuthServerAddress}}` |

### Environment & Database Settings

> [!TIP]
> **Dual Configuration Mode:**
> 1. **Local Development (`Development` profile):** Directly uses the values in `appsettings.Development.json` (pointing to `localhost` Mongo & RabbitMQ). No external `ServiceRegistrations.json` file is required to build or debug locally!
> 2. **Deployed / Docker Environments (`DockerLocal`, `dev`, `qa`, `stg`, `prod`):** Dynamically loads connection strings from `ServiceRegistrations.json` via `ServiceRegistrationsFilePath`. Paths are **OS-independent** (relative `./config/...` in Docker Compose, `/app/Settings/...` inside Linux containers).

Ask for each environment if custom overrides are needed: `Development`, `DockerLocal`, `dev`, `qa`, `stg`, `prod`

| Config Key | Local Development Default | Deployed / Docker Default |
|-----------|---------------------------|---------------------------|
| `ServiceRegistrationsFilePath` | `""` (reads directly from `appsettings`) | `/app/Settings/.../ServiceRegistrations.json` (Docker) or `/Config/ServiceRegistrations.json` (Linux) |
| `RabbitMQ ConnectionString` | `amqp://guest:guest@localhost:5672/` | `amqp://guest:guest@host.docker.internal:5672/` (Docker) or host cluster |
| `MongoDB ConnectionString` | `mongodb://localhost:27017` | Provided via `ServiceRegistrations.json` |
| `Redis ConnectionString` | `localhost:6379` | `redis:6379` or host cluster (optional) |

### Optional Features (ask once)

| Feature | Question |
|---------|---------|
| Redis Cache | Will this service use Redis for caching? (If yes, uncomment Redis in appsettings and worker registrations) |
| Quartz Scheduler | Will `CommandWorker` need a built-in job scheduler? (Quartz.NET) |
| Report Worker | Does the read side need a dedicated report/export worker separate from EventWorker? |
| Additional ports | What HTTP port should `BusinessApiService` listen on locally? (default: 5000 HTTP / 7000 HTTPS) |

---

## Step 2: Clone the Target Repository

```bash
git clone <Git URL>
cd <repo-name>
```

---

## Step 3: Copy Template Scaffold

Copy **everything** from `scratch/` into the repository root:

> **Do not copy** the `scratch/` folder itself — copy its *contents*.
> Also copy `.agents/` and `.ai/` into the new repository root so the AI agent system remains available in the new service.

After copying, the repo root should contain:
- `ProjectTemplate.sln`
- `Directory.Packages.props`
- `Directory.Build.props`
- `global.json`
- `.gitignore`
- `.dockerignore`
- `docker-compose.yml`
- `docker-compose.infra.yml`
- `config/`
- `.config/`
- `src/`
- `tests/`
- `.agents/`
- `.ai/`

---

## Step 4: Find-and-Replace All Placeholders

Replace every occurrence of each token throughout **all files** (filenames included where noted):

| Token | Replace With | Notes |
|-------|-------------|-------|
| `{{ProjectName}}` | User's answer #1 | Also rename `ProjectTemplate.sln` → `{{ProjectName}}.sln` and `tests/ProjectTemplate.IntegrationTest/` folder + `.csproj` file |
| `{{BaseNamespace}}` | User's answer #2 | Used in `AssemblyName`, `RootNamespace`, `using` directives, queue names |
| `{{ServiceId}}` | User's answer #3 | Used in `ServiceId` config key, Docker image names in `docker-compose.yml` |
| `{{ServiceName}}` | User's answer #4 | Used in `ServiceName` config key |
| `{{AuthServerAddress}}` | User's answer #7 | Used in `AuthorizationServerAddress` in Business API appsettings |
| `{{ServiceRegPath_Dev}}` | User's answer #6 | Used in `ServiceRegistrationsFilePath` in development appsettings |

---

## Step 5: Populate Environment-Specific appsettings

For each runnable project (`CommandWorker`, `EventWorker`, `BusinessApiService`, `SagaWorker` if included), populate the environment configuration files:

### `appsettings.Development.json` (Local Development)
- `ServiceRegistrationsFilePath`: `""` (Empty — instructs the platform to read local settings directly from `appsettings.Development.json`)
- `BusConfig.ConnectionString`: `amqp://guest:guest@localhost:5672/`
- `MongoDb.ConnectionString`: `mongodb://localhost:27017`
- `MongoDb.DatabaseName`:
  - `CommandWorker`: `{{ServiceId}}_write_db` (Write DB)
  - `EventWorker`: `{{ServiceId}}_read_db` (Read DB)
  - `BusinessApiService`: `{{ServiceId}}_read_db` (Read DB)
  - `SagaWorker`: `{{ServiceId}}_saga_state_db` (Saga State DB)
- Redis: uncomment `ConnectionStrings.Redis` only if Redis is used

### `appsettings.DockerLocal.json` (Local Docker Compose)
- `ServiceRegistrationsFilePath`: `/app/Settings/ServiceRegistrations/ServiceRegistrations.json` (OS-independent container path)
- `BusConfig.ConnectionString`: `amqp://guest:guest@host.docker.internal:5672/` (container → host RabbitMQ)
- Redis: `redis:6379` (container name) if used

### `appsettings.dev.json`, `appsettings.qa.json`, `appsettings.stg.json`, `appsettings.prod.json` (Deployed Environments)
- Configure message bus connection strings and environment settings
- Set `ServiceRegistrationsFilePath` to the environment-specific path (typically `/Config/ServiceRegistrations.json` or container path on Linux hosts)

### BusinessApiService only:
- `AuthorizationServerAddress`: `{{AuthServerAddress}}`

### External Setup: Service Registration Entry
Ensure the user's `C:\Config\ServiceRegistrations.json` (or target environment's file) contains the database entries for this service:

```json
{
  "Services": [
    {
      "ServiceId": "{{ServiceId}}",
      "ServiceName": "{{ServiceName}}",
      "Databases": {
        "WriteDb": {
          "ConnectionString": "mongodb://localhost:27017",
          "DatabaseName": "{{ServiceId}}_write_db"
        },
        "ReadDb": {
          "ConnectionString": "mongodb://localhost:27017",
          "DatabaseName": "{{ServiceId}}_read_db"
        },
        "StateDb": {
          "ConnectionString": "mongodb://localhost:27017",
          "DatabaseName": "{{ServiceId}}_saga_state_db"
        },
        "CacheDb": {
          "ConnectionString": "localhost:6379",
          "InstanceName": "{{ServiceId}}_cache"
        }
      },
      "BusConfig": {
        "ConnectionString": "amqp://guest:guest@localhost:5672/"
      }
    }
  ]
}
```

---

## Step 6: Handle Optional Features

### If Saga = No
- Delete the entire `src/Saga/` directory
- Remove saga-related projects from the `.sln` file (the `Saga` solution folder and its two projects)
- Remove the commented saga service entry from `docker-compose.yml`

### If Redis = No
- In all `appsettings.*.json` files, delete or leave commented the `ConnectionStrings.Redis` key
- In `CommandWorker.csproj`, comment out `Platform.Infrastructure.RedisCache` PackageVersion reference
- In `CommandWorker/Program.cs`, keep `// builder.Services.UseRedisCache();` commented

### If Quartz = No
- In `CommandWorker/Program.cs`, delete the entire `// ── Scheduler (Quartz.NET) ──` commented block
- In `CommandWorker.csproj`, remove the three Quartz `PackageReference` entries
- In `Directory.Packages.props`, remove the three Quartz `PackageVersion` entries from the optional section

### If Redis = Yes
- Uncomment `// builder.Services.UseRedisCache();` in `CommandWorker/Program.cs`
- Uncomment `// <PackageReference Include="Platform.Infrastructure.RedisCache" />` in `CommandWorker.csproj`
- Uncomment `ConnectionStrings.Redis` in relevant appsettings files

### If Quartz = Yes
- Uncomment the Quartz block in `CommandWorker/Program.cs`
- Uncomment the three Quartz `PackageReference` entries in `CommandWorker.csproj`

### If Report Worker = Yes
- Create `src/Read/ReportWorker/` following the same structure as `EventWorker`
- Add it to the solution file under the `Read` solution folder

---

## Step 7: Configure docker-compose.yml

Update `docker-compose.yml`:
- `{{ServiceId}}` is already replaced in Step 4
- Update the `volumes` path if the user's `ServiceRegistrations.json` is not at `C:\Config\`
- If Saga was excluded, remove the commented saga worker entry entirely
- Set port numbers from user's answers (default BusinessApiService HTTP port: 5000)

---

## Step 8: Verify the Setup

Run the following and confirm no errors:

```bash
dotnet restore
dotnet build
dotnet test
```

If restore fails with NuGet authentication errors:
- Check that `config/NuGetPackageSource.Config` has valid credentials for the BracIts private feed
- Run `dotnet nuget add source` or copy the NuGet.Config to the appropriate location

---

## Step 9: Initial Commit and Push

```bash
git add -A
git commit -m "chore: initialize project from backend-repo-template

Project: {{ProjectName}}
Namespace: {{BaseNamespace}}
.NET: 10.0
Architecture: DDD/CQRS/Event-driven (Platform.Infrastructure.Core)"
git push origin main
```

---

## Per-Environment Config Matrix

| Config Key | Development (Local) | DockerLocal (Compose) | dev / qa / stg / prod | Notes |
|-----------|---------------------|-----------------------|-----------------------|-------|
| `ServiceRegistrationsFilePath` | `""` (reads local appsettings) | `/app/Settings/.../ServiceRegistrations.json` | `/Config/ServiceRegistrations.json` or container mount | **OS-independent paths** |
| `MongoDb.ConnectionString` | `mongodb://localhost:27017` | From ServiceRegistrations / Docker | From ServiceRegistrations / cluster | Direct in appsettings for local dev |
| `BusConfig.ConnectionString` | `amqp://guest:guest@localhost:5672/` | `amqp://guest:guest@host.docker.internal:5672/` | environment host | Message bus |
| `ConnectionStrings.Redis` | `localhost:6379` | `redis:6379` | environment host | Optional cache |
| `AuthorizationServerAddress` | dev Keycloak realm | — | env Keycloak realm | Keycloak auth |
| `Logging.LogLevel.Default` | `Information` | `Information` | `Warning` (prod) | Serilog / Microsoft |

---

## Notes on What NOT to Do

<!-- - Do **not** store MongoDB connection strings in `appsettings.json` — all DB configs come from `ServiceRegistrations.json`. -->
- Do **not** add project-specific business logic here — this is a technical scaffold only
- Do **not** add `DataMigration` projects unless the user explicitly requests it (business-specific)
- Do **not** configure platform infrastructure services (RabbitMQ, Redis, MongoDB) in the compose file — these run separately
- Do **not** use this template for platform services (genericcommand, genericquery, etc.) — they have a different structure
- Do **not** configure platform infrastructure services (RabbitMQ, Redis, MongoDB) inside the app's `docker-compose.yml` — use `docker-compose.infra.yml` for local infrastructure.

