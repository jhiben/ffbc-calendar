## ADDED Requirements

### Requirement: Display full event details on a dedicated page
The system SHALL provide a dedicated Event Detail page that displays all available information for a single event, identified by date and title.

#### Scenario: Detail page shows all event fields
- **WHEN** the user navigates to the Event Detail page with a valid date and title
- **THEN** the system SHALL display the event's title, date, town, postal code, country, province, challenge type, and notes

#### Scenario: Detail page renders a user-friendly layout
- **WHEN** the Event Detail page is rendered for an event
- **THEN** the system SHALL present the event information in a structured, readable layout with clear labels for each field

#### Scenario: Missing optional fields are handled gracefully
- **WHEN** an event has null or empty values for optional fields (town, postal code, province, challenge)
- **THEN** the detail page SHALL omit those fields from the display rather than showing blank labels

### Requirement: Event not found handling
The system SHALL display a meaningful message when the requested event cannot be found.

#### Scenario: Event not found by date and title
- **WHEN** the user navigates to the Event Detail page with a date and title that do not match any event
- **THEN** the system SHALL display a "not found" message and a link back to the event list

#### Scenario: Missing parameters
- **WHEN** the user navigates to the Event Detail page without providing both date and title parameters
- **THEN** the system SHALL redirect to the event list page

### Requirement: Event lookup via date and title
The `IEventStore` interface SHALL expose a method to retrieve a single event by its date and title.

#### Scenario: Lookup returns matching event
- **WHEN** `GetByDateAndTitle` is called with a date and title that match an existing event
- **THEN** the method SHALL return that event

#### Scenario: Lookup returns null for no match
- **WHEN** `GetByDateAndTitle` is called with a date and title that do not match any event
- **THEN** the method SHALL return null

#### Scenario: Lookup is case-insensitive on title
- **WHEN** `GetByDateAndTitle` is called with a title that differs only in casing from an existing event
- **THEN** the method SHALL return the matching event
