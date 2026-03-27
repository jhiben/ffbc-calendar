## ADDED Requirements

### Requirement: CI/CD workflow triggers on push to main
The repository SHALL contain a GitHub Actions workflow that triggers automatically on every push to the `main` branch.

#### Scenario: Push to main triggers the workflow
- **WHEN** a commit is pushed to the `main` branch
- **THEN** the GitHub Actions workflow SHALL start automatically

#### Scenario: Push to other branches does not trigger deployment
- **WHEN** a commit is pushed to a branch other than `main`
- **THEN** the deployment workflow SHALL NOT run

---

### Requirement: Workflow builds and tests before deploying
The workflow SHALL build the application and run all tests, and SHALL only proceed to deployment if both steps succeed.

#### Scenario: Successful build and tests proceed to deploy
- **WHEN** `dotnet build` succeeds and `dotnet test` reports zero failures
- **THEN** the workflow SHALL proceed to the deploy step

#### Scenario: Build failure prevents deployment
- **WHEN** `dotnet build` fails
- **THEN** the workflow SHALL fail and SHALL NOT deploy

#### Scenario: Test failure prevents deployment
- **WHEN** `dotnet test` reports one or more test failures
- **THEN** the workflow SHALL fail and SHALL NOT deploy

---

### Requirement: Deployment targets Azure App Service Free tier
The workflow SHALL deploy the built application to an Azure App Service (F1 Free tier, Linux) using the `azure/webapps-deploy` GitHub Action with publish profile authentication.

#### Scenario: Successful deployment to App Service
- **WHEN** build and tests pass
- **THEN** the workflow SHALL publish the app with `dotnet publish` and deploy the output to the configured Azure App Service using the `AZURE_WEBAPP_PUBLISH_PROFILE` secret

#### Scenario: App is accessible after deployment
- **WHEN** deployment completes successfully
- **THEN** the application SHALL be accessible at the App Service's default `*.azurewebsites.net` URL

---

### Requirement: Workflow configuration uses repository secrets
The workflow SHALL reference the Azure publish profile from a GitHub repository secret, not from hardcoded values.

#### Scenario: Publish profile is read from secret
- **WHEN** the deploy step runs
- **THEN** it SHALL use the value of the `AZURE_WEBAPP_PUBLISH_PROFILE` repository secret for authentication
- **AND** the app name SHALL be configured via the `AZURE_WEBAPP_NAME` environment variable at the top of the workflow
