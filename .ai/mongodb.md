# MongoDB Usage

Database: MongoDB via `MongoDB.Driver`.

## Configuration & Connection Strings

The repository layer supports dual configuration modes:

1. **Local Development Mode (`Development` profile):**
   - Microservices read database connection strings and database names directly from `appsettings.Development.json`.
   - `ServiceRegistrationsFilePath` is left empty (`""`).
   - Workload separation:
     - Write Side (`CommandWorker`): `{{ServiceId}}_write_db` via `ITransactionalRepository` / `IAggregateRootRepository`
     - Read Side (`EventWorker` / `BusinessApiService`): `{{ServiceId}}_read_db` via `IReadRepository`
     - Saga State (`SagaWorker`): `{{ServiceId}}_saga_state_db` via `IStateRepository`

2. **Deployed / Container Mode (`DockerLocal`, `dev`, `qa`, `stg`, `prod`):**
   - Database connection strings and service endpoints are loaded dynamically from `ServiceRegistrations.json` via `ServiceRegistrationsFilePath`.
   - Paths are **OS-independent** (e.g. `./config/...` in Docker Compose or `/app/Settings/...` in Linux containers).

## Repositories & Unit of Work (Write Side)
- Handled internally by `Platform.Infrastructure.Repository.MongoDb`.
- Aggregates are persisted into collections implicitly mapped by name (`$"{typeof(T).Name}s"`).
- Always use `ITransactionalRepository` in combination with `IMongoUnitOfWork` for complex write sequences to ensure ACID behaviors (via `BeginTransactionAsync` / `CommitTransactionAsync`).
- Domain models should be annotated with `[BsonIgnoreExtraElements]`.

## Repositories (Read Side)
- Handled by `IReadRepository`.
- Used heavily in Query Handlers and Event Handlers (for Projection building).

## Repositories (Saga State)
- Handled by `IStateRepository` via `builder.Services.AddStateRepository()`.

## Best Practices
- Use async operations only (e.g., `InsertOneAsync`, `ReplaceOneAsync`, `DeleteOneAsync`).
- Avoid exposing direct MongoDB Driver types (like `FilterDefinition`) heavily in the Application tier. Rely on `Expression<Func<T, bool>>` provided by the repository interfaces.
