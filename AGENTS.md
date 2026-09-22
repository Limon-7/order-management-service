# AGENTS.md

This file provides universal instructions and operational context for all AI coding agents (Antigravity, Codex, Cursor, Copilot, Windsurf) working in this repository.

---

## 🧭 Repository Agentic Architecture

All agent rules, roles, skills, and templates are centrally organized:

- **`.agents/rules/`**: Mandatory, always-on behavioral constraints:
  - `general-rules.md`: The 10 core architectural rules (CQRS, DDD Aggregates, Event-Driven).
  - `global-architecture.md`: CQRS physical separation and Domain isolation.
  - `global-security.md`: Zero secrets, endpoint authorization (RBAC/Claims), injection prevention.
  - `global-tech-stack.md`: Flexible .NET 8 / 9 / 10, centralized NuGet packages.
  - `global-coding-standards.md`: C# naming, unused code/using cleanup, OpenTelemetry structured logging.
  - `global-permissions.md`: Safe autonomous execution vs. mandatory user confirmations; deployed Dev DB policies.

- **`.agents/roles/`**: Specialized Sub-Agent personas for each phase of feature development:
  - `srs-architect.md`: Requirements, API contracts, ERD schemas.
  - `jira-planner.md`: Epics, task breakdown, Definition of Ready.
  - `backend-developer.md`: C# CQRS implementation + living flow docs.
  - `test-engineer.md`: 2-tier testing (Aggregates unit tests + End-to-End integration tests).
  - `code-reviewer.md`: Architectural audit against the 10 rules + PR review output.
  - `bug-fixer.md`: Forensic defect investigation, reproduction test authoring, and surgical fixes.

- **`.agents/skills/`**: Executable runbooks:
  - `create-pull-request`: Commits code and opens a GitHub Pull Request via `gh` CLI.
  - `post-pr-review`: Posts structured review feedback directly to the PR via `gh` CLI.

- **`.ai/`**: Architectural and domain ground truth:
  - `templates/`: Canonical C# skeletons (`command.cs`, `aggregate-root.cs`, `data-mapper.cs`, etc.).
  - `examples/`: Canonical end-to-end reference implementation (Category).
  - `flows/`: Living feature documentation and architectural decisions.
  - `architecture.md`, `cqrs.md`, `di-registration-guide.md`, `namespace-conventions.md`.

---

## ⚡ Core Development Commands

```bash
# Build the entire solution
dotnet build

# Run all Unit & End-to-End Integration Tests
dotnet test

# Start Local Infrastructure (MongoDB + RabbitMQ Management UI)
docker compose -f scratch/docker-compose.infra.yml up -d

# Stop Local Infrastructure
docker compose -f scratch/docker-compose.infra.yml down

# Run Application Workers locally
docker compose -f scratch/docker-compose.yml up -d
```

---

## 🛡️ Non-Negotiable Agent Guardrails

0. **"No Plan, No Code" Guardrail:** NEVER generate or modify production code in `src/` without an approved **Task Plan** (Jira ticket `PROJ-xxx`) and an approved **SRS specification** (`docs/features/<feature>-srs.md`). If requested to write code without a plan, STOP and guide the user to run Stage 1 (SRS Discovery) and Stage 2 (Task Breakdown).
1. **Follow the 10 Architectural Rules:** All writes must be Commands extending `Platform.Infrastructure.Core.Commands.Command`; all reads must be Queries implementing `IQueryHandlerAsync`.
2. **Template Conformance:** Skeletons in `.ai/templates/` must be followed for all new components.
3. **Local / Dev Only:** Never target or connect to Staging, QA, or Production environments.
4. **Clean Code & OpenTelemetry:** Clean up unused `using` statements, remove dead code, and use structured logging with named placeholders via `ILogger`.
5. **Living Documentation:** Always update or create `.ai/flows/<feature>.md` upon completing any feature slice.

