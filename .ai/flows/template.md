# [Feature Flow Name - e.g., Category Management]

## 📌 Metadata & Traceability
- **Status:** ⚪ Not Started | 🔵 In Progress | 🟢 Complete
- **Jira Tickets:** `[PROJ-101]` (Parent Story: `[PROJ-100]`)
- **SRS Reference:** `docs/features/<feature>-srs.md`
- **Feature Branch:** `feature/PROJ-101-<name>`

---

## 🎯 Purpose & Scope
[2-4 sentences explaining the business purpose and boundary of this vertical slice/feature flow]

---

## 🗂️ File Inventory & Impact Matrix

### 🆕 Files to Create (New Files)
| Layer | Target File Path | Purpose |
| :--- | :--- | :--- |
| **Application** | `src/Application/Commands/[CommandName].cs` | Write command contract |
| **Application** | `src/Application/CommandValidators/[ValidatorName].cs` | FluentValidation rules |
| **Application** | `src/Application/CommandHandlers/[HandlerName].cs` | Orchestrates aggregate & repository |
| **Domain** | `src/Domain/Aggregates/[AggregateName].cs` | AggregateRoot with business invariants |
| **Domain** | `src/Domain/Events/[EventName].cs` | Domain event raised upon mutation |
| **Read** | `src/Read/ViewModels/[ViewModelName].cs` | Read-side MongoDB projection model |
| **Read** | `src/Read/EventHandlers/[EventHandlerName].cs` | Consumes event & updates read model |
| **Tests** | `tests/Aggregates.UnitTest/[AggregateName]Tests.cs` | Invariant unit tests |
| **Tests** | `tests/[ProjectName].IntegrationTest/[Feature]Tests.cs` | E2E integration test (Write ➔ Read) |

### ✏️ Existing Files Impacted (Modified Code)
| File Path | Modification Summary |
| :--- | :--- |
| `src/Application/CommandWorker/Extensions/ServiceCollectionExtensions.cs` | Register CommandHandler & Validator in DI |
| `src/Read/EventWorker/Extensions/ServiceCollectionExtensions.cs` | Register EventHandler & ReadRepository in DI |
| `src/Shared/SharedDto/` *(if shared)* | Add shared event / message contract |

---

## 💡 Key Architectural Decisions & Invariants
- `[YYYY-MM-DD]` **[Decision Title]:** [Why this technical approach or invariant was chosen]
- **Business Invariant:** [E.g., "Category Name must be unique within a Tenant"]

---

## 📋 Remaining Work / Edge Cases
- [ ] [Pending subtask or future enhancement]
- [ ] [Edge case to cover in next sprint]

