### Requirement: Display events on an interactive map

The system SHALL display events as markers on an interactive map using Leaflet.js with OpenStreetMap tiles. The map SHALL be centered on Belgium and zoomed to fit all visible markers.

#### Scenario: Events with location data displayed as markers

- **WHEN** the user navigates to the Map page and events have valid postal code or town data
- **THEN** each event with a resolved location SHALL appear as a marker on the map

#### Scenario: Map centered on Belgium with appropriate zoom

- **WHEN** the Map page loads with one or more markers
- **THEN** the map SHALL auto-fit its bounds to show all markers

#### Scenario: No events available

- **WHEN** the user navigates to the Map page and no events exist
- **THEN** the map SHALL display centered on Belgium at a default zoom level with a message indicating no events are available

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

### Requirement: Navigation link to map view

The shared navigation bar SHALL include a link to the Map page.

#### Scenario: Map link in navigation

- **WHEN** the user views any page in the application
- **THEN** the navigation bar SHALL contain a "Map" link that navigates to the Map page

### Requirement: Event model includes town and postal code

The `Event` model SHALL include `Town`, `PostalCode`, and `Country` as optional properties so that location data is structured and available to all views.

#### Scenario: Event with full location data

- **WHEN** an event is parsed from the remote source with place text containing a Belgian postal code and town name
- **THEN** the event SHALL have `PostalCode` and `Town` populated as separate fields and `Country` set to `"Belgium"`

#### Scenario: Event with Belgian-prefixed postal code

- **WHEN** an event's place text contains a `B-` prefix followed by a 4-digit postal code and town name (e.g., `"B-1234 Town"`)
- **THEN** `PostalCode` SHALL be `"1234"`, `Town` SHALL be `"Town"`, and `Country` SHALL be `"Belgium"`

#### Scenario: Event with French-prefixed postal code

- **WHEN** an event's place text contains an `F-` prefix followed by a 5-digit postal code and town name (e.g., `"F-12345 Town"`)
- **THEN** `PostalCode` SHALL be `"12345"`, `Town` SHALL be `"Town"`, and `Country` SHALL be `"France"`

#### Scenario: Event with unparseable location

- **WHEN** an event's place text does not match any expected postal code format
- **THEN** `PostalCode` SHALL be null, `Country` SHALL be null, and `Town` SHALL contain the full place text

#### Scenario: Event with no location data

- **WHEN** an event has no place text from the source
- **THEN** `Town`, `PostalCode`, and `Country` SHALL all be null

### Requirement: Geocode events by postal code

The system SHALL resolve event locations to geographic coordinates using the postal code and/or town name via the OpenStreetMap Nominatim API, passing the correct country derived from the postal code format.

#### Scenario: Successful geocoding of Belgian postal code

- **WHEN** an event has a valid Belgian postal code and `Country` is `"Belgium"`
- **THEN** the system SHALL query Nominatim with `country=Belgium` and resolve it to latitude and longitude coordinates

#### Scenario: Successful geocoding of French postal code

- **WHEN** an event has a valid French postal code and `Country` is `"France"`
- **THEN** the system SHALL query Nominatim with `country=France` and resolve it to latitude and longitude coordinates

#### Scenario: Geocoding with unknown country

- **WHEN** an event has a postal code but `Country` is null
- **THEN** the system SHALL default to querying Nominatim with `country=Belgium`

#### Scenario: Geocoding results are cached

- **WHEN** events are fetched and geocoded
- **THEN** the geocoded coordinates SHALL be cached alongside the events so that repeated page loads do not re-invoke the geocoding API

#### Scenario: Geocoding failure

- **WHEN** the Nominatim API fails or returns no results for a postal code
- **THEN** the event SHALL be treated as having no location (not shown on map, shown in the list below)
