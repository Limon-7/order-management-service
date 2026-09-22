---
trigger: always_on
---

# Global Security Rules

These rules apply to all AI agents working on this project.

- **Zero Secrets:** NEVER hardcode secrets, API keys, passwords, tokens, or connection strings in the codebase or output.
- **Authorization & Authentication:** Ensure every API endpoint, Command, and Query handler explicitly dictates or validates user authorization levels/roles (RBAC/Claims) prior to execution.
- **Assume Malicious Input:** Always validate inputs. Rely on structured validation (e.g., FluentValidation) before processing any commands.
- **No Injection Vulnerabilities:** Avoid dynamic query building that could lead to NoSQL or SQL injection vulnerabilities.
- **Environment Isolation (Local/Dev Only):** AI agents must ONLY ever connect to, query, or execute against **Local** (Docker / localhost) or designated **Dev** environments. NEVER target, connect to, run migrations against, or test against Staging, QA, or Production databases and servers.
- **Environment Isolation (Local/Dev Only):** AI agents must ONLY ever connect to, query, or execute against **Local** (Docker / localhost) or designated **Dev** environments (including deployed Dev MongoDB clusters configured in `appsettings.dev.json` or User Secrets). NEVER target, connect to, run migrations against, or test against Staging, QA, or Production databases and servers.
- **Deployed Dev Database Safety:** When interacting with a deployed Dev DB, the agent is forbidden from running destructive operations (`dropDatabase` or dropping shared collections) without explicit confirmation, and must mask all credentials in output.
