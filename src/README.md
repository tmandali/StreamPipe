# Source layout

This directory is reserved for SDK implementations. Specifications are authoritative; source code is a conforming implementation artifact.

The initial .NET solution is planned to contain:

- `StreamPipe.Abstractions` — public contracts and shared primitives
- `StreamPipe.Core` — protocol runtime and state machines
- `StreamPipe.Protocols.Arrow` — Arrow IPC format integration
- `StreamPipe.Transports.Http` — HTTP transport profile
- `StreamPipe.Client` — client-facing composition API
- `StreamPipe.Server` — server-facing composition API

Additional language SDKs belong in separate repositories unless a future governance decision changes that policy.
