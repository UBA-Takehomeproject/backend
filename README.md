# BlogPlatform.API 

A clean and modular ASP.NET Core Web API built using **Onion Architecture** principles, integrated with **Entity Framework Core**, **Swagger**, and ready for collaborative development.

---

##  Architecture Overview

This project uses a layered Onion Architecture:

```
├── BlogPlatform.Domain          → Core entities and interfaces (no dependencies)
├── BlogPlatform.Application     → Use cases, services, DTOs
├── BlogPlatform.Infrastructure → EF Core, repositories, unit of work
├── BlogPlatform.API            → Web API entry point (controllers, Swagger)
```

---

## Getting Started

###  Prerequisites

- [.NET SDK 9](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- SQL Server (local or remote)
- VS Code or Visual Studio

---

###  Setup Instructions

1. **Clone the repository:**

```bash
git clone https://github.com/your-username/BlogPlatform.API.git
cd BlogPlatform.API
```

2. **Restore dependencies:**

```bash
dotnet restore
```

3. **Set up your database connection:**

Edit `BlogPlatform.API/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=BlogPlatformDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

---

## 🛠 Entity Framework Core Commands

> Run these from the root folder.

###  Add a Migration

```bash
dotnet ef migrations add YourMigrationName --project BlogPlatform.Infrastructure --startup-project BlogPlatform.API
```

###  Update the Database

```bash
dotnet ef database update --project BlogPlatform.Infrastructure --startup-project BlogPlatform.API
```

###  Remove Last Migration

```bash
dotnet ef migrations remove --project BlogPlatform.Infrastructure --startup-project BlogPlatform.API
```

---

##  Run the API

```bash
dotnet run --project BlogPlatform.API
```

Swagger UI will be available at:

```
https://localhost:5001/swagger
```

---

##  Developer Tips

- Use `./ef-migrate.sh` (if available) to automate migrations.
- Use `launch.json` in VS Code for debugging.
- Keep your EF and business logic strictly in `Infrastructure` and `Application` layers.

---

##  Folder Structure Summary

```
/BlogPlatform.API           → Entry point (Controllers, Swagger, Program.cs)
/BlogPlatform.Infrastructure → EF Core DbContext, Repositories, Unit of Work
/BlogPlatform.Application    → Application Services and Interfaces
/BlogPlatform.Domain         → Domain Entities and Contracts
```

---

##  License

MIT License — free to use and extend.

---

##  Contributing

Pull requests are welcome! Please follow the code style used in the repo and update documentation as needed.
