# Architecture Decision Records (ADR)

A running log of key architecture and design decisions made in this project.
Never delete entries — append with date if a decision was revised.

---

## ADR-001 — CQRS Physical Separation
**Date:** <!-- Add date -->
**Status:** Active
**Decision:** Write and Read sides are physically separated into different projects/layers. Commands go through CommandHandlers and AggregateRoots. Queries go directly through QueryHandlers to the read database.
**Reason:** Enforce strict separation of concerns, allow independent scaling of read/write paths.

---

## ADR-002 — MongoDB as Primary Database
**Date:** <!-- Add date -->
**Status:** Active
**Decision:** MongoDB (Driver 2.x) is used for both read and write stores. `ITransactionalRepository` for writes, `IReadRepository` for reads.
**Reason:** Document model flexibility for evolving bounded contexts.

---

## ADR-003 — Event-Driven via MassTransit + RabbitMQ
**Date:** <!-- Add date -->
**Status:** Active
**Decision:** All cross-aggregate and cross-service communication is done via domain events published through MassTransit over RabbitMQ.
**Reason:** Decouples bounded contexts; enables saga orchestration for complex workflows.

---

<!-- Add project-specific ADRs below -->
