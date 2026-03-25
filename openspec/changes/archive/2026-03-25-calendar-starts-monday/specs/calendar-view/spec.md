## MODIFIED Requirements

### Requirement: Display events in monthly calendar grid
The system SHALL display planned mountain biking events in a monthly calendar grid showing all days of the selected month, with weekly columns ordered Monday through Sunday.

#### Scenario: Default to current month
- **WHEN** the user navigates to the Calendar page with no query string parameters
- **THEN** the system SHALL render the calendar for the current year and month

#### Scenario: Display events on correct day cells
- **WHEN** the Calendar page is rendered for a given month
- **THEN** each day cell that has one or more events SHALL display the event titles for those events

#### Scenario: Empty day cells shown
- **WHEN** the Calendar page is rendered for a given month
- **THEN** days with no events SHALL still appear as empty cells in the grid

#### Scenario: Calendar week starts on Monday
- **WHEN** the Calendar page is rendered for any month
- **THEN** the calendar grid SHALL place Monday in the first weekly column and Sunday in the last weekly column
