# Source layout

This directory is reserved for SDK implementations. Specifications are authoritative; source code is a conforming implementation artifact.

Current .NET projects:

- `StreamPipe.Abstractions` — public data-model and streaming contracts (`DataSchema`, `LogicalType`, `IDataStream`, …)
- `StreamPipe.Core` — reserved for shared runtime; empty until wire/streaming SPSS documents are ready

Planned follow-on projects:

- `StreamPipe.Protocols.Arrow` — Arrow IPC format integration
- `StreamPipe.Transports.Http` — HTTP transport profile
- `StreamPipe.Client` — client-facing composition API
- `StreamPipe.Server` — server-facing composition API

Additional language SDKs belong in separate repositories unless a future governance decision changes that policy.
