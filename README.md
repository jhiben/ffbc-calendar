# FFBC Calendar

A .NET 10 Razor Pages web application displaying FFBC-licensed mountain biking events in Belgium. Features a calendar view, event list, interactive map, event detail pages with activity cards, and an RSS feed.

## Getting Started

```bash
dotnet restore
dotnet build
dotnet run --project src/WebApp/WebApp.csproj
```

The app will be available at `http://localhost:5090`.

### Running Tests

```bash
dotnet test
```

## Deployment

The app deploys automatically to Azure App Service via GitHub Actions on every push to `main`.

### Azure Setup (one-time)

1. **Create an App Service** in the Azure Portal:
   - **Runtime stack:** .NET 10
   - **OS:** Linux
   - **Plan:** Free (F1)

2. **Download the Publish Profile:**
   - In the Azure Portal, go to your App Service → **Overview** → **Download publish profile**

3. **Add the GitHub Secret:**
   - Go to your GitHub repo → **Settings** → **Secrets and variables** → **Actions**
   - Create a secret named `AZURE_WEBAPP_PUBLISH_PROFILE` and paste the full contents of the downloaded publish profile XML

4. **Update the workflow** (if your App Service name differs):
   - Edit `.github/workflows/deploy.yml` and set `AZURE_WEBAPP_NAME` to your App Service name

### How It Works

- **Trigger:** Every push to `main`
- **Build & Test:** Restores, builds, and runs all tests. Deployment is skipped if any step fails.
- **Deploy:** Publishes the app and deploys to Azure App Service using the publish profile.

The app will be accessible at `https://<your-app-name>.azurewebsites.net`.
