# Copilot Instructions

## Coding Standards

Keep methods short, readable, and single-purpose.

Classes should be shorter than 1000 lines.

Methods should be shorter than 100 lines.

Prefer primary constructors.

No underscore prefixes; use camelCase naming.

Favor var and asynchronous patterns (async/await).

Convert collections into lists or arrays if accessed multiple times.

## Architecture Principles

Adhere to Volatility-Based Decomposition (VBD).

Use clearly separated layers: UI, Business Logic, Data Access, Infrastructure.

Favor extensibility and modularity; anticipate future changes.

### Volatility-Based Decomposition (VBD)

Clients: User interfaces; callable by end-users.

Managers: Handle workflows; callable by Clients.

Engines: Business logic and transformations; callable by Managers.

Accessors: Data persistence; callable by Managers and Engines.

Resources: External services; callable only by Accessors.

Ifx: Infrastructure; callable by all layers.  Can be solution specific.

Utility: Shared code; callable by all layers.  Intended to be shared across mulitple solutions.

### Layer Responsibilities

#### Traditional 3 layer view:

- UX Layer
  - Web Apps
  - Native Apps
  - Web Api
- Business Layer
  - Managers
  - Engines
- Data Layer
  - Accessors
  - Resources

#### Layer Responsibilities (VBD view):

- Clients
    - Web Apps
    - Native Apps
    - Web Apis
- Managers
- Engines
- Accessors
- Resources

- Cross-Cutting Concerns: Available to all layers
    - Infrastructure
    - Utility
    
### Communication Constraints

- Clients → Managers
- Managers → Engines / Accessors
- Engines → Accessors
- Accessors → Resources

All communication between layers is via interfaces.

No crosstalk between components within a layer.

No calling up to a layer above.

No sharing data/models acquired from a layer below.

### Message Bus Communication

Only clients and managers can communicate with the message bus.

- Clients → Message Bus
- Managers → Message Bus
- Message Bus → Clients
- Message Bus → Managers

## Technology Stack

IDE: Visual Studio, VSCode, Blend

Language: C# (.NET 9, .NET 8)

ORM: EntityFrameworkCore

UI: OpenApi, React/WebApi, WPF, WinUI, Terminal

## Extensibility and Maintainability

Define clear interfaces between components.

Keep methods short, focused, and single-purpose.

Anticipate future changes by isolating frequently changing logic.

## Quality Guidelines

Always write unit tests with at least 80% code coverage.

## Testing Guidelines

Automate tests as part of the build pipeline.

Focus unit tests around Engines and Accessors; integration tests around Managers.

## Database Conventions

Clearly separate read/write access.

Never allow direct queries outside Accessors.

Enable CQRS for all Accessors.  

Plan for change.