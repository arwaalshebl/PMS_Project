# PMS Project

A Project Management System built with ASP.NET Core MVC and Entity Framework Core. The application helps manage projects, tasks, sprints, and users with role-based access for Admin, Tech Lead, and Developer roles.

## Features

- Dashboard overview for tasks and projects
- Project management with parent/sub-project relationships
- Task tracking with status, sprint, assignment, and deadlines
- Sprint management
- User authentication and role-based authorization
- Audit logging for key actions
- ASP.NET Core MVC UI with Razor views
- SQL Server database support

## Tech Stack

- ASP.NET Core MVC
- .NET 9
- Entity Framework Core
- SQL Server / LocalDB
- ASP.NET Core Identity
- Razor Views

## Project Structure

```text
PMS_Project/
├── PMSProject/
│   ├── Controllers/
│   ├── Data/
│   ├── Migrations/
│   ├── Models/
│   ├── Views/
│   ├── wwwroot/
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   ├── Program.cs
│   ├── PMSProject.csproj
│   └── ...
├── PMSProject.slnx
├── .gitignore
├── .gitattributes
└── README.md
```

## Prerequisites

Before running this project, make sure you have:

- .NET 9 SDK installed
- SQL Server / LocalDB available
- Visual Studio 2022 or VS Code (optional)

## Configuration

The application uses a SQL Server connection string defined in `PMSProject/appsettings.json`.

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PMSDB;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

If you are using a different SQL Server instance, update the connection string accordingly.

## Run the Project

1. Clone the repository:

```bash
git clone https://github.com/arwaalshebl/PMS_Project.git
```

2. Open the project directory:

```bash
cd PMS_Project
```

3. Restore dependencies:

```bash
dotnet restore
```

4. Run the application:

```bash
dotnet run --project PMSProject
```

5. Open the app in your browser:

```text
https://localhost:5001
```

or the local URL shown in the terminal.

## Seeded Accounts

The application creates default users at startup using ASP.NET Core Identity.

- Admin
  - Email: admin@gmail.com
  - Password: @Arwa123
- Tech Lead
  - Email: TechLeader@gmail.com
  - Password: @Arwa123
- Developer
  - Email: Developer@gmail.com
  - Password: @Arwa123

## Main Modules

### Dashboard
Displays task and project summaries, filtering based on the logged-in user role.

### Projects
Allows creation and management of projects, including parent and sub-project relationships.

### Tasks
Supports assignment, task status updates, sprint association, notes, and dates.

### Sprints
Tracks sprint-based planning and work organization.

### Audit Logs
Records actions related to project/activity history for traceability.

## Database

The project uses Entity Framework Core with SQL Server. Migrations are present under the `PMSProject/Migrations` folder.

If needed, you can update the database with:

```bash
dotnet ef database update --project PMSProject
```

## License

This project is for educational/demo use unless otherwise stated by the repository owner.

## Author

Arwa Alshebl
