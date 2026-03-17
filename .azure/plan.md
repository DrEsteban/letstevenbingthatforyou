# Azure Static Web Apps Migration Plan

## Status

Ready for Validation

## Scenario

Refactor the existing ASP.NET Core MVC application into an Azure Static Web Apps solution that uses:

- A static frontend for all HTML, CSS, JavaScript, and images
- Azure Functions for server-side behavior under the `/api` route

## Current Application Analysis

### Existing server-side functionality

1. MVC routing for `/`, `/Home/About`, `/Home/GenerateUrl`, and a catch-all route that redirects friendly text paths into `/?q=...`
2. Anti-forgery validation for the AJAX request that generates a shareable URL
3. TinyURL integration performed on the server
4. Dynamic background image selection performed in the Razor layout via Unsplash and a daily keyword service
5. In-memory caching of the selected background image for 24 hours

### Existing client-side functionality

1. Bing-style landing page and form interactions
2. Animated “Let me Bing that for you” walkthrough when `q` is present
3. Static CSS, JavaScript, and image assets that can remain frontend-hosted

## Target Architecture

### Frontend

- Static site hosted by Azure Static Web Apps
- Client-side routing logic for:
  - root page
  - about page
  - query-string-driven animation page
  - legacy friendly text paths
- JavaScript-driven rendering for generated links and background images

### API

- Azure Functions .NET isolated project under `api/`
- HTTP-triggered endpoints:
  - `POST /api/generate-url`
  - `GET /api/background`
- Shared services for daily keyword selection, URL generation, TinyURL access, and Unsplash access

## Design Decisions

1. Replace Razor views with static HTML and client-side DOM updates
2. Replace the MVC partial response with JSON returned from `POST /api/generate-url`
3. Remove ASP.NET Core anti-forgery validation because the MVC pipeline no longer exists in Static Web Apps; use a JSON POST endpoint instead
4. Keep the Unsplash access key server-side by calling Unsplash from an Azure Function
5. Preserve the existing user-facing URL patterns through Static Web Apps navigation fallback and client-side route interpretation

## Planned Repository Changes

1. Add a new Azure Functions project under `api/`
2. Add a static frontend under `app/`
3. Reuse the existing CSS, JavaScript, and image assets where practical
4. Add Static Web Apps configuration and local development configuration generated through the SWA CLI when possible
5. Update the solution and test project to include the new backend implementation where needed
6. Update the README with local development and deployment guidance for Static Web Apps

## Validation Plan

1. Build the Azure Functions project
2. Run the unit tests
3. Validate that the static frontend references the new `/api` endpoints correctly
4. Validate friendly path handling and query-string rendering logic

## Azure Notes

- Target platform: Azure Static Web Apps free tier
- API model: Azure Functions managed by Static Web Apps
- Recommended local development tool: Azure Static Web Apps CLI
