---
trigger: always_on
---

# Global Architecture Rules

These rules apply to all AI agents working on this project.

- **Domain Driven Design (DDD):** Encapsulate business logic within `AggregateRoots`. Do not place domain validation logic in API Controllers or Application layers.
- **CQRS Strictness:** Maintain a strict physical separation between Read and Write operations.
  - Write operations must use `Commands` and `CommandHandlers`.
  - Read operations must use `Queries` and `QueryHandlers`.
  - NEVER inject an `IReadRepository` into a `CommandHandler`.
- **Event-Driven:** State changes must raise Domain Events.
- **Database Access:** All DB calls must be async. Access MongoDB exclusively through `ITransactionalRepository` (writes) and `IReadRepository` (reads).
- **No Direct Queries:** NEVER execute direct database queries from the Domain layer.
