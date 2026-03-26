## ADDED Requirements

### Requirement: Display enriched event details

The Event Detail page SHALL display additional information fetched from the popup API when available.

#### Scenario: Enriched details displayed when available

- **WHEN** the Event Detail page loads for an event with a valid EventId and enriched details are successfully fetched
- **THEN** the page SHALL display the additional details alongside the basic event information

#### Scenario: Page works without enriched details

- **WHEN** the Event Detail page loads for an event without an EventId or when detail fetching fails
- **THEN** the page SHALL display the basic event information without enriched details

#### Scenario: Enriched details section hidden when empty

- **WHEN** enriched details are not available for an event
- **THEN** the page SHALL NOT display an empty enriched details section
