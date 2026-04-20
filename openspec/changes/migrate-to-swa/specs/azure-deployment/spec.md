## MODIFIED Requirements

### Requirement: CI/CD workflow triggers on push to main
The repository SHALL contain a GitHub Actions workflow that triggers automatically on every push to the `main` branch.

#### Scenario: Push to main triggers the workflow
- **WHEN** a commit is pushed to the `main` branch
- **THEN** the GitHub Actions workflow SHALL start automatically

#### Scenario: Push to other branches does not trigger deployment
- **WHEN** a commit is pushed to a branch other than `main`
- **THEN** the deployment workflow SHALL NOT run

### Requirement: Workflow builds and tests before deploying
The workflow SHALL build both the frontend and backend applications and run all tests, and SHALL only proceed to deployment if all steps succeed.

#### Scenario: Successful build and tests proceed to deploy
- **WHEN** `npm run build` succeeds for the frontend, `dotnet build` succeeds for the API, and all tests pass
- **THEN** the workflow SHALL proceed to the deploy step

#### Scenario: Frontend build failure prevents deployment
- **WHEN** `npm run build` fails
- **THEN** the workflow SHALL fail and SHALL NOT deploy

#### Scenario: Backend build failure prevents deployment
- **WHEN** `dotnet build` fails for the Functions project
- **THEN** the workflow SHALL fail and SHALL NOT deploy

#### Scenario: Test failure prevents deployment
- **WHEN** any test (frontend or backend) reports one or more failures
- **THEN** the workflow SHALL fail and SHALL NOT deploy

### Requirement: Deployment targets Azure Static Web Apps
The workflow SHALL deploy the built application to Azure Static Web Apps using the `Azure/static-web-apps-deploy` GitHub Action with a deployment token.

#### Scenario: Successful deployment to SWA
- **WHEN** build and tests pass
- **THEN** the workflow SHALL deploy the frontend build output and Functions publish output to Azure Static Web Apps using the `AZURE_STATIC_WEB_APPS_API_TOKEN` secret

#### Scenario: App is accessible after deployment
- **WHEN** deployment completes successfully
- **THEN** the application SHALL be accessible at the SWA's default `*.azurestaticapps.net` URL

### Requirement: Workflow configuration uses repository secrets
The workflow SHALL reference the Azure SWA deployment token from a GitHub repository secret, not from hardcoded values.

#### Scenario: Deployment token is read from secret
- **WHEN** the deploy step runs
- **THEN** it SHALL use the value of the `AZURE_STATIC_WEB_APPS_API_TOKEN` repository secret for authentication

## REMOVED Requirements

### Requirement: Deployment targets Azure App Service Free tier
**Reason**: The deployment target changes from Azure App Service to Azure Static Web Apps.
**Migration**: Replace `AZURE_WEBAPP_PUBLISH_PROFILE` secret with `AZURE_STATIC_WEB_APPS_API_TOKEN`. Update the workflow to use `Azure/static-web-apps-deploy` action instead of `azure/webapps-deploy`.
