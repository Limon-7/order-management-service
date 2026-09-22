---
trigger: always_on
---

# Global Tech Stack Rules

These rules apply to all AI agents working on this project.

- **Primary Stack:** .NET 8 / 9 / 10 (flexible; inspect `Directory.Build.props` or project files for target TFM, default to .NET 10), C# (latest supported by target TFM), MongoDB (Driver 2.x).
- **Dependency Strictness:** ONLY use libraries and NuGet packages already defined in the project's `Directory.Packages.props` or `global.json`. Do NOT introduce new third-party packages, frameworks, or testing tools without explicit user permission.
- **Technology Drift:** Do not suggest or introduce new infrastructure components (e.g., suggesting Kafka if the project uses RabbitMQ) unless explicitly requested.
