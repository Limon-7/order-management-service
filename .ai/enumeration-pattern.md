# Enumeration Pattern

This project uses a custom `Enumeration` base class instead of C# `enum`. This provides richer behavior including display names, type safety, and MongoDB serialization support.

## Base Class

Located at: `src/Shared/Common/Enums/Enumeration.cs`

## How to Create a New Enumeration

```csharp
namespace {{RootNamespace}}.Shared.Common.Enums;

public class MyStatus(int value, string displayName) : Enumeration(value, displayName)
{
    public static readonly MyStatus Active = new(1, nameof(Active));
    public static readonly MyStatus Inactive = new(2, nameof(Inactive));
    public static readonly MyStatus Suspended = new(3, nameof(Suspended));
}
```

## Key Rules

1. **Use Primary Constructor** with `(int value, string displayName)` params.
2. **Static readonly fields** for each value — these are the "enum members".
3. **Use `nameof()`** for `displayName` to keep display names in sync with field names.
4. **Start values at 1**, not 0 (convention in this project).
5. **Namespace**: `{{RootNamespace}}.Shared.Common.Enums` (flat).
6. **File location**: `src/Shared/Common/Enums/[EnumName].cs`.

## Available API (from base class)

```csharp
// Get all values
IEnumerable<MyStatus> all = Enumeration.GetAll<MyStatus>();

// Lookup by integer value
MyStatus status = Enumeration.FromValue<MyStatus>(1); // Active

// Lookup by display name
MyStatus status = Enumeration.FromDisplayName<MyStatus>("Active");

// Comparison
bool isEqual = MyStatus.Active == someStatus;
int comparison = MyStatus.Active.CompareTo(MyStatus.Inactive);

// Properties
int value = MyStatus.Active.Value;       // 1
string name = MyStatus.Active.DisplayName; // "Active"
string str = MyStatus.Active.ToString();   // "Active"
```

## When to Use

- **Use `Enumeration`** for: Status types, category types, or any domain concept where you need display names and are stored in MongoDB.
- **Use C# `enum`** for: Internal-only flags or simple switches that don't get persisted.

## Existing Enumerations

<!-- PROJECT-SPECIFIC: List your project's enumerations here -->
| File | Values |
|------|--------|
| *Add your project's enumerations* | *List their values* |
