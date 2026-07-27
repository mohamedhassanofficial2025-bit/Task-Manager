# TaskFlow — Task Manager

TaskFlow is a full-stack task management application built as a technical assessment for **ElectroPi** using **ASP.NET Core 10**, **Angular 21**, and **SQL Server**. It provides a simple UI for managing projects and tasks with CRUD operations, status updates, and filtering.

## Features

- Create, list, view, edit, and delete **projects**
- Create, list, view, edit, and delete **tasks**
- Change task status directly
- Filter tasks by status
- View a project with its related tasks
- Backend validation with FluentValidation
- Frontend form validation with Angular Reactive Forms
- Centralized exception handling middleware
- Seed data on first run
- Interactive API docs with Scalar
- Docker support for the backend

## Tech Stack

- **Backend:** ASP.NET Core 10, C#
- **ORM:** Entity Framework Core 10 (Code First)
- **Database:** SQL Server
- **Validation:** FluentValidation
- **Mapping:** AutoMapper
- **API Docs:** Scalar.AspNetCore
- **Frontend:** Angular 21, TypeScript
- **Styling:** CSS
- **Containerization:** Docker

## Architecture

The backend follows a **simplified Clean Architecture** with a modular monolith structure:

- **TaskManager.API** — controllers, middleware, and startup
- **TaskManager.Core** — entities, DTOs, services, validators, shared models
- **TaskManager.Infrastructure** — DbContext, repositories, configurations, migrations, seed data

Projects and Tasks are separated by feature, making the codebase easy to maintain and straightforward to split later if needed.

## API Overview

Base URL: `http://localhost:5045/api`

### Projects

- `GET /Projects`
- `GET /Projects/{id}`
- `GET /Projects/{id}/tasks`
- `POST /Projects`
- `PUT /Projects/{id}`
- `DELETE /Projects/{id}`

### Tasks

- `GET /Tasks`
- `GET /Tasks/{id}`
- `GET /Tasks/status/{status}`
- `POST /Tasks`
- `PUT /Tasks/{id}`
- `PATCH /Tasks/{id}/status`
- `DELETE /Tasks/{id}`

Scalar API documentation is available at:

`http://localhost:5045/scalar/v1`

## Setup

### Prerequisites

- .NET 10 SDK
- Node.js 18+
- SQL Server or LocalDB
- Angular CLI

### Backend

Update the connection string in `appsettings.json`, then run:

```bash
cd Backend/TaskManager.API/TaskManager.API
dotnet run
```

The database is migrated automatically on startup.

### Frontend

```bash
cd FrontEnd/task-Manager-ui
npm install
npm start
```

The frontend runs on `http://localhost:4200`.

### Docker

```bash
cd Backend/TaskManager.API
docker build -f TaskManager.API/Dockerfile -t taskmanager-api .
docker run -p 5045:8080 taskmanager-api
```

## Design Decisions

- Feature-based organization for Projects and Tasks
- Repository + Service pattern to keep controllers thin
- FluentValidation for expressive validation rules
- Result pattern for consistent service responses
- Angular standalone components for a simpler frontend structure
- Global exception middleware for centralized error handling

## Future Improvements

- Add authentication and authorization
- Add pagination and search
- Add unit and integration tests
- Add CI/CD with GitHub Actions
- Add real-time updates with SignalR

## License

This project was built as a technical assessment for **ElectroPi**.
