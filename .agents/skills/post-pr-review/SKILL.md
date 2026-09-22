---
name: post-pr-review
description: Use this skill to take the results of an AI Code Review and officially post them to the active GitHub Pull Request using the GitHub CLI.
---

# Post Pull Request Review

## Prerequisites
- A Pull Request must already exist for the current branch.
- The GitHub CLI (`gh`) must be installed and authenticated.
- You must have completed your code review using `@code-reviewer.md` and decided on a Status (`Approve` or `Request Changes`).

## Execution Steps

1. **Format the Feedback:**
   Write your full structured review report to a temporary file named `temp_review_body.md` in the root directory.
   
   Ensure the content follows the format defined in `@code-reviewer.md`:
   - Summary status table (`general-rules.md`, `templates`, DI, security, namespaces, tests, OpenTelemetry).
   - Detailed findings with exact file paths, line numbers, rule citations, and snippet fixes.
   - Clear verdict.

   *Example content for `temp_review_body.md`:*
   ```markdown
   # 🔍 AI Code Review Report

   **Status:** ❌ Request Changes
   **Reviewer:** Senior Code Reviewer Agent

   ### 📋 Architectural & Rule Compliance Summary
   | Category | Status | Notes |
   | :--- | :---: | :--- |
   | **General Rules & CQRS (`general-rules.md`)** | 🔴 Fail | Rule #2: Missing DI registration |
   | **Template Adherence (`/.ai/templates/`)** | 🟢 Pass | Matches command-handler.cs |
   | **DI Registration (`di-registration-guide.md`)** | 🔴 Fail | Handler not wired up in ServiceCollection |
   | **Namespaces (`namespace-conventions.md`)** | 🟢 Pass | Proper application namespace |
   | **Security & Authorization** | 🟢 Pass | Validated claims |
   | **Tests & Coverage** | 🟢 Pass | Unit tests cover failure paths |
   | **OpenTelemetry & Code Hygiene** | 🟢 Pass | Structured logging in place |

   ### 🔎 Detailed Findings & Required Changes
   #### ❌ Missing DI Registration for CommandHandler
   - **File:** `src/Application/Categories/CreateCategoryCommandHandler.cs#L24`
   - **Rule Violated:** `.agents/rules/general-rules.md` Rule #2 & `di-registration-guide.md`
   - **Suggested Fix:**
   ```csharp
   services.AddTransient<ICommandHandlerAsync<CreateCategoryCommand>, CreateCategoryCommandHandler>();
   ```

   ### 🏁 Final Verdict
   Please register the handler in DI and re-run tests. Once resolved, request a re-review.
   ```

2. **Execute the GitHub CLI Command:**
   Run the appropriate `gh pr review` command based on your final decision, passing the temporary file as the body.

   - **If Approved:**
     Run: `gh pr review --approve -F temp_review_body.md`
   
   - **If Requesting Changes:**
     Run: `gh pr review --request-changes -F temp_review_body.md`
   
   - **If just leaving a neutral comment:**
     Run: `gh pr review --comment -F temp_review_body.md`

3. **Cleanup:**
   Delete the temporary file so it doesn't clutter the workspace.
   Run (Windows/PowerShell): `Remove-Item temp_review_body.md`
   Run (Mac/Linux): `rm temp_review_body.md`

## Verification
- Verify the terminal outputs success (e.g., `Approved pull request #1` or `Submitted review for pull request #1`).
- If you receive an error stating "no pull requests found for branch", inform the user that a PR must be created first (they can use the `create-pull-request` skill).
