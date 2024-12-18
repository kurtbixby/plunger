# Plunger
I was managing my video game backlog/queue with a Google sheet and decided to make a web app to replace it.

Currently a WIP, but the API is functional. The UI needs to integrate with the API and be built.

Hosted at [URL TBD]()

## General Components
### Current Components
* PostgreSQL database to hold all of the game data and the user-created data (lists, collections, etc)
* .NET Core C# command line program to fill the games database with information from the [IGDB](https://www.igdb.com/)
* ASP.NET Core C# Web API to create an interface for the database
* React-based frontend for the UI

### Future Component Changes
* Additional endpoint to update the game database with changes pushed by the IGDB
* Add a caching layer with a batching process to shrink the self-hosted game data size

## Tech Stacks
### Frontend
* React
* TailwindCSS
    * Used in v0 of the UI and in the older components
* Chakra UI
    * Used for new components and rewrites of older ones
* TanStack Query (aka React Query)
* Hosted in Azure as a Static Web App

### Backend
* Written in C# using ASP.NET
* Data stored in a PostgreSQL DB
* Database hosted by Neon.tech
* Deployed as an Azure Web App

## Functionality
### Current Functionality
* Creating a collection of games that a user owns
* Tracking the "play state" of games (Unplayed, In Progress, Completed, Paused)

### Future Functionality
* A finished user interface
* Creating ordered lists of games a user is interested in playing
* Support for game dependency play orders
* Auto-populated queue functionality using dependency graph to automatically surface the next games a user is interested in playing
* Per-entity privacy, e.g. lists, collection items, and played games can be designated as private