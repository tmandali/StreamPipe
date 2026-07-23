# StreamPipe Agent Instructions

## Authority order

1. Accepted SPSS documents in `spec/spss/`
2. Accepted ADRs in `spec/adr/`
3. This file and repository governance documents
4. Implementation, examples, and issue discussion

Agents MUST treat the higher-ranked source as authoritative when sources conflict.

## Required behavior

- Read the relevant SPSS documents before changing code, examples, or specifications.
- Do not invent protocol behavior, defaults, wire fields, or compatibility rules.
- Preserve language and transport independence.
- Every normative requirement MUST have a stable `REQ-<AREA>-<NNN>` identifier.
- If a required behavior is unspecified or contradictory, stop and report the gap; do not guess.
- A change to observable protocol behavior MUST update the applicable SPSS and traceability information in the same change.
- Keep diagrams in Mermaid or another text-based, diffable source format.

## Non-negotiable principles

- Entire payloads MUST NOT be buffered merely for convenience.
- Implementations MUST preserve bounded memory with respect to stream length.
- Backpressure MUST be propagated rather than hidden by unbounded queues.
- SDKs MUST NOT redefine the protocol.
