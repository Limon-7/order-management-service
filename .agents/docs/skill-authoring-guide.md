# AI Skill Authoring: Best Practices

Skills are active, multi-step procedures that teach the AI how to perform complex workflows in your repository. Unlike Rules (which are passive constraints), Skills are loaded on-demand when the AI decides it needs to execute a specific task.

Follow these best practices to ensure your AI agents execute workflows flawlessly.

## 1. Always Use YAML Frontmatter
Because of **progressive disclosure**, the AI will initially only see the name and description of the skill. It uses this information to decide whether to load the full skill file.
- **`name`**: Use lowercase, kebab-case (e.g., `run-database-migration`).
- **`description`**: Be highly specific about *when* the AI should use this skill.

```yaml
---
name: run-database-migration
description: Use this skill when you need to generate a new EF Core database migration and update the local database.
---
```

## 2. Use Deterministic, Numbered Steps
AI agents perform best when given a strict sequence of events. Avoid paragraphs; use numbered lists with exact terminal commands.
- **Bad:** "Go ahead and create a migration using the dotnet cli and then update the database."
- **Good:** 
  1. Navigate to `src/Infrastructure`.
  2. Run `dotnet ef migrations add <Name>`.
  3. Run `dotnet ef database update`.

## 3. Include Prerequisites
If a skill requires a specific environment state, explicitly state it at the top.
- "Prerequisite: Ensure Docker daemon is running."
- "Prerequisite: The user must be authenticated with AWS CLI."

## 4. Build-In Verification & Fallbacks
A robust skill doesn't just run a command; it verifies the outcome and provides troubleshooting steps if it fails.
- **Verification:** "After running the tests, verify that the console outputs `0 Failed`."
- **Fallback:** "If the database update fails due to a locked file, instruct the user to stop the local API server and try again."

## 5. Keep It Focused (Single Responsibility Principle)
A skill should do one thing perfectly. Do not combine "Create a Migration" and "Deploy to Staging" into a single skill. If a workflow is massive, break it into smaller skills and have the AI chain them together.

---

## 📝 Example of a Perfect Skill Template

Create this file at: `.agents/skills/run-unit-tests/SKILL.md`

```markdown
---
name: run-unit-tests
description: Use this skill to execute the backend xUnit test suite and generate a coverage report.
---

# Run Unit Tests

## Prerequisites
- You must be in the repository root.

## Execution Steps
1. Navigate to the `tests/` directory: `cd tests/`
2. Run the tests with the coverage flag: 
   `dotnet test --collect:"XPlat Code Coverage"`
3. If tests fail, STOP and read the failure output. Do not proceed.
4. If tests pass, navigate back to the root: `cd ../`

## Verification
- Look for the `Test Run Successful` message in the console output.
- If you see `Failed: [number]`, read the logs, locate the failing test file, and propose a fix to the user.
```

