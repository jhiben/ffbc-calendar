## 1. GitHub Actions Workflow

- [x] 1.1 Create `.github/workflows/deploy.yml` with `on: push: branches: [main]` trigger
- [x] 1.2 Add `env:` block at workflow top with `AZURE_WEBAPP_NAME` variable and `DOTNET_VERSION: '10.0.x'`
- [x] 1.3 Add `build-and-test` job: checkout, setup .NET 10, `dotnet restore`, `dotnet build`, `dotnet test`
- [x] 1.4 Add `deploy` job (depends on `build-and-test`): checkout, setup .NET, `dotnet publish`, deploy using `azure/webapps-deploy@v3` with publish profile secret

## 2. Documentation

- [x] 2.1 Add a `## Deployment` section to `README.md` documenting: Azure App Service setup, publish profile secret configuration, and workflow trigger behavior

## 3. Verification

- [x] 3.1 Validate the workflow YAML syntax is correct
- [x] 3.2 Confirm `dotnet build` and `dotnet test` pass locally
