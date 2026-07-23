# StreamPipe Specification Style Guide

## Normative language

The key words **MUST**, **MUST NOT**, **REQUIRED**, **SHALL**, **SHALL NOT**, **SHOULD**, **SHOULD NOT**, **RECOMMENDED**, **NOT RECOMMENDED**, **MAY**, and **OPTIONAL** are interpreted as described by RFC 2119 and RFC 8174 when written in uppercase.

## Requirement identifiers

Each normative statement has exactly one identifier in the form `REQ-<AREA>-<NNN>`, for example `REQ-MEM-001`. IDs are stable: never reuse an ID for a different requirement. Retired requirements remain documented as retired.

## Required document sections

Every SPSS document MUST include: metadata, abstract, status, scope, terminology or references to it, normative requirements, compatibility considerations, security considerations, performance considerations, and references.

## Examples and diagrams

Examples are informative unless explicitly marked normative. Prefer Mermaid for diagrams and keep their source in version control. Diagrams MUST not add requirements absent from the prose.

## Writing rules

Write one testable behavior per normative statement. Define terms before use. State ownership, failure behavior, ordering, and limits where relevant. Avoid vague terms such as “fast”, “small”, or “normally” unless a measurable condition is specified.
