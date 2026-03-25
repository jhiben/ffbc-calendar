### Requirement: Event model includes challenge type
The `Event` model SHALL include an optional `Challenge` property that stores the challenge/category extracted from the FFBC calendar (e.g., "Brevet à Dénivelé", "Flèche Wallonne").

#### Scenario: Challenge field populated from source
- **WHEN** an event is parsed from the FFBC calendar HTML and the challenge column contains text
- **THEN** the event's `Challenge` property SHALL contain the normalized challenge text

#### Scenario: Challenge field null when source is empty
- **WHEN** an event is parsed from the FFBC calendar HTML and the challenge column is empty
- **THEN** the event's `Challenge` property SHALL be null

### Requirement: Event model includes province
The `Event` model SHALL include an optional `Province` property that stores the province extracted from the place text (e.g., "Namur", "Luxembourg").

#### Scenario: Province field populated from place text with province prefix
- **WHEN** an event's place text contains a province prefix (e.g., "Luxembourg - 6780 MESSANCY")
- **THEN** the event's `Province` property SHALL contain the province name (e.g., "Luxembourg")

#### Scenario: Province field null when place text has no province prefix
- **WHEN** an event's place text does not contain a province prefix (e.g., "5190 Spy")
- **THEN** the event's `Province` property SHALL be null

### Requirement: Backward-compatible Notes field
The `Notes` field SHALL continue to contain the combined place and challenge text for backward compatibility with existing views.

#### Scenario: Notes still contains combined text
- **WHEN** an event is parsed with both place and challenge text
- **THEN** the `Notes` property SHALL still contain the combined text (place | challenge) as before
