# StreamPipe

StreamPipe is a specification-first initiative for a language- and transport-independent streaming data protocol and SDK ecosystem.

The project defines the StreamPipe Standard Specification (SPSS): a normative contract for bounded-memory streaming, backpressure, schema-aware data exchange, and independently implemented SDKs. This repository is the source of truth for the specification; SDK code is a conforming implementation artifact.

## Repository layout

- `spec/spss/` — normative StreamPipe standards
- `spec/adr/` — architecture decision records
- `spec/examples/` — informative protocol and API examples
- `spec/diagrams/` — source-controlled diagrams
- `spec/templates/` — authoring templates
- `src/` — .NET SDK foundation (`StreamPipe.Abstractions`, `StreamPipe.Core`)
- `tests/` — unit and conformance tests
- `.github/` — issue and pull-request templates

## Build and test (.NET)

Prerequisites: [.NET 10 SDK](https://dotnet.microsoft.com/download).

```bash
dotnet build StreamPipe.sln
dotnet test StreamPipe.sln
```

The first milestone implements public data-model abstractions and protocol-independent validation/lifecycle primitives only. Wire framing, transports, Arrow, and network sessions are intentionally out of scope until their SPSS documents are accepted.

## Status

The specification is in its foundation phase. No wire-format behavior is normative until its corresponding SPSS document is accepted.

## Contributing

Read [AGENTS.md](AGENTS.md), [GOVERNANCE.md](GOVERNANCE.md), [CONTRIBUTING.md](CONTRIBUTING.md), and [SPECIFICATION_STYLE_GUIDE.md](SPECIFICATION_STYLE_GUIDE.md) before proposing a change.
