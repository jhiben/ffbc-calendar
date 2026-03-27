## Requirements

### Requirement: RSS 2.0 feed endpoint
The system SHALL serve a valid RSS 2.0 XML feed at the `/feed.xml` URL path containing upcoming events.

#### Scenario: Feed returns valid RSS 2.0 XML
- **WHEN** a client sends a GET request to `/feed.xml`
- **THEN** the response SHALL have status 200, content type `application/rss+xml`, and a body that is valid RSS 2.0 XML with `<rss version="2.0">` root element

#### Scenario: Feed contains channel metadata
- **WHEN** the feed is requested
- **THEN** the `<channel>` element SHALL include `<title>`, `<link>`, and `<description>` elements identifying the FFBC Calendar

#### Scenario: Feed contains event items
- **WHEN** there are upcoming events in the event store
- **THEN** each event SHALL be represented as an `<item>` element with `<title>`, `<link>`, `<description>`, and `<pubDate>` elements

#### Scenario: Feed is empty when no events exist
- **WHEN** there are no events in the event store
- **THEN** the feed SHALL return a valid RSS 2.0 document with an empty `<channel>` (no `<item>` elements)

---

### Requirement: Event item mapping
Each event SHALL map to an RSS item with meaningful content derived from the Event model fields.

#### Scenario: Item title is the event title
- **WHEN** an event is mapped to an RSS item
- **THEN** the `<title>` SHALL be the event's Title

#### Scenario: Item link points to event detail page
- **WHEN** an event is mapped to an RSS item
- **THEN** the `<link>` SHALL be an absolute URL to the event's detail page (`/EventDetail?date=YYYY-MM-DD&title=...`)

#### Scenario: Item description includes location
- **WHEN** an event has Town and Country values
- **THEN** the `<description>` SHALL include the town and country information

#### Scenario: Item pubDate is the event date
- **WHEN** an event is mapped to an RSS item
- **THEN** the `<pubDate>` SHALL be the event's Date in RFC 822 format

---

### Requirement: RSS auto-discovery
The site SHALL include an RSS auto-discovery `<link>` tag so feed readers can detect the feed automatically.

#### Scenario: Auto-discovery link in HTML head
- **WHEN** any page is rendered
- **THEN** the `<head>` SHALL contain `<link rel="alternate" type="application/rss+xml" title="FFBC Calendar Events" href="/feed.xml" />`

---

### Requirement: Visible feed link
The site SHALL provide a visible link to the RSS feed for manual discovery.

#### Scenario: Feed link in footer
- **WHEN** any page is rendered
- **THEN** the footer SHALL contain a visible link to `/feed.xml` with an RSS label or icon
