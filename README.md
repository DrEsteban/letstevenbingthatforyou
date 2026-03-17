# LetMeBingThatForYou

[![Build and Publish](https://github.com/mateusrodrigues/letmebingthatforyou/actions/workflows/build-and-publish.yml/badge.svg)](https://github.com/mateusrodrigues/letmebingthatforyou/actions/workflows/build-and-publish.yml)

Despite what our well intentioned teachers told us in elementary school, there 
is such a thing as a stupid question. And stupid questions deserve snarky 
answers.

[LetMeBingThatForYou](http://letmebingthatforyou.com/) is for all those people 
that love to bother you with their question rather than simply Bing it 
themselves. While answering such questions in a funny manner, you’re also 
educating those people on how to use search. “Teach a man to fish” as they say. 
:trollface:

## Development
* Maarten Balliauw
* Juliën Hanssens
* Phil Haack
* Mateus de Morais

## Inspiration
[Let me Google that for you](http://lmgtfy.com/)

## Architecture

This repository is now structured for Azure Static Web Apps.

- `Lmbtfy.Web/wwwroot` contains the static frontend served by Static Web Apps.
- `api` contains the Azure Functions back end exposed through the `/api` route.

The original ASP.NET Core MVC app was refactored so that all server-side behavior now fits the Static Web Apps model:

- URL generation is handled by `POST /api/generate-url`
- The dynamic Unsplash background is handled by `GET /api/background`
- Friendly text URLs are interpreted client-side and rewritten to `/?q=...`

## Local development

Install the Azure Static Web Apps CLI and Azure Functions Core Tools if they are not already available.

Run the site locally from the repository root:

```bash
swa start
```

The checked-in `swa-cli.config.json` points the CLI at:

- app location: `Lmbtfy.Web/wwwroot`
- api location: `api`

To enable dynamic backgrounds locally, create `api/local.settings.json` from `api/local.settings.sample.json` and set `UnsplashAccessKey`.

## Deployment

The GitHub Actions workflow now deploys this repo as an Azure Static Web App using the `Azure/static-web-apps-deploy` action.

Set the following repository secret before enabling deployment:

- `AZURE_STATIC_WEB_APPS_API_TOKEN`

## Notes

- `Lmbtfy.Web/wwwroot/staticwebapp.config.json` configures navigation fallback and the managed Functions runtime.
- `api` targets .NET 8 isolated Azure Functions.

# Disclaimer
Let me Bing that for you is in no way associated with Microsoft nor Bing. It’s 
just something we did for fun.
