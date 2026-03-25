## MODIFIED Requirements

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
