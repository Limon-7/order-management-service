---
trigger: always_on
---

# Global Coding Standards

These rules apply to all AI agents working on this project.

- **Strict Formatting:** Adhere strictly to standard C# naming conventions and formatting guidelines (e.g., PascalCase for classes/methods, camelCase for local variables). Ensure consistent indentation and bracket placement. Do not introduce arbitrary whitespace changes unless formatting a completely new file.
- **Remove Unused Code:** When refactoring or updating files, you MUST aggressively remove any unused code, dead variables, unreachable blocks, or commented-out legacy code.
- **Clean Usings:** Always clean up and remove unused `using` statements at the top of C# files.
- **Structured Logging (OpenTelemetry):** Do not leave `Console.WriteLine` or arbitrary debug logs. You MUST use the injected `ILogger` interface and implement strict **structured logging** suitable for OpenTelemetry. Always use message templates with named placeholders (e.g., `_logger.LogInformation("Processing order {OrderId}", order.Id)`) so that telemetry backends can index the properties.
- **Keep it Clean:** Never leave "TODO" comments in the code unless explicitly requested by the user. If you solve a problem, implement it fully.
