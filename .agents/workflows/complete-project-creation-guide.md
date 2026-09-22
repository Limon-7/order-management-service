# 📘 Complete End-to-End Guide: From Requirements to Production

This document is the authoritative step-by-step master guide for creating a new backend project and taking any feature from **initial business requirements (SRS) to Jira planning, development, testing, review, and pull request**.

---

## 📑 Table of Contents
- [📘 Complete End-to-End Guide: From Requirements to Production](#-complete-end-to-end-guide-from-requirements-to-production)
  - [📑 Table of Contents](#-table-of-contents)
  - [Part 1: How to Bootstrap a New Project from this Template](#part-1-how-to-bootstrap-a-new-project-from-this-template)
    - [Step 1: Create Your New Git Repository](#step-1-create-your-new-git-repository)
    - [Step 2: Copy the Template Contents](#step-2-copy-the-template-contents)
    - [Step 3: Replace Scaffold Placeholders](#step-3-replace-scaffold-placeholders)
    - [Step 4: Verify Initial Build \& Infrastructure](#step-4-verify-initial-build--infrastructure)
  - [Part 2: The 6-Stage Feature Development Lifecycle](#part-2-the-6-stage-feature-development-lifecycle)
    - [Stage 1: Requirements Analysis \& SRS Design](#stage-1-requirements-analysis--srs-design)
      - [Step-by-Step Procedure:](#step-by-step-procedure)
    - [Stage 2: Jira Task Breakdown \& Definition of Ready](#stage-2-jira-task-breakdown--definition-of-ready)
      - [Step-by-Step Procedure:](#step-by-step-procedure-1)
    - [Stage 3: Clean Architecture Implementation](#stage-3-clean-architecture-implementation)
      - [Step-by-Step Procedure:](#step-by-step-procedure-2)
    - [Stage 4: Unified Event-Driven Testing](#stage-4-unified-event-driven-testing)
      - [Step-by-Step Procedure:](#step-by-step-procedure-3)
    - [Stage 5: Architectural Audit \& Code Review](#stage-5-architectural-audit--code-review)
      - [Step-by-Step Procedure:](#step-by-step-procedure-4)
    - [Stage 6: Automated Pull Request \& GitHub Review](#stage-6-automated-pull-request--github-review)
      - [Step-by-Step Procedure:](#step-by-step-procedure-5)
  - [Part 3: Daily Developer Cheatsheet \& Prompts (Antigravity \& Claude Code)](#part-3-daily-developer-cheatsheet--prompts-antigravity--claude-code)
    - [1. Stage 1: Triggering the Architect (SRS \& Discovery)](#1-stage-1-triggering-the-architect-srs--discovery)
    - [2. Stage 2: Triggering the Jira Planner (Task Breakdown)](#2-stage-2-triggering-the-jira-planner-task-breakdown)
    - [3. Stage 3: Triggering the Backend Developer (Implementation)](#3-stage-3-triggering-the-backend-developer-implementation)
    - [4. Stage 4: Triggering the Test Engineer (Unified Testing)](#4-stage-4-triggering-the-test-engineer-unified-testing)
    - [5. Stage 5: Triggering the Code Reviewer (Architectural Audit)](#5-stage-5-triggering-the-code-reviewer-architectural-audit)
    - [6. Stage 6: Shipping (Automated Pull Request \& Review)](#6-stage-6-shipping-automated-pull-request--review)
  - [Part 4: Configuring MCP Tools (MongoDB, GitHub, GitLab, Jira)](#part-4-configuring-mcp-tools-mongodb-github-gitlab-jira)
    - [In Antigravity:](#in-antigravity)
    - [In Claude Code:](#in-claude-code)

---

## Part 1: How to Bootstrap a New Project from this Template

> **Note:** [SETUP.md](file:///d:/platform-ai-content/backend-repo-template/SETUP.md) is the single source of truth for repository bootstrapping. 

You can trigger the entire setup in **Google Antigravity** or **Claude Code** with a single one-shot prompt:

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

When performing the bootstrap manually or via agent:

### Step 1: Create Your New Git Repository
Create an empty Git repository (GitLab or GitHub), for example: `hotel-management-service`.

### Step 2: Copy the Template Contents
Copy all foundational directories and files into your new repository root:
```bash
cp -r .agents/ /path/to/hotel-management-service/
cp -r .ai/ /path/to/hotel-management-service/
cp -r scratch/* /path/to/hotel-management-service/
cp AGENTS.md /path/to/hotel-management-service/
cp CLAUDE.md /path/to/hotel-management-service/
cp SETUP.md /path/to/hotel-management-service/
```

### Step 3: Replace Scaffold Placeholders
Search and replace the template tokens across all copied files:

| Token | Replacement Value | Example |
| :--- | :--- | :--- |
| `{{ProjectName}}` | PascalCase project name | `HotelManagement` |
| `{{BaseNamespace}}` | Base C# root namespace | `Bits.HotelManagement` |
| `{{ServiceId}}` | Kebab/lowercase service ID | `hotel-management` |
| `{{ServiceName}}` | Human-readable display name | `Hotel Management Service` |
| `{{HttpPort}}` | Dedicated Business API local port | `5010` |
| `{{AuthServerAddress}}` | Keycloak realm URL | `https://login-mdn.bracits.com/realms/mdndev` |

Rename `ProjectTemplate.sln` to `{{ProjectName}}.sln` and rename `tests/ProjectTemplate.IntegrationTest/` to `tests/{{ProjectName}}.IntegrationTest/`.

### Step 4: Dual Configuration Architecture (Local Dev vs. Deployed)
- **Local Development (`appsettings.Development.json`):**
  - Uses `localhost` MongoDB (`mongodb://localhost:27017`) and RabbitMQ (`amqp://guest:guest@localhost:5672/`) directly.
  - `ServiceRegistrationsFilePath` is left empty `""` (no external file required to build or debug locally!).
  - Workload database isolation:
    - Write DB: `{{ServiceId}}_write_db` (in `CommandWorker`)
    - Read DB: `{{ServiceId}}_read_db` (in `EventWorker` and `BusinessApiService`)
    - Saga State DB: `{{ServiceId}}_saga_state_db` (in `SagaWorker`)
- **Deployed / Docker Environments:**
  - Connection strings and database names are loaded dynamically from `ServiceRegistrations.json`.
  - Paths are **OS-independent** (relative `./config/ServiceRegistrations.example.json` in Docker Compose, `/app/Settings/...` in Linux containers).

### Step 5: Verify Initial Build & Infrastructure
```bash
# 1. Start local MongoDB & RabbitMQ
docker compose -f docker-compose.infra.yml up -d

# 2. Restore and build
dotnet restore
dotnet build

# 3. Verify tests run
dotnet test
```

Commit and push your baseline:
```bash
git add -A
git commit -m "chore: initial scaffold from backend-repo-template"
git push -u origin main
```

---

## Part 2: The 6-Stage Feature Development Lifecycle

```mermaid
flowchart TD
    subgraph S1 [Stage 1: SRS & Design]
        A[User Feature Request] --> B[srs-architect.md]
        B --> C[Produce docs/features/xyz-srs.md]
    end

    subgraph S2 [Stage 2: Jira Planning]
        C --> D[jira-planner.md]
        D --> E[Validate Definition of Ready]
        E --> F[Generate Epics & Sub-Tasks]
    end

    subgraph S3 [Stage 3: Development]
        F --> G[backend-developer.md]
        G --> H[Write CQRS & Domain Code]
        H --> I[Update .ai/flows/xyz.md]
    end

    subgraph S4 [Stage 4: Testing]
        I --> J[test-engineer.md]
        J --> K[Tier 1: Aggregate Unit Tests]
        J --> L[Tier 2: E2E Integration Tests]
        K & L --> M[dotnet test Verification]
    end

    subgraph S5 [Stage 5: Code Review]
        M --> N[code-reviewer.md]
        N --> O{Review Passes?}
        O -- No: Request Changes --> G
        O -- Yes: Approve --> P[Stage 6]
    end

    subgraph S6 [Stage 6: Shipping]
        P --> Q[Skill: create-pull-request]
        Q --> R[Skill: post-pr-review]
        R --> S[PR Ready for Merge]
    end
```

---

### Stage 1: Requirements Analysis & SRS Design
* **Assigned Sub-Agent:** `srs-architect.md`
* **Input:** Raw user story, business document, or ticket description.
* **Goal:** Conduct domain discovery, resolve ambiguous invariants, and lock down technical contracts before writing code.

#### Step-by-Step Procedure:
1. Provide the raw business request to the architect sub-agent.
2. The agent conducts an interactive interview to discover business rules (uniqueness, tenant isolation, price limits, concurrency).
3. The agent produces a structured document at `docs/features/<feature-name>-srs.md` containing:
   - **API Contracts (OpenAPI):** Exact endpoints, verbs, request/response DTOs, HTTP status codes.
   - **MongoDB Data Model:** Entity definitions, BSON serialization attributes, indexes.
   - **Event Contracts:** Message schemas for MassTransit/RabbitMQ events.
   - **Authorization & Security:** Required roles, claims, and tenant boundaries.
4. **Guardrail Check:** If business rules are vague, the architect agent **must stop and ask for clarification** rather than guessing.

---

### Stage 2: Jira Task Breakdown & Sprint Planning
* **Assigned Sub-Agent:** `jira-planner.md`
* **Input:** The approved SRS document from Stage 1 (`docs/features/<feature>-srs.md`).
* **Goal:** Validate readiness, break down the feature into vertical use cases, and generate the autonomous Sprint Tracking Sheet.

#### How to Run Sprint Planning (Step-by-Step):

1. **Validate Definition of Ready (DoR):**
   The planner reviews the SRS document. It checks that:
   - Every endpoint has request/response schemas.
   - MongoDB collections, fields, and indexes are defined.
   - Event contracts and authorization roles are explicit.
   *(If any are missing, it halts and sends the requirement back to Stage 1).*

2. **Strict Vertical Slicing by Use Case:**
   The planner rejects horizontal layer tasks. It slices work into independently runnable and testable business operations:
   - `Ticket 1: [Vertical Slice] Implement "Create [Entity]"` (Command + Validator + Aggregate + Event + Read Projection + DI)
   - `Ticket 2: [Vertical Slice] Implement "Update [Entity]"` (Command + Validator + Aggregate + Event + Read Projection + DI)
   - `Ticket 3: [Vertical Slice] Implement "Deactivate [Entity]"` (Command + Validator + Aggregate + Event + Read Projection + DI)

3. **Generate the Sprint Tracking Sheet:**
   The planner loads the template from `.agents/docs/sprint-tracking-template.md` and generates a dedicated tracker at `docs/planning/sprint-<number>-tracker.md`:
   - Calculates **Base Estimates** (in hours or story points).
   - Adds mandatory **Buffer Time (+25% on dev, +50% on tests)** for edge cases, network lag, and review cycles.
   - Initializes the status to `⚪ To Do`.

4. **Autonomous Status Progression During the Sprint:**
   Once planned, you do not need to update the sheet manually. As agents work:
   - `backend-developer.md` marks `🔵 In Progress` and records the feature branch.
   - `test-engineer.md` marks `🧪 Tests Passed` upon passing `dotnet test`.
   - `code-reviewer.md` marks `🔴 Changes Requested` or `🟢 Approved`.
   - `create-pull-request` skill marks `🟡 In PR Review` and pastes the clickable PR link.
   - Once merged, the task is marked `🟢 Done`.

5. **Guardrail Check:** No subtask may exceed 3 days of development scope (including buffer). Never separate an Aggregate from its Command into different tickets.

---

### Stage 3: Clean Architecture Implementation
* **Assigned Sub-Agent:** `backend-developer.md`
* **Input:** A specific Jira sub-task (`PROJ-xxx`) + reference to the SRS doc (`docs/features/<feature>-srs.md`).
* **Goal:** Write maintainable C# code strictly following platform rules.

> [!CAUTION]
> **🛡️ Mandatory Pre-Condition: "No Plan, No Code"**  
> The developer agent is strictly prohibited from writing or modifying code in `src/` without an approved Jira ticket from the active Sprint Tracker and an approved SRS reference. If a user asks for code directly, the agent must HALT and initiate Stage 1 (SRS Discovery) and Stage 2 (Task Breakdown).

> [!TIP]
> **🧠 Best Practice: Token Optimization via File-Based Handoff**  
> Running SRS discovery, Jira planning, and full-stack coding in a **single long conversation** will exhaust the LLM's context window (~80k+ tokens), causing high latency, attention degradation ("lost in the middle"), and hallucinations.  
> **The Solution:** Persist state to disk! Stage 1 writes `docs/features/<feature>-srs.md`, Stage 2 writes `docs/planning/sprint-*-tracker.md`. You then launch Stage 3 in a **fresh prompt or sub-agent session** that only reads the SRS and Flow doc (~3,000 tokens), keeping 97% of the context window pristine for rapid, high-precision code generation.

#### Step-by-Step Procedure:
1. **Initialize the Technical Flow Doc (`.ai/flows/<feature>.md`):**
   Before writing code, create or update `.ai/flows/<feature>.md` using `.ai/flows/template.md`:
   - **🆕 Files to Create:** Exact paths for Command, Validator, Handler, Aggregate, Event, ViewModel.
   - **✏️ Existing Files Impacted:** DI extensions, shared DTOs, Mongo collection mappings.
   - **Traceability Links:** Record Jira ticket (`PROJ-101`) and SRS section (`SRS §3.1`).
2. **Create Feature Branch:** `git checkout -b feature/PROJ-101-category-flow`.
3. **Implement Components following `.ai/templates/`:**
   - `command.cs` ➔ `src/Application/Commands/`
   - `command-validator.cs` ➔ `src/Application/CommandValidators/`
   - `command-handler.cs` ➔ `src/Application/CommandHandlers/`
   - `aggregate-root.cs` ➔ `src/Domain/Aggregates/`
   - `event-handler.cs` ➔ `src/Read/EventHandlers/`
4. **Wire Dependencies in DI:** Follow `.ai/di-registration-guide.md` to register handlers and repositories.
5. **Apply Coding Standards:** Follow `.agents/rules/global-coding-standards.md` (clean up unused `using` statements, eliminate dead code, add structured OpenTelemetry logging).
6. **Finalize Living Flow Doc:** Update status to `🟢 Complete` in `.ai/flows/<feature>.md`.

---

### Stage 4: Unified Event-Driven Testing
* **Assigned Sub-Agent:** `test-engineer.md`
* **Input:** Newly written implementation code.
* **Goal:** Validate the full lifecycle without mocks in integration tests.

#### Step-by-Step Procedure:
1. **Tier 1: Domain Unit Tests (`tests/Aggregates.UnitTest/`)**:
   - Tests business invariants in memory.
   - Asserts staged domain events:
     ```csharp
     var aggregate = Category.Create(id, name, tenantId);
     aggregate.DomainEvents.Should().ContainSingle(e => e is CategoryCreatedEvent);
     ```
2. **Tier 2: Unified E2E Integration Tests (`tests/ProjectTemplate.IntegrationTest/`)**:
   - Combines real MongoDB (`localhost:27017` or Testcontainers) and MassTransit bus in one fixture.
   - Executes the full loop in one test:
     `Dispatch Command ➔ Assert MongoDB Write ➔ Assert MassTransit Published & Consumed ➔ Assert MongoDB Read Model Projected`.
3. Runs test runner: `dotnet test`.

---

### Stage 5: Architectural Audit & Code Review
* **Assigned Sub-Agent:** `code-reviewer.md`
* **Input:** Git diff of changes made by Developer and Test Engineer.
* **Goal:** Ensure zero architectural violations or security holes reach Git.

#### Step-by-Step Procedure:
1. Audits against the **10 Core Rules in `general-rules.md`**:
   - Are commands inheriting `Command`?
   - Is `IReadRepository` kept completely out of write handlers?
   - Is DI wiring complete?
   - Is the living flow doc (`.ai/flows/<feature>.md`) updated?
2. Emits a structured review matrix with line-by-line findings and drop-in code fix snippets.
3. **Decides Verdict:**
   - If violations exist: Emits `Status: Request Changes`. The developer agent fixes the code, and Stage 5 re-runs.
   - If clean: Emits `Status: Approve`.

---

### Stage 6: Automated Pull Request & GitHub Review
* **Assigned Skills:** `create-pull-request` and `post-pr-review`
* **Goal:** Push code and post official AI approval on GitHub.

#### Step-by-Step Procedure:
1. **Run `create-pull-request` Skill:**
   - Stages files: `git add .`
   - Commits with conventional commit: `git commit -m "feat(category): [PROJ-101] implement category creation"`
   - Pushes branch: `git push -u origin feature/PROJ-101-category-flow`
   - Creates GitHub PR via CLI: `gh pr create`
2. **Run `post-pr-review` Skill:**
   - Takes the structured review report from Stage 5.
   - Posts official PR review comment via `gh pr review --approve -F temp_review_body.md`.

---

## Part 3: Daily Developer Cheatsheet & Prompts (Antigravity & Claude Code)

Copy and paste these exact prompts into **Google Antigravity** (IDE chat) or run them in **Claude Code** (terminal):

### 0. Stage 0: Bootstrapping a New Service Repository
* **Google Antigravity:**
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
* **Claude Code:**
  ```bash
  claude "Set up a new backend repository for me. Refer to SETUP.md. GitLab URL: https://gitlab.bracits.com/my-group/hotel-management-service.git, Project: HotelManagement, Namespace: Bits.HotelManagement, ServiceId: hotel-management, Port: 5010, Saga: No."
  ```

### 1. Stage 1: Triggering the Architect (SRS & Discovery)
* **Google Antigravity:**
  ```text
  Act as @srs-architect.md. Here is the feature requirement:
  "We need a Category Management module where admins can create and manage product categories scoped by tenant."
  First ask clarifying questions to uncover domain invariants, then produce docs/features/category-srs.md.
  ```
* **Claude Code:**
  ```bash
  claude "Act as the architect in .agents/roles/srs-architect.md. I want to build a Category Management module. First ask clarifying questions to uncover our domain rules, then write docs/features/category-srs.md."
  ```

### 2. Stage 2: Triggering the Jira Planner (Task Breakdown)
* **Google Antigravity:**
  ```text
  Act as @jira-planner.md. Read @docs/features/category-srs.md.
  Validate the Definition of Ready and break this down into actionable, sequential Jira technical sub-tasks.
  ```
* **Claude Code:**
  ```bash
  claude "Act as the planner in .agents/roles/jira-planner.md. Read docs/features/category-srs.md, validate the Definition of Ready, and break it down into sequential technical Jira tickets."
  ```

### 3. Stage 3: Triggering the Backend Developer (Implementation)
* **Google Antigravity:**
  ```text
  Act as @backend-developer.md. Implement Jira ticket PROJ-101: "Create Category Flow" (Vertical Slice).
  1. Initialize @.ai/flows/category-flow.md from @.ai/flows/template.md (list new files and impacted DI files).
  2. Refer to contracts in @docs/features/category-srs.md §3.1 and skeletons in @.ai/templates/.
  3. Wire DI and update status to 🔵 In Progress in @docs/planning/sprint-1-tracker.md.
  ```
* **Claude Code:**
  ```bash
  claude "Act as .agents/roles/backend-developer.md. Implement Jira ticket PROJ-101 (Create Category Flow). Refer to docs/features/category-srs.md §3.1 and initialize .ai/flows/category-flow.md with files to create/impact. Follow .ai/templates/ and .ai/di-registration-guide.md."
  ```

### 4. Stage 4: Triggering the Test Engineer (Unified Testing)
* **Google Antigravity:**
  ```text
  Act as @test-engineer.md. Write Tier 1 Aggregate unit tests and Tier 2 unified MongoDB + MassTransit integration tests for Category. Run dotnet test and verify all tests pass.
  ```
* **Claude Code:**
  ```bash
  claude "Act as the test engineer in .agents/roles/test-engineer.md. Write Tier 1 unit tests and Tier 2 unified MongoDB + MassTransit integration tests for Category. Execute dotnet test and report results."
  ```

### 5. Stage 5: Triggering the Code Reviewer (Architectural Audit)
* **Google Antigravity:**
  ```text
  Act as @code-reviewer.md. Review all modified files against @general-rules.md, .ai/templates/, and @global-security.md.
  Output the full markdown review report.
  ```
* **Claude Code:**
  ```bash
  claude "Act as the reviewer in .agents/roles/code-reviewer.md. Review all git changes against .agents/rules/general-rules.md, .ai/templates/, and .agents/rules/global-security.md. Output the structured markdown review report."
  ```

### 6. Stage 6: Shipping (Automated Pull Request & Review)
* **Google Antigravity:**
  ```text
  The code review has passed. Please execute the @create-pull-request skill, followed by the @post-pr-review skill.
  ```
* **Claude Code:**
  ```bash
  claude "The review has passed. Follow .agents/skills/create-pull-request/SKILL.md to commit and open the PR, then follow .agents/skills/post-pr-review/SKILL.md to post the review."
  ```

---

## Part 4: Configuring MCP Tools (MongoDB, GitHub, GitLab, Jira)

Both Antigravity and Claude Code support Model Context Protocol (MCP) servers:

### In Antigravity:
1. Copy `.agents/mcp_config.json.example` to `.agents/mcp_config.json`.
2. Populate connection strings and tokens.
3. Antigravity automatically detects and mounts the tools.

### In Claude Code:
You can register MCP servers directly from your terminal:
```bash
# Add MongoDB MCP (local/dev)
claude mcp add mongodb npx -y @modelcontextprotocol/server-mongodb mongodb://localhost:27017/my_dev_db

# Add GitHub MCP
claude mcp add github -e GITHUB_PERSONAL_ACCESS_TOKEN=<YOUR_TOKEN> npx -y @modelcontextprotocol/server-github

# Add GitLab MCP
claude mcp add gitlab -e GITLAB_PERSONAL_ACCESS_TOKEN=<YOUR_TOKEN> npx -y @modelcontextprotocol/server-gitlab

# List active servers
claude mcp list
```
