## ADDED Requirements

### Requirement: Activity cards with emoji-labeled metrics
Each activity SHALL be displayed as a card with emoji-labeled metrics (📏 Distance, ⛰️ D+, 💪 Difficulty, 🕐 Time, 🥤 Ravito, 💰 Price) instead of a plain table row.

#### Scenario: Activity card renders all available metrics
- **WHEN** an activity has values for distance, elevation, difficulty, time, ravito, and price
- **THEN** the card SHALL display each metric as an emoji-labeled badge or pill

#### Scenario: Activity card hides missing metrics
- **WHEN** an activity has null or "—" for a metric
- **THEN** that metric SHALL still be displayed with a muted "—" indicator

---

### Requirement: Color-coded difficulty badges
The difficulty metric SHALL render as a color-coded badge: green for easy, yellow for moderate, orange for hard, red for expert, gray for unknown.

#### Scenario: Known difficulty values are color-coded
- **WHEN** an activity has a recognizable difficulty value (numeric 1-4 or text like "facile", "moyen", "difficile", "expert")
- **THEN** the difficulty badge SHALL use the corresponding color class

#### Scenario: Unknown difficulty values default to gray
- **WHEN** an activity has an unrecognized difficulty value or "N.A"
- **THEN** the difficulty badge SHALL use a neutral gray style

---

### Requirement: Distance and elevation visual emphasis
Distance and elevation values SHALL be displayed with larger font weight and an accompanying emoji to aid scanning.

#### Scenario: Distance shows with route emoji
- **WHEN** an activity card renders the distance metric
- **THEN** it SHALL display with a 📏 emoji prefix and semibold font weight

#### Scenario: Elevation shows with mountain emoji
- **WHEN** an activity card renders the elevation metric
- **THEN** it SHALL display with a ⛰️ emoji prefix and semibold font weight

---

### Requirement: Price visual indicator
Price values SHALL visually distinguish free events from paid events.

#### Scenario: Price shown with money emoji
- **WHEN** an activity has a price value
- **THEN** it SHALL render with a 💰 emoji prefix

---

### Requirement: Ravito visual indicator
Ravito (refreshment stop) values SHALL display with a drink emoji for quick recognition.

#### Scenario: Ravito shown with drink emoji
- **WHEN** an activity has a ravito value
- **THEN** it SHALL render with a 🥤 emoji prefix

---

### Requirement: Detail section labels with emoji prefixes
The general information and additional details sections SHALL use emoji prefixes on labels for visual scannability.

#### Scenario: General info labels have emoji prefixes
- **WHEN** the general information card is rendered
- **THEN** labels for Date (📅), Town (🏘️), Postal Code (📮), Country (🌍), Province (🗺️), Challenge (🏆), and Notes (📝) SHALL include the corresponding emoji prefix

#### Scenario: Additional details labels have emoji prefixes
- **WHEN** the additional details card is rendered
- **THEN** labels for Start Location (📍), Club (🏠), Website (🌐), Registration (🎫), and Notes (📝) SHALL include the corresponding emoji prefix
