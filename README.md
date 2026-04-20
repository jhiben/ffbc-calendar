# FFBC Calendar

A web application displaying FFBC-licensed mountain biking events in Belgium. Features a calendar view, event list, interactive map, event detail pages with activity cards, and an RSS feed.

**Architecture:** SvelteKit frontend (static SPA) + .NET 10 Azure Functions API, hosted on Azure Static Web Apps.

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js 22+](https://nodejs.org/)
- [Azure Functions Core Tools](https://learn.microsoft.com/azure/azure-functions/functions-run-local) (optional, for local API)

### Local Development

```bash
# Install frontend dependencies
cd src/WebApp
npm install

# Start the SvelteKit dev server (proxies /api to localhost:7071)
npm run dev
```

In a separate terminal, start the Azure Functions API:

```bash
cd src/Api
func start
```

The frontend will be available at `http://localhost:5173` with API calls proxied to the Functions host.

### Running Tests

```bash
# .NET API tests
dotnet test

# Build frontend
cd src/WebApp && npm run build
```

## Project Structure

```
├── src/
│   ├── WebApp/          SvelteKit frontend (static SPA)
│   │   ├── src/
│   │   │   ├── routes/  File-based routing (/, /list, /calendar, /map, /event)
│   │   │   └── lib/     API client, types, stores
│   │   └── static/      favicon
│   └── Api/             .NET Azure Functions (isolated worker)
│       ├── Functions/   HTTP trigger endpoints
│       ├── Services/    Event fetching, geocoding, details parsing
│       └── Models/      Shared data models
├── tests/
│   └── Api.Tests/       xUnit tests for API services and endpoints
├── staticwebapp.config.json  SWA routing configuration
└── .github/workflows/        CI/CD pipeline
```

## Deployment

The app deploys automatically to Azure Static Web Apps via GitHub Actions on every push to `main`.

### Azure Setup (one-time)

1. **Create a Static Web App** in the Azure Portal
2. **Get the deployment token** from the Static Web App → **Manage deployment token**
3. **Add the GitHub Secret:**
   - Go to your GitHub repo → **Settings** → **Secrets and variables** → **Actions**
   - Create a secret named `AZURE_STATIC_WEB_APPS_API_TOKEN` and paste the deployment token

### How It Works

- **Trigger:** Every push to `main` or PR against `main`
- **Build & Test:** Builds frontend (npm), builds API (.NET), runs all tests
- **Deploy:** Uploads static frontend + API to Azure Static Web Apps
- **PR previews:** Staging environments created for pull requests
