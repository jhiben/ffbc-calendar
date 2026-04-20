## ADDED Requirements

### Requirement: Azure Static Web Apps resource configuration
The app SHALL be deployed as an Azure Static Web App with a managed .NET Azure Functions backend.

#### Scenario: SWA hosts static frontend
- **WHEN** the SWA is deployed
- **THEN** the static frontend files SHALL be served from the SWA global CDN

#### Scenario: SWA routes API requests to Functions
- **WHEN** a request is made to `/api/*`
- **THEN** the SWA SHALL route it to the managed Azure Functions backend

#### Scenario: SPA fallback routing
- **WHEN** a request is made to a client-side route (e.g., `/calendar`, `/map`)
- **THEN** the SWA SHALL serve `index.html` to allow client-side routing

### Requirement: SWA configuration file
The repository SHALL contain a `staticwebapp.config.json` at the root that configures routing, fallback, and API integration.

#### Scenario: Navigation fallback configured
- **WHEN** the SWA receives a request for a path without a matching static file
- **THEN** it SHALL serve the `index.html` fallback for client-side routing

#### Scenario: RSS feed route rewrite
- **WHEN** a request is made to `/feed.xml`
- **THEN** the SWA SHALL rewrite it to `/api/feed.xml` so the Functions backend generates the RSS feed

#### Scenario: Cache headers for static assets
- **WHEN** static assets (CSS, JS, images) are served
- **THEN** the response SHALL include appropriate cache-control headers for performance

### Requirement: GitHub Actions workflow for SWA deployment
The repository SHALL contain a GitHub Actions workflow that builds both the frontend and backend and deploys to Azure Static Web Apps.

#### Scenario: Push to main triggers deployment
- **WHEN** a commit is pushed to the `main` branch
- **THEN** the GitHub Actions workflow SHALL trigger and deploy to SWA

#### Scenario: Workflow builds frontend
- **WHEN** the workflow runs
- **THEN** it SHALL install Node.js dependencies and run `npm run build` in the frontend project

#### Scenario: Workflow builds backend
- **WHEN** the workflow runs
- **THEN** it SHALL run `dotnet build` and `dotnet publish` on the Functions project

#### Scenario: Workflow runs tests before deploying
- **WHEN** the workflow runs
- **THEN** it SHALL execute both .NET and frontend tests and SHALL NOT deploy if any test fails

#### Scenario: Deployment uses SWA CLI or GitHub Action
- **WHEN** the deploy step runs
- **THEN** it SHALL use the `Azure/static-web-apps-deploy` GitHub Action with the SWA deployment token from repository secrets
