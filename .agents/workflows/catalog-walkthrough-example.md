# 📦 Concrete Feature Walkthrough: Building a Catalog Service

This document provides an end-to-end practical walkthrough of building a real feature (**Catalog / Product Management**) using our 6-stage multi-agent workflow. 

It includes the **exact prompts and commands** for both **Google Antigravity** and **Claude Code** at every step.

---

## 🎯 The Feature Scenario
**Goal:** Build a Catalog module where merchants can create and manage products under categories with SKU, price, name, and tenant isolation.

---

## Stage 1: Requirements Discovery & Domain Invariants (SRS)

### ❓ What Happens:
You do **not** need a finished specification beforehand. The **System Architect Sub-Agent** (`srs-architect.md`) conducts an **interactive clarification dialogue** with you to discover domain rules, invariants, and edge cases before generating technical contracts.

### 💻 Commands to Trigger Stage 1:

#### In Google Antigravity:
```text
Act as @srs-architect.md. 

I want to build a Catalog feature where merchants can create and manage products under categories. 

Before generating the specification:
1. Ask me any clarifying questions needed to identify critical domain rules, invariants, and edge cases.
2. Once we agree on the domain rules, produce the complete technical contract in docs/features/catalog-srs.md (including API payload, MongoDB schema, and MassTransit events).
```

#### In Claude Code:
```bash
claude "Act as the architect in .agents/roles/srs-architect.md. I want to build a Catalog feature for creating products under categories. First ask me clarifying questions to uncover our domain rules (SKU uniqueness, tenant isolation, price invariants). Then write docs/features/catalog-srs.md."
```

### 💬 Clarification Dialogue Example:
> **Architect:**
> 1. **SKU Invariant:** Is SKU unique globally across the entire database, or unique only per tenant?
> 2. **Category Assignment:** Can a product exist without a category?
> 3. **Price Rules:** What currency format is required, and can prices be zero or negative?
>
> **Your Reply:**
> 1. SKU must be unique per tenant.
> 2. Category is strictly required.
> 3. Price must be a positive decimal; multi-currency is not needed in v1.

**Deliverable Generated:** `docs/features/catalog-srs.md` (OpenAPI contracts, MongoDB BSON schemas, `ProductCreatedEvent` schema, and RBAC requirements).

---

## Stage 2: Jira Task Breakdown & Definition of Ready

### ❓ What Happens:
The **Jira Planner Sub-Agent** (`jira-planner.md`) validates the SRS against the **Definition of Ready (DoR)** (checks for API payloads, schemas, and auth). It strictly enforces **Vertical Slicing** (combining Command, Aggregate, Event, and Projection into one working slice per ticket) and outputs a **Sprint Tracking Sheet** with estimations and buffer times.

### 💻 Commands to Trigger Stage 2:

#### In Google Antigravity:
```text
Act as @jira-planner.md. 

Read @docs/features/catalog-srs.md. 
1. Validate that it satisfies the Definition of Ready (DoR).
2. Break it down into vertical use-case sub-tasks (Command ➔ Aggregate ➔ Event ➔ Read Model).
3. Output the Sprint 1 Tracking Sheet with base estimations, +25% buffer time, and status tracking.
```

#### In Claude Code:
```bash
claude "Act as the planner in .agents/roles/jira-planner.md. Read docs/features/catalog-srs.md, validate the Definition of Ready, and break it down into vertical use-case tickets. Output the Sprint 1 Tracking Sheet with base estimates and buffer time."
```

### 📋 Deliverable: Sprint 1 Feature Tracking Sheet (`docs/planning/sprint-1-tracker.md`)
Generated using `.agents/docs/sprint-tracking-template.md`:

| Ticket Key | Vertical Slice / Use Case | Sprint | Base Est. | Buffer (+25%) | Total Est. | Status | PR / Branch | Dependencies |
| :--- | :--- | :---: | :---: | :---: | :---: | :---: | :---: | :--- |
| **[PROJ-101](https://jira.org/PROJ-101)** | `Create Product` Flow (Command ➔ Aggregate ➔ Event ➔ Read Model) | Sprint 1 | 4h | +1h | **5h** | ⚪ To Do | `branch/PROJ-101` | MongoDB setup |
| **[PROJ-102](https://jira.org/PROJ-102)** | `Update Product Price` Flow (Command ➔ Aggregate ➔ Event ➔ Read Model) | Sprint 1 | 3h | +1h | **4h** | ⚪ To Do | — | PROJ-101 |
| **[PROJ-103](https://jira.org/PROJ-103)** | `Deactivate Product` Flow (Command ➔ Aggregate ➔ Event ➔ Read Model) | Sprint 1 | 3h | +1h | **4h** | ⚪ To Do | — | PROJ-101 |
| **[PROJ-104](https://jira.org/PROJ-104)** | Unified E2E Integration Test Suite (Testcontainers + MassTransit) | Sprint 1 | 4h | +2h | **6h** | ⚪ To Do | — | PROJ-101..103 |
| **TOTALS** | *Sprint 1 Scope* | | **14h** | **+5h** | **19h** | *0% Complete* | | **Safe Delivery: ~2.5 Days** |

#### 🔄 How this Sheet Updates Automatically During the Sprint:
As the sub-agents execute subsequent stages, they edit `docs/planning/sprint-1-tracker.md` automatically:
1. **Developer starts PROJ-101:** Changes status from `⚪ To Do` ➔ `🔵 In Progress` and records `branch/PROJ-101`.
2. **Tester runs tests:** When `dotnet test` passes, changes status ➔ `🧪 Tests Passed`.
3. **Reviewer audits code:** Marks `🟢 Approved` (or `🔴 Changes Requested`).
4. **Skill runs `create-pull-request`:** Marks `🟡 In PR Review` and automatically pastes the created PR link (e.g. `[#1](https://github.com/org/repo/pull/1)`).
5. **PR Merged:** Row is finalized as `🟢 Done`.

---

## Stage 3: Clean Architecture Implementation

### ❓ What Happens:
The **Backend Developer Sub-Agent** (`backend-developer.md`) takes the sub-tasks, creates a feature branch, implements the C# CQRS components, wires DI, cleans up unused code, adds OpenTelemetry structured logs, and updates the living flow doc.

### 💻 Commands to Trigger Stage 3:

#### In Google Antigravity:
```text
Act as @backend-developer.md. 

Implement Sub-Tasks #1 & #2 for the Catalog feature:
- Reference the schema in @docs/features/catalog-srs.md.
- Follow the C# skeletons in .ai/templates/.
- Wire DI according to .ai/di-registration-guide.md.
- Adhere to @global-coding-standards.md (OpenTelemetry structured logging, clean usings).
- Update .ai/flows/catalog.md when complete.
```

#### In Claude Code:
```bash
claude "Act as the developer in .agents/roles/backend-developer.md. Implement CreateProductCommand, Product aggregate, and DI registration following docs/features/catalog-srs.md and .ai/templates/. Update .ai/flows/catalog.md when done."
```

**Deliverable Generated:** C# files in `src/Domain/`, `src/Application/`, `src/Read/`, plus an updated `.ai/flows/catalog.md` living doc.

---

## Stage 4: Unified Event-Driven Testing

### ❓ What Happens:
The **Test Engineer Sub-Agent** (`test-engineer.md`) writes domain unit tests and end-to-end integration tests combining real MongoDB and the MassTransit bus.

### 💻 Commands to Trigger Stage 4:

#### In Google Antigravity:
```text
Act as @test-engineer.md. 

Write the test suite for the new Catalog feature:
1. Tier 1: Aggregate Unit Tests in tests/Aggregates.UnitTest/ verifying SKU uniqueness and ProductCreatedEvent staging.
2. Tier 2: Unified Integration Test in tests/ProjectTemplate.IntegrationTest/ connecting real MongoDB and MassTransit harness.
3. Run dotnet test and verify all tests pass.
```

#### In Claude Code:
```bash
claude "Act as the test engineer in .agents/roles/test-engineer.md. Write Tier 1 unit tests and Tier 2 unified MongoDB + MassTransit integration tests for the Catalog feature. Execute dotnet test and report results."
```

**Automated Command Run Behind the Scenes:**
```bash
docker compose -f docker-compose.infra.yml up -d
dotnet test
```

---

## Stage 5: Architectural Code Review

### ❓ What Happens:
The **Senior Code Reviewer Sub-Agent** (`code-reviewer.md`) audits the git diff against the 10 core architectural rules, DI registration, templates, and OpenTelemetry standards.

### 💻 Commands to Trigger Stage 5:

#### In Google Antigravity:
```text
Act as @code-reviewer.md. 

Audit all files modified for the Catalog feature against @general-rules.md, @global-security.md, and .ai/templates/. 
Output the complete review report with Pass/Fail matrix and final status.
```

#### In Claude Code:
```bash
claude "Act as the reviewer in .agents/roles/code-reviewer.md. Review all git changes against .agents/rules/general-rules.md and .agents/rules/global-security.md. Output the structured markdown review report."
```

### 📋 Review Report Output:
- **Status:** `✅ Approve` (or `❌ Request Changes` with exact code snippets for fixes)
- Compliance table across CQRS, templates, DI, security, and living flow docs.

---

## Stage 6: Automated Pull Request & GitHub Approval

### ❓ What Happens:
Once approved, the AI commits the changes, pushes the branch, opens the PR, and posts the review report directly to GitHub.

### 💻 Commands to Trigger Stage 6:

#### In Google Antigravity:
```text
The review is approved. 
1. Run the @create-pull-request skill to commit, push, and open the PR.
2. Run the @post-pr-review skill to submit the AI approval report on GitHub.
```

#### In Claude Code:
```bash
claude "The code review is approved. Follow .agents/skills/create-pull-request/SKILL.md to commit and open a PR, then follow .agents/skills/post-pr-review/SKILL.md to submit the review."
```

