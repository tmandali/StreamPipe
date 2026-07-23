# SPSS-000B — Document Conventions

| Field | Value |
| --- | --- |
| Status | Draft |
| Category | Process |
| Depends on | SPSS-000A |
| Updates | None |
| Last updated | 2026-07-23 |

## Abstract

This document standardizes SPSS document structure, normative language, requirement identifiers, references, examples, and diagrams. It is designed to make the specification readable by people and automated tooling without hidden assumptions.

## Normative language

The uppercase terms **MUST**, **MUST NOT**, **REQUIRED**, **SHALL**, **SHALL NOT**, **SHOULD**, **SHOULD NOT**, **RECOMMENDED**, **NOT RECOMMENDED**, **MAY**, and **OPTIONAL** are interpreted as described in RFC 2119 and RFC 8174.

`REQ-DOC-001` — A normative requirement **MUST** use an uppercase RFC 2119 keyword.

`REQ-DOC-002` — Each normative requirement **MUST** define one independently testable behavior.

## Document metadata and sections

Every SPSS document **MUST** start with a title and a metadata table containing status, category, dependencies, updates, and last-updated date. It **MUST** include an Abstract, Scope, Normative Requirements, Compatibility Considerations, Security Considerations, Performance Considerations, and References.

## Requirement identifiers

Requirement identifiers have the form `REQ-<AREA>-<NNN>`, where `<AREA>` is uppercase ASCII and `<NNN>` is a zero-padded decimal number.

`REQ-DOC-003` — A requirement identifier **MUST** be unique across the repository.

`REQ-DOC-004` — A retired identifier **MUST NOT** be reused for a different behavior.

`REQ-DOC-005` — A semantic change **MUST** preserve an identifier only when the requirement’s intent remains unchanged.

## References and examples

Cross-references should include the SPSS identifier and section title. Examples are informative unless explicitly labelled **Normative Example**. Informative examples **MUST NOT** introduce behavior not present in normative prose.

## Diagrams

Mermaid is the preferred diagram format. Diagram source belongs in the repository and must be readable in a text diff. A diagram explains; it does not supersede normative prose.

## Compatibility considerations

Each proposal must state whether it is backward compatible, conditionally compatible through negotiation, or incompatible. A change that creates ambiguity between compliant implementations is incompatible.

## Security considerations

Each document must state which trust boundaries, attacker-controlled values, limits, and failure cases it creates or changes.

## Performance considerations

Each document must state its effects on buffering, copy count, allocation, flow control, and latency where applicable. It must distinguish a protocol guarantee from an implementation optimization.

## References

- RFC 2119
- RFC 8174
- [SPECIFICATION_STYLE_GUIDE.md](../../SPECIFICATION_STYLE_GUIDE.md)
