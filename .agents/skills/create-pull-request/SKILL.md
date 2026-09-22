---
name: create-pull-request
description: Use this skill to commit approved code, push it to the remote repository, and create a GitHub Pull Request with a descriptive summary.
---

# Create Pull Request

## Prerequisites
- The code must have passed the `@code-reviewer.md` agent's checks.
- You must be on a feature branch (not `main` or `master`).
- The GitHub CLI (`gh`) must be installed and authenticated.

## Execution Steps

1. **Check Status:**
   Run `git status` to ensure you know exactly which files have been modified.

2. **Stage Changes:**
   Run `git add .` to stage all approved modifications.

3. **Commit Code:**
   Create a conventional commit message. If there is a Jira ticket or Issue number in the branch name (e.g., `feature/PROJ-123-add-category`), include it.
   Run: `git commit -m "feat(scope): [Ticket] Brief description of changes"`

4. **Push Branch:**
   Push the branch to the remote repository.
   Run: `git push -u origin HEAD`

5. **Create the Pull Request:**
   Use the GitHub CLI to create the PR. Generate a detailed body summarizing the architectural changes, tests added, and linking the Jira ticket.
   Run: 
   ```bash
   gh pr create --title "feat: [Ticket] Feature Name" --body "## Summary
   Brief description of the changes.
   
   ## Technical Details
   - List of architectural changes (CQRS, etc)
   - Tests added
   
   ## Reviewer Notes
   Passed automated Code Reviewer agent checks."
   ```

6. **Update Sprint Tracker Sheet:**
   Open the active sprint tracker in `docs/planning/sprint-*-tracker.md` (or the tracking table in the SRS doc), locate the ticket's row:
   - Update **Status** to `🟡 In PR Review`.
   - Update **PR / Branch** column with the clickable markdown link to the created PR (e.g., `[#1](https://github.com/org/repo/pull/1)`).

## Verification
- Verify the console outputs a URL to the newly created Pull Request (e.g., `https://github.com/org/repo/pull/1`).
- Verify the tracking table in `docs/planning/sprint-*-tracker.md` shows `🟡 In PR Review` with the new PR link.
- If `gh` is not authenticated, instruct the user to run `gh auth login` and STOP.
