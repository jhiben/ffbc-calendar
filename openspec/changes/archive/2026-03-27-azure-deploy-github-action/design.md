## Context

The FFBC Calendar is a .NET 10 Razor Pages app with no CI/CD pipeline. Code is pushed to GitHub but deployment is manual. The app has no database (in-memory event cache from an external API), making it stateless and ideal for simple cloud hosting. The goal is zero-cost continuous deployment to Azure App Service Free tier.

## Goals / Non-Goals

**Goals:**
- Automated build → test → deploy on every push to `main`
- Zero-cost hosting on Azure App Service F1 (Free) tier, Linux
- Simple setup: publish profile secret, no Azure AD app registration
- Fast feedback: fail the workflow if tests fail, before deploying

**Non-Goals:**
- Infrastructure provisioning via IaC (manual Azure Portal/CLI setup)
- Multi-environment (staging/production) pipeline
- Docker containerization
- Custom domains or TLS certificates

## Decisions

### 1. GitHub Actions over Azure DevOps Pipelines
**Choice:** GitHub Actions  
**Rationale:** The repo is on GitHub; Actions is native, free for public repos, and requires no external service.  
**Alternative:** Azure DevOps Pipelines — rejected for being out-of-ecosystem for a GitHub-hosted repo.

### 2. Publish Profile authentication over Service Principal (OIDC)
**Choice:** Azure Publish Profile stored as `AZURE_WEBAPP_PUBLISH_PROFILE` GitHub secret  
**Rationale:** Simplest setup — download the publish profile XML from Azure Portal, paste into GitHub Secrets. No Azure AD app registration, no federated credentials, no tenant/subscription IDs to manage.  
**Alternative:** OIDC-based service principal — more secure at scale but overkill for a single free-tier app.

### 3. Linux App Service over Windows
**Choice:** Linux App Service Plan (F1 Free)  
**Rationale:** Faster cold starts for .NET apps, smaller resource footprint, aligns with modern .NET deployment best practices.  
**Alternative:** Windows App Service — no advantage for this workload.

### 4. `dotnet publish` over Docker
**Choice:** Direct `dotnet publish` with the `azure/webapps-deploy` action  
**Rationale:** No Dockerfile to maintain, simpler workflow, and App Service natively supports .NET 10 runtime on Linux.  
**Alternative:** Docker build + push to ACR — unnecessary complexity.

### 5. Single workflow file with build-test-deploy stages
**Choice:** One `.github/workflows/deploy.yml` with sequential jobs: build → test → deploy  
**Rationale:** Keeps the pipeline simple and readable. Tests gate deployment — if tests fail, no deploy happens.  
**Alternative:** Separate CI and CD workflows — over-engineered for a single-target deployment.

## Risks / Trade-offs

- **[Free tier limitations]** → F1 tier has 60 min/day CPU, 1 GB RAM, no always-on, no custom domain SSL. Acceptable for a low-traffic event calendar.
- **[Cold starts]** → Free tier apps shut down after idle. First request after idle may take 5-10 seconds. Acceptable for this use case.
- **[Publish profile rotation]** → If the App Service is redeployed or reset, the publish profile changes and the GitHub secret must be updated manually. Low risk given single-maintainer project.
- **[.NET 10 on App Service]** → .NET 10 may need early-access runtime. If not available, the workflow can use a self-contained publish to bundle the runtime.

## Migration Plan

1. Create Azure App Service (F1, Linux, .NET 10) via Portal or CLI
2. Download the publish profile from Azure Portal
3. Add `AZURE_WEBAPP_PUBLISH_PROFILE` secret to GitHub repo settings
4. Merge the workflow file to `main` — first deployment triggers automatically
5. **Rollback:** Delete the workflow file or disable the GitHub Action; the App Service remains unchanged.
