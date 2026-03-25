## MODIFIED Requirements

### Requirement: List item shows key fields
Each event item in the list SHALL display the event date, title, and notes. The event title SHALL be a link to the Event Detail page.

#### Scenario: Event details visible
- **WHEN** the List View page is rendered with at least one event
- **THEN** each item SHALL show the event's date, title, and notes (notes may be empty)

#### Scenario: Event title links to detail page
- **WHEN** the List View page is rendered with at least one event
- **THEN** each event's title SHALL be a hyperlink that navigates to the Event Detail page for that event
