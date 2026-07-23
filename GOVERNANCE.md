# StreamPipe Governance

## Purpose

This document defines how the StreamPipe Standard Specification (SPSS) evolves.

## Decision principles

The protocol is the product. SDKs are conforming implementations, not alternative sources of protocol truth. Changes favor interoperability, clear conformance, bounded resource use, and backward compatibility.

## Lifecycle

An SPSS document progresses through `Draft`, `Review`, `Accepted`, `Implemented`, `Stable`, `Deprecated`, and `Obsolete`. Only Accepted, Implemented, Stable, Deprecated, or Obsolete documents may define normative behavior.

## Changes

Editorial changes may be merged when they do not alter meaning. A semantic change requires a proposal, explicit compatibility analysis, review, and updates to affected requirement IDs. Material architecture changes require an ADR before or alongside the SPSS update.

## Versioning

The protocol and every SDK publish independent versions. Protocol compatibility is governed by accepted SPSS documents, not by an SDK release number. Incompatible wire changes require a new protocol major version or a negotiated extension explicitly specified by SPSS.

## Conflict resolution

When documents conflict, the newer accepted document wins only when it explicitly updates or replaces the older document. Otherwise, the conflict MUST be resolved by an ADR and a corrective specification change before implementation proceeds.
