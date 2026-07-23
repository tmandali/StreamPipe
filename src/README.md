# Source layout

This directory contains .NET SDK implementations. Specifications in `spec/spss/` remain authoritative.

Current foundation projects:

- `StreamPipe.Abstractions` — public contracts and shared primitives
- `StreamPipe.Core` — protocol-independent validation and lifecycle primitives

Planned follow-on projects:

- `StreamPipe.Protocols.Arrow` — Arrow IPC format integration
- `StreamPipe.Transports.Http` — HTTP transport profile
- `StreamPipe.Client` — client-facing composition API
- `StreamPipe.Server` — server-facing composition API

Additional language SDKs belong in separate repositories unless a future governance decision changes that policy.
