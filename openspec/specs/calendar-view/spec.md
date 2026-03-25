### Requirement: Display events in monthly calendar grid
The system SHALL display planned mountain biking events in a monthly calendar grid showing all days of the selected month.

#### Scenario: Default to current month
- **WHEN** the user navigates to the Calendar page with no query string parameters
- **THEN** the system SHALL render the calendar for the current year and month

#### Scenario: Display events on correct day cells
- **WHEN** the Calendar page is rendered for a given month
- **THEN** each day cell that has one or more events SHALL display the event titles for those events

#### Scenario: Empty day cells shown
- **WHEN** the Calendar page is rendered for a given month
- **THEN** days with no events SHALL still appear as empty cells in the grid

### Requirement: Navigate between months
The system SHALL allow users to navigate to the previous and next month from the Calendar page.

#### Scenario: Navigate to previous month
- **WHEN** the user activates the "Previous" navigation control
- **THEN** the system SHALL render the calendar for the month immediately before the currently displayed month

#### Scenario: Navigate to next month
- **WHEN** the user activates the "Next" navigation control
- **THEN** the system SHALL render the calendar for the month immediately after the currently displayed month

#### Scenario: Navigate to a specific month via URL
- **WHEN** the user navigates to the Calendar page with `?year=YYYY&month=MM` query parameters
- **THEN** the system SHALL render the calendar for the specified year and month

### Requirement: Navigation link to calendar view
The application navigation SHALL include a link to the Calendar page accessible from any page.

#### Scenario: Calendar link present in nav bar
- **WHEN** the user is on any page in the application
- **THEN** the shared navigation bar SHALL contain a "Calendar" link that navigates to the Calendar page
