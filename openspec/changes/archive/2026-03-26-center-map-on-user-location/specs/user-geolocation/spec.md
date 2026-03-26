## ADDED Requirements

### Requirement: Request user geolocation on map load

The system SHALL request the user's current geographic position using the browser's Geolocation API when the map page loads.

#### Scenario: Geolocation permission granted

- **WHEN** the user navigates to the Map page and grants geolocation permission
- **THEN** the system SHALL obtain the user's current latitude and longitude coordinates

#### Scenario: Geolocation permission denied

- **WHEN** the user navigates to the Map page and denies geolocation permission
- **THEN** the system SHALL not display an error and SHALL proceed with the default map centering behavior

#### Scenario: Geolocation unavailable

- **WHEN** the user navigates to the Map page and geolocation is not supported by the browser
- **THEN** the system SHALL proceed with the default map centering behavior without displaying an error

#### Scenario: Geolocation timeout

- **WHEN** the user navigates to the Map page and geolocation takes longer than 5 seconds
- **THEN** the system SHALL abandon the geolocation request and proceed with the default map centering behavior

### Requirement: User can re-center map on their location

The system SHALL provide a control allowing the user to re-center the map on their current location after panning or zooming.

#### Scenario: Re-center button displayed

- **WHEN** the map page is rendered
- **THEN** a location button SHALL be displayed in the bottom-right corner of the map

#### Scenario: Re-center on button click

- **WHEN** the user clicks the location button and geolocation permission is granted
- **THEN** the map SHALL center on the user's current position at the default nearby events zoom level

#### Scenario: Re-center with denied permission

- **WHEN** the user clicks the location button and geolocation permission is denied
- **THEN** the system SHALL not change the map view and SHALL not display an error message
