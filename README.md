# StreamPipe

StreamPipe is a specification-first initiative for a language- and transport-independent streaming data protocol and SDK ecosystem.

The project defines the StreamPipe Standard Specification (SPSS): a normative contract for bounded-memory streaming, backpressure, schema-aware data exchange, and independently implemented SDKs. This repository is the source of truth for the specification; it is not an SDK implementation.

## Repository layout

- `spec/spss/` — normative StreamPipe standards
- `spec/adr/` — architecture decision records
- `spec/examples/` — informative protocol and API examples
- `spec/diagrams/` — source-controlled diagrams
- `spec/templates/` — authoring templates
- `.github/` — issue and pull-request templates

## Status

The specification is in its foundation phase. No wire-format or SDK behavior is normative until its corresponding SPSS document is accepted.

## Contributing

Read [AGENTS.md](AGENTS.md), [GOVERNANCE.md](GOVERNANCE.md), [CONTRIBUTING.md](CONTRIBUTING.md), and [SPECIFICATION_STYLE_GUIDE.md](SPECIFICATION_STYLE_GUIDE.md) before proposing a change.
