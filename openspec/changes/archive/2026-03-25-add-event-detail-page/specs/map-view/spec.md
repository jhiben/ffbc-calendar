## MODIFIED Requirements

### Requirement: Show event details on marker interaction
The system SHALL display event information in a popup when the user interacts with a map marker. The popup SHALL include a link to the Event Detail page.

#### Scenario: Marker popup shows event details
- **WHEN** the user clicks on a map marker
- **THEN** a popup SHALL display the event title, date, and town

#### Scenario: Marker popup links to detail page
- **WHEN** the user clicks on a map marker
- **THEN** the popup SHALL include a hyperlink to the Event Detail page for that event

### Requirement: Events without location are listed separately
Events that cannot be geocoded SHALL NOT be silently hidden. Event titles in the unmapped list SHALL link to the Event Detail page.

#### Scenario: Ungeocodable events shown below the map
- **WHEN** one or more events do not have a resolved location (no coordinates)
- **THEN** those events SHALL be listed in a section below the map with their title and date

#### Scenario: Unmapped event titles link to detail page
- **WHEN** the unmapped events table is rendered
- **THEN** each event title SHALL be a hyperlink that navigates to the Event Detail page for that event
