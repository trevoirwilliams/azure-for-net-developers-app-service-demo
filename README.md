# Product Catalogue API

Starting application for Section 5, Azure App Service for ASP.NET Core, in Microsoft Azure for .NET Developers.

The API is supplied ready to run. Five sample products live in memory. This section uses the application to practise deployment, configuration, logging and release verification.

## Get the starting code

```text
git clone --branch start https://github.com/trevoirwilliams/azure-for-net-developers-app-service-demo.git
cd azure-for-net-developers-app-service-demo
```

## Run locally

Use a stable .NET 10 SDK in the 10.0.4xx feature band. The supplied global.json selects 10.0.400 or a later patch in that band.

In Visual Studio 2026, open ProductCatalog.Api.csproj and run the ProductCatalog.Api profile. In VS Code or another terminal, run these commands from the repository root.

```text
dotnet --version
dotnet run
```

The launch profile uses http://localhost:5080. Open the supplied .http file in your IDE or request the following routes. Use curl.exe on Windows.

| Route | Expected result |
| --- | --- |
| GET /api/products | HTTP 200, storeName Azure Demo Store, mode standard and five products |
| GET /api/info | HTTP 200, application product-catalog-api and releaseVersion 1.0.0 |
| GET /health | HTTP 200 and status healthy |

```text
curl -i http://localhost:5080/api/products
curl -i http://localhost:5080/api/info
curl -i http://localhost:5080/health
```

The root URL has no endpoint. Use the routes listed above.

## Settings used during the lessons

| Application key | App Service setting name | Default | Purpose |
| --- | --- | --- | --- |
| Catalog:StoreName | Catalog__StoreName | Azure Demo Store | Change the visible store name without a code change |
| Catalog:Mode | Catalog__Mode | standard | Exercise configuration and a reversible fault |

Both standard and maintenance are accepted mode labels and return the product list with HTTP 200. In this teaching sample, maintenance is a visible label.

An unsupported mode such as maintenence makes /api/products return HTTP 503 and records CATALOG_MODE_INVALID at Warning level. The log omits the supplied value. The other two endpoints keep responding, helping you compare the failing feature with the running process.

For a local configuration exercise in PowerShell, stop the app, set the override and start again.

```powershell
$env:Catalog__StoreName = 'My Training Store'
$env:Catalog__Mode = 'maintenance'
dotnet run
```

Stop the app, change Catalog__Mode to maintenence, and run it again. Request /api/products and check the warning in the application terminal. Restore maintenance, restart and verify HTTP 200. Remove the overrides when finished.

```powershell
Remove-Item Env:Catalog__StoreName
Remove-Item Env:Catalog__Mode
```

For Bash, use export Catalog__Mode=maintenance before starting the app and unset Catalog__Mode to clear the override.

## Publishing starting point

The App Service lessons create the Azure destination and publish this project. Use the default hostname returned for your actual web app, followed by /api/products, /api/info or /health.

The releaseVersion constant in Program.cs identifies the application release. Change it for the later release and slot demonstrations. The value comes from the deployed code.

For a framework-dependent Release publish, run the following from the repository root.

```text
dotnet publish ProductCatalog.Api.csproj -c Release --self-contained false -o ./publish
```

Package the contents of publish at the archive root for ZIP deployment. The publish directory and credentials are excluded from Git.

The local HTTP address is for development. Verify the deployed API over HTTPS. The public endpoints contain synthetic demonstration data. /health is a process check and does not test the product route or external dependencies.

## Checkpoints

| Branch | Teaching state |
| --- | --- |
| start | Runnable API with sample products, configuration, warning logging and release 1.0.0 |

Additional lesson checkpoints will be added as the section is developed.

## Microsoft Learn references

Read on 17 November 2026. ASP.NET Core references use the 10.0 view.

- [Minimal API route handlers](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/route-handlers?view=aspnetcore-10.0) covers endpoint registration and handler parameter binding.
- [Minimal API responses](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/responses?view=aspnetcore-10.0) covers JSON success and problem responses.
- [Configuration in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-10.0) covers IConfiguration, defaults and environment overrides.
- [Logging in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-10.0) covers ILogger and warning messages.
- [global.json](https://learn.microsoft.com/en-us/dotnet/core/tools/global-json) covers the SDK feature-band and patch policy.
- [ASP.NET Core on App Service](https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/azure-apps/?view=aspnetcore-10.0) covers the hosting context.

## Validation

Verified locally on 17 November 2026 with .NET SDK 10.0.400. Release publish succeeded. HTTP checks returned the expected products, app information and health response. Environment overrides changed the store name and mode. The invalid mode returned 503 and recorded the warning. Restoring maintenance recovered the product route with 200. Azure deployment and live log streaming remain rehearsal checks.
