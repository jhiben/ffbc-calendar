## Why

The FFBC Calendar has no deployment pipeline. The app must be manually built and deployed, which is error-prone and blocks rapid iteration. Adding a GitHub Actions workflow that deploys to Azure App Service (Free F1 tier) on push to `main` gives the project continuous delivery at zero cost.

## What Changes

- Add a GitHub Actions workflow (`.github/workflows/deploy.yml`) that triggers on push to `main`, builds the .NET 10 app, runs tests, and deploys to Azure App Service using the `azure/webapps-deploy` action.
- Add a Publish Profile–based authentication approach (no Azure AD service principal needed — just a publish profile secret stored in GitHub).
- Target the Azure App Service Free (F1) tier on Linux with the .NET 10 runtime stack.

## Capabilities

### New Capabilities
- `azure-deployment`: GitHub Actions CI/CD pipeline that builds, tests, and deploys the app to Azure App Service Free tier on push to main.

### Modified Capabilities
_None — no existing spec requirements change._

## Impact

- **New file**: `.github/workflows/deploy.yml` — GitHub Actions workflow
- **Dependencies**: Requires an Azure App Service resource (Free F1, Linux, .NET 10) created manually or via Azure CLI. Requires the publish profile stored as a GitHub repository secret `AZURE_WEBAPP_PUBLISH_PROFILE`.
- **No code changes**: The application source code is unaffected; this is purely infrastructure/CI.

## Non-goals

- Infrastructure-as-Code (Bicep/ARM/Terraform) to provision the Azure resource — the App Service is created manually via the Azure Portal or CLI.
- Custom domain or SSL configuration.
- Staging slots or blue-green deployment (not available on Free tier).
- Docker containerization — the workflow uses `dotnet publish` directly.
