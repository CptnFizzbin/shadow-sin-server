# shadow-sin-server

A C# ASP.NET Core 8 Web API with JWT authentication and a PostgreSQL database.

## Features

- **User registration & login** via JWT Bearer tokens
- **ASP.NET Core Identity** for password hashing and user management
- **Entity Framework Core** with Npgsql (PostgreSQL) and auto-migration on startup
- **Swagger / OpenAPI** UI available in development
- **Docker & Docker Compose** support

## Endpoints

| Method | Path | Auth | Description |
|--------|------|------|-------------|
| POST | `/api/auth/register` | — | Register a new user |
| POST | `/api/auth/login` | — | Login and receive a JWT token |
| GET | `/api/auth/me` | Bearer | Get the current user's profile |

## Running with Docker Compose

```bash
# Copy and customize environment values
cp docker-compose.yml docker-compose.override.yml   # optional

docker compose up --build
```

The API will be available at `http://localhost:8080`.

> **Important:** Change the `Jwt__Secret` and `POSTGRES_PASSWORD` values before deploying to production.

## Running locally

Prerequisites: .NET 8 SDK, a PostgreSQL instance.

1. Update `src/ShadowSinServer/appsettings.Development.json` with your local DB connection string.
2. Run:

```bash
cd src/ShadowSinServer
dotnet run
```

The Swagger UI will be at `https://localhost:<port>/swagger`.

## Project structure

```
src/
  ShadowSinServer/
    Controllers/     # AuthController
    Data/            # AppDbContext + EF migrations
    DTOs/            # Request / response records
    Models/          # ApplicationUser
    Services/        # TokenService (JWT generation)
    Program.cs
    appsettings.json
Dockerfile
docker-compose.yml
ShadowSinServer.sln
```
