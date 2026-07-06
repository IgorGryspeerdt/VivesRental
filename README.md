# VivesRental

A rental management web application built primarily with **C#** (with HTML, CSS, and JavaScript for the frontend). (school project)

## Overview

VivesRental is a full-stack web project focused on rental workflows (such as listing, managing, and/or booking rental items or properties).

## Tech Stack

- **Backend:** C#
- **Frontend:** HTML, CSS, JavaScript

## Repository

- **Owner:** IgorGryspeerdt
- **Repository:** VivesRental

### 1) Clone the repository

```bash
git clone https://github.com/IgorGryspeerdt/VivesRental.git
cd VivesRental
```

### 2) Restore dependencies

```bash
dotnet restore
```

### 3) Build

```bash
dotnet build
```

### 4) Run

```bash
dotnet run
```

## Project Structure (suggested)

Depending on your current layout, you may have folders like:

- `Controllers/` – request handling
- `Models/` – domain/data models
- `Views/` – UI templates/pages
- `wwwroot/` – static assets (CSS/JS/images)
- `Data/` – database context/migrations

## Configuration

Common .NET app configuration files:

- `appsettings.json`
- `appsettings.Development.json`

If your app uses a database, ensure your connection string is set correctly before running.

## Scripts & Styling

Frontend assets are likely located in `wwwroot/` or equivalent folders.

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Open a pull request
