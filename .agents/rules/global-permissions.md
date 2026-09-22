---
trigger: always_on
---

# Global Agent Permissions & Boundaries

These rules govern the autonomous execution permissions and operational boundaries for all AI agents in this repository.

## 0. Code Generation Pre-Condition: "No Plan, No Code"
- **Strict Prohibition:** Agents are **strictly prohibited** from writing, modifying, or scaffolding production code in `src/` without an approved **Task Plan** (Jira ticket `PROJ-xxx` or Defect ticket `BUG-xxx`) linked to an approved SRS requirement document in `docs/features/`.
- **Enforcement:** If a user prompts directly for code generation without providing or referencing a task plan, the agent must **HALT** and guide the user through Stage 1 (SRS Discovery) and Stage 2 (Task Breakdown).

---

## 1. Terminal Command Execution Permissions

### ✅ Auto-Approved (Autonomous Execution Permitted)
The agent is permitted to execute the following non-destructive commands without prompting for user confirmation:
- **Build & Restore:** `dotnet build`, `dotnet restore`.
- **Testing:** `dotnet test` (all test runners).
- **Inspection & Status:** `git status`, `git diff`, `git log`, file listing, directory inspection.
- **Code Generation & Scaffolding:** creating or modifying files within the authorized boundaries.

### ⚠️ Mandatory User Confirmation Required
The agent **MUST** explicitly ask the user for permission before running any of the following:
- **Destructive Git Operations:** `git reset --hard`, `git clean -f`, `git push --force`, deleting branches, rebasing shared branches.
- **Destructive File System Actions:** `rm -rf`, `Remove-Item -Recurse`, deleting entire folders or configuration directories.
- **Database Modifying Actions:** Dropping databases, clearing MongoDB collections, rolling back migrations, or executing raw database migration scripts against non-local environments.
- **Dependency Changes:** Adding, updating, or removing package versions in `Directory.Packages.props` or `global.json`.
- **Non-Local Environments Forbidden:** Any attempt to execute commands, tests, or migrations against QA, Staging, or Production connection strings is strictly prohibited without explicit, written confirmation. All autonomous actions must target `Local` (Docker/localhost) or designated `Dev` instances.

---

## 2. File & Workspace Boundaries

### Allowed Modification Zones:
- `src/`: Application, Domain, Infrastructure, Read, Saga, and Shared projects.
- `tests/`: Unit and integration test projects.
- Local development configs: `appsettings.Development.json`, `appsettings.DockerLocal.json`.

### Protected Zones (Ask Before Modifying):
- **Production & Shared Configs:** `appsettings.prod.json`, `appsettings.stg.json`, `appsettings.qa.json`.
- **Build Infrastructure:** `Directory.Build.props`, `Directory.Packages.props`, `global.json`, `docker-compose.yml`.
- **CI/CD Pipelines:** `.gitlab-ci.yml`, `.github/workflows/`, deployment scripts.

---

## 3. Secrets & Sensitive Data Handling
- **No Ingestion/Output of Secrets:** Never print, log, or export actual API keys, database connection strings with passwords, or JWT secrets in code, logs, or chat outputs.
- **Placeholder Usage:** Always use environment variable references or placeholders (e.g., `<YOUR_MONGODB_CONNECTION_STRING>`).

