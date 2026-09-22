# CLAUDE.md

This file provides comprehensive instructions and operational guidance to **Claude Code** (`claude.ai/code`) when operating in this repository.

---

## Project Overview

This repository is a pre-configured backend scaffold for business-oriented services built on an **Event-Driven, Clean Architecture with CQRS**:
- **Framework:** .NET 8 / 9 / 10 (flexible; verify target framework in `Directory.Build.props` / project files, default to .NET 10).
- **Persistence:** MongoDB (via `ITransactionalRepository` for writes and `IReadRepository` for reads).
- **Messaging:** MassTransit + RabbitMQ for asynchronous event publishing and consumer projections.
- **Centralized Dependencies:** Managed centrally in `Directory.Packages.props`.

---

## Development Commands

### Building & Restoring
```bash
# Restore packages
dotnet restore

# Build solution
dotnet build
```

### Testing
```bash
# Run all unit and integration tests
dotnet test

# Run specific test project
dotnet test tests/Aggregates.UnitTest
dotnet test tests/ProjectTemplate.IntegrationTest
```

### Local Infrastructure & Docker
```bash
# Start local MongoDB (port 27017) and RabbitMQ (ports 5672, 15672)
docker compose -f scratch/docker-compose.infra.yml up -d

# Stop local infrastructure
docker compose -f scratch/docker-compose.infra.yml down

# Start compiled micro-host application workers
docker compose -f scratch/docker-compose.yml up -d
```

---

## 🚀 How to Use Claude Code in this Repository

Claude Code operates directly in your terminal via the `claude` CLI. You can use it interactively or with direct prompts.

### 1. Starting Claude Code
```bash
# Start an interactive session in the project root
claude

# Or run a single prompt directly
claude "Run dotnet build and report any errors"
```

---

### 2. Invoking Specialized Sub-Agent Roles in Claude Code

When working on features, instruct Claude Code to load the specific role prompt from `.agents/roles/`:

#### A. Stage 1: Requirements Discovery & Domain Rules (SRS Architect)
```bash
claude "Act as the architect in .agents/roles/srs-architect.md. I want to build a [Feature Name] feature. First, ask me clarifying questions to uncover our domain rules (uniqueness, tenant isolation, invariants). Once agreed, write docs/features/[feature]-srs.md."
```

#### B. Stage 2: Jira Task Breakdown (Jira Planner)
```bash
claude "Act as the technical planner in .agents/roles/jira-planner.md. Read docs/features/[feature]-srs.md, validate the Definition of Ready, and break it down into sequential Jira tickets."
```

#### C. Stage 3: Development (Backend Developer)
```bash
claude "Act as the developer in .agents/roles/backend-developer.md. Implement Task #1 from docs/features/[feature]-srs.md using .ai/templates/ and .ai/di-registration-guide.md. Update .ai/flows/[feature].md when complete."
```

#### D. Stage 4: Unified Event-Driven Testing (Test Engineer)
```bash
claude "Act as the test engineer in .agents/roles/test-engineer.md. Write Tier 1 unit tests and Tier 2 unified MongoDB + MassTransit integration tests for [Feature]. Run dotnet test to verify."
```

#### E. Stage 5: Architectural Code Review (Code Reviewer)
```bash
claude "Act as the reviewer in .agents/roles/code-reviewer.md. Review all git changes against .agents/rules/general-rules.md, .ai/templates/, and .agents/rules/global-security.md. Output the structured markdown review report."
```

#### F. Bug Resolution & Hotfixing (Bug Fixer)
```bash
claude "Act as the bug fixer in .agents/roles/bug-fixer.md. Follow .agents/workflows/bug-resolution-procedure.md to resolve bug [TICKET-KEY]. First write a failing test reproducing the defect in tests/, apply the surgical fix, verify all tests pass, and report results."
```

---

### 3. Running Skills & Automations with Claude Code

Claude Code has native terminal execution capabilities. To run the automated workflows:

#### Create Pull Request:
```bash
claude "Follow .agents/skills/create-pull-request/SKILL.md to commit all changes with a conventional commit message, push the branch, and create a pull request using gh pr create."
```

#### Post PR Review:
```bash
claude "Follow .agents/skills/post-pr-review/SKILL.md to take the latest review report and post it to the active GitHub Pull Request using gh pr review."
```

---

### 4. Configuring MCP Tools for Claude Code

Claude Code supports Model Context Protocol (MCP) servers. You can add the project MCP servers defined in `.agents/mcp_config.json.example`:

```bash
# Add MongoDB MCP (local/dev)
claude mcp add mongodb npx -y @modelcontextprotocol/server-mongodb mongodb://localhost:27017/my_dev_db

# Add GitHub MCP
claude mcp add github -e GITHUB_PERSONAL_ACCESS_TOKEN=<YOUR_TOKEN> npx -y @modelcontextprotocol/server-github

# Add GitLab MCP
claude mcp add gitlab -e GITLAB_PERSONAL_ACCESS_TOKEN=<YOUR_TOKEN> npx -y @modelcontextprotocol/server-gitlab

# List active MCP servers
claude mcp list
```

---

## 🏛️ Architecture & Coding Standards

Claude **MUST** consult and strictly adhere to:
1. **`.agents/rules/general-rules.md`**: The 10 core architectural rules of the platform (CQRS, DDD Aggregates, ITransactionalRepository vs IReadRepository).
2. **`.ai/templates/`**: Canonical skeletons for commands, handlers, aggregate roots, data mappers, and events.
3. **`.ai/di-registration-guide.md`**: Mandatory DI wiring for every command, query, handler, validator, and mapper.
4. **`.ai/namespace-conventions.md`**: Layer-specific namespaces and folder placement.
5. **`.agents/rules/global-coding-standards.md`**: Strict C# formatting, aggressive removal of unused code/usings, and structured OpenTelemetry logging with `ILogger`.

---

## 🛡️ Non-Negotiable Operational Guardrails for Claude

- **"No Plan, No Code" Rule:** NEVER generate or modify production code in `src/` without an approved **Task Plan** (Jira ticket `PROJ-xxx` or Defect ticket `BUG-xxx`) linked to an approved SRS document in `docs/features/`. If asked to write code directly without a plan, Claude must STOP and prompt the user to run Stage 1 (SRS Discovery) and Stage 2 (Task Planning) first.
- **Environment Isolation:** ONLY target `Local` (Docker/localhost) or designated `Dev` environments. Never connect to, migrate, or run tests against Staging, QA, or Production.
- **Deployed Dev DB Safety:** Never execute `dropDatabase` or drop shared collections on remote Dev MongoDB clusters.
- **No Database Mocks in Integration Tests:** Integration tests must always hit real MongoDB (via Testcontainers or local Docker).
- **Zero Secrets:** Never hardcode passwords, tokens, or connection strings in code or output.
- **Dependency Strictness:** ONLY use NuGet packages defined in `Directory.Packages.props`.
- **Always Verify:** Claude should proactively run `dotnet build` and `dotnet test` after code generation.
- **Living Documentation:** When completing any feature, create or update `.ai/flows/<feature>.md` (using `.ai/flows/template.md`) detailing files touched and decisions made.
