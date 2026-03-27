## 1. Activity Card CSS

- [x] 1.1 Add `.activity-card` styles to `site.css`: flexbox card layout with border, radius, shadow, padding
- [x] 1.2 Add `.activity-metric` styles: emoji + label + value layout, pill/badge appearance
- [x] 1.3 Add `.difficulty-easy`, `.difficulty-moderate`, `.difficulty-hard`, `.difficulty-expert`, `.difficulty-unknown` color classes (green/yellow/orange/red/gray)
- [x] 1.4 Add `.activity-type-badge` style for the activity type label (e.g. VTT, GRAVEL, MARCHE)

## 2. Activity Cards in EventDetail.cshtml

- [x] 2.1 Replace the activities `<table>` with a card-per-activity layout using `.activity-card` divs
- [x] 2.2 Add emoji-labeled metric badges inside each card: 📏 Distance, ⛰️ D+, 💪 Difficulty, 🕐 Time, 🥤 Ravito, 💰 Price
- [x] 2.3 Add Razor helper to map difficulty values to CSS class names (easy/moderate/hard/expert/unknown)
- [x] 2.4 Style missing metric values ("—" or null) with muted appearance

## 3. Detail Section Emoji Labels

- [x] 3.1 Add emoji prefixes to General Information labels: 📅 Date, 🏘️ Town, 📮 Postal Code, 🌍 Country, 🗺️ Province, 🏆 Challenge, 📝 Notes
- [x] 3.2 Add emoji prefixes to Additional Details labels: 📍 Start Location, 🏠 Club, 🌐 Website, 🎫 Registration, 📝 Notes

## 4. Build & Verify

- [x] 4.1 Run `dotnet build` and confirm zero errors
- [x] 4.2 Run `dotnet test` and confirm all tests pass
- [x] 4.3 Visual review of event detail page in browser
