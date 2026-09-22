# 📋 Sprint Tracking Sheet Template

Use this template to track Jira tickets, estimations, buffer times, and PR statuses for each sprint.

---

## 🚀 Sprint Details
- **Sprint Name:** Sprint [Number] - [Feature / Module Name]
- **Start Date:** YYYY-MM-DD
- **End Date:** YYYY-MM-DD
- **Sprint Goal:** [1-2 sentences summarizing the core business outcome]

---

## 📊 Vertical Slice Ticket Tracker

| Ticket Key | Parent Story | Use Case / Vertical Slice | SRS Reference | Base Est. | Buffer (+25%) | Total Est. | Status | PR / Branch | Dependencies |
| :--- | :--- | :--- | :--- | :---: | :---: | :---: | :---: | :---: | :--- |
| **[PROJ-101](https://tim.brac.net/browse/PROJ-101)** | `[PROJ-100]` | `Create [Entity]` Flow | `SRS §3.1` | 4h | +1h | **5h** | ⚪ To Do | `branch/PROJ-101` | None |
| **[PROJ-102](https://tim.brac.net/browse/PROJ-102)** | `[PROJ-100]` | `Update [Entity]` Flow | `SRS §3.2` | 3h | +1h | **4h** | ⚪ To Do | — | PROJ-101 |
| **[PROJ-103](https://tim.brac.net/browse/PROJ-103)** | `[PROJ-100]` | `Delete / Archive [Entity]` Flow | `SRS §3.3` | 3h | +1h | **4h** | ⚪ To Do | — | PROJ-101 |
| **[PROJ-104](https://tim.brac.net/browse/PROJ-104)** | `[PROJ-100]` | Unified E2E Integration Test Suite | `SRS §5.0` | 4h | +2h | **6h** | ⚪ To Do | — | PROJ-101..103 |
| **[PROJ-105](https://tim.brac.net/browse/PROJ-105)** | `[PROJ-100]` | OpenAPI Contract Verification & Smoke Tests | `SRS §2.0` | 2h | +1h | **3h** | ⚪ To Do | — | All |
| **TOTALS** | *Sprint Scope* | | | **16h** | **+6h** | **22h** | *0% Complete* | | **Safe Delivery: ~3 Days** |

---

## 🚦 Status Legend
- ⚪ **To Do:** Ready for pickup (Meets Definition of Ready).
- 🔵 **In Progress:** Developer agent actively working on this vertical slice.
- 🟡 **In Review:** Pull Request created; waiting for Code Reviewer / human approval.
- 🟢 **Done:** PR approved, merged to `main`, living flow doc updated.
- 🔴 **Blocked:** Dependency or environment issue.

---

## 🛡️ Estimation & Buffer Rules
1. **Vertical Slices Only:** Every ticket must deliver an end-to-end slice (never separate Aggregate from its Command).
2. **Buffer Time Standard:** Always allocate **+25% buffer** on development tickets and **+50% buffer** on integration test tickets to account for infrastructure and deserialization debugging.
3. **Capacity Cap:** Total sprint commitment must not exceed 80% of team capacity to absorb sprint-level uncertainty.

