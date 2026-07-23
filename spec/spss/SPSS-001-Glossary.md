# SPSS-001 — Glossary

| Field | Value |
| --- | --- |
| Status | Draft |
| Category | Standards Track |
| Depends on | SPSS-000A, SPSS-000B |
| Updates | None |
| Last updated | 2026-07-23 |

## Abstract

This document defines terms used by StreamPipe specifications. Unless a later SPSS document explicitly overrides a definition, these meanings apply throughout the repository.

## Scope

This glossary defines terminology only. It does not impose a wire format or public API.

## Terms

| Term | Definition |
| --- | --- |
| Application | User-owned code that consumes or produces data through an SDK. |
| Batch | A bounded group of logical records transferred or processed together; not necessarily a protocol frame. |
| Backpressure | A mechanism by which a slower consumer limits upstream production to bound resource use. |
| Client | The endpoint that initiates a request or connection according to an applicable transport profile. |
| Data source | A producer of logical records, such as a database reader or async iterator. |
| Data sink | A consumer of logical records, such as a bulk loader or application callback. |
| Frame | A discrete protocol unit with defined semantics and encoded boundaries. |
| Format | The representation of logical data in a payload, such as Arrow IPC. |
| Payload | Bytes carried by a frame after protocol-defined framing fields. |
| Protocol | The language-independent rules for a StreamPipe session and frames. |
| SDK | A language-specific implementation of StreamPipe protocol and adapter requirements. |
| Server | The endpoint that accepts a request or connection according to an applicable transport profile. |
| Session | The protocol-governed relationship between endpoints from negotiation until termination. |
| Stream | An ordered logical flow of frames within a session. |
| Transport | A mechanism that carries bytes between endpoints, such as HTTP, TCP, named pipes, or gRPC. |
| Transport adapter | SDK code that maps a transport’s I/O and lifecycle to StreamPipe session semantics. |
| Window | A bounded amount of in-flight data or credit used for flow control. |

## Normative requirements

`REQ-GLOSSARY-001` — Later SPSS documents **MUST** use these terms consistently or explicitly define a replacement term.

`REQ-GLOSSARY-002` — A transport adapter **MUST NOT** be described as the protocol itself.

## Compatibility considerations

Adding a term is compatible. Changing a defined term requires updating each affected SPSS document or publishing an explicit replacement rule.

## Security considerations

The terms client and server describe session roles and do not imply trust. Either role may receive attacker-controlled data depending on deployment.

## Performance considerations

The term bounded means bounded by explicit implementation or protocol limits; it does not imply a universal numeric limit.

## References

- [SPSS-000](SPSS-000-Overview.md)
- [SPSS-000B](SPSS-000B-Document-Conventions.md)
