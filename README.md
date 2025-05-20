# 📚 Library Management System

A web-based Library Management System built with **ASP.NET Core MVC** and **Entity Framework Core**. This system allows administrators to manage books, authors, and users, while providing users the ability to borrow and return books. The application runs on **.NET 8** and uses **SQL Server** within a Docker container for easy setup and deployment.

---

## 🚀 Tech Stack

- **Backend Framework:** ASP.NET Core MVC (.NET 8)
- **Database ORM:** Entity Framework Core
- **Authentication:** Claims-based authorization using ASP.NET Core Identity
- **Database:** SQL Server (via Docker)
- **Package Manager:** NuGet
- **Containerization (just for SQLserver):** Docker

---

## 🛠️ Features
- Default user admin: david123 - david123
- Default user: turing123 - turing123

### 👤 User Roles

- **User**
  - Register and update profile
  - Borrow and return books
- **Admin**
  - Full control over users, books, and authors

### 📘 Book Management

- Add, edit, delete, and view books
- One copy per user (by ISBN)
- Admin only functionality

### 🖋️ Author Management

- CRUD operations for authors
- Admin only functionality

### 📦 Inventory

- Tracks user-book relationships
- Prevents multiple borrowing of the same book
- (Planned) Borrow and return history

---

## 🧪 NuGet Packages Used

- `Microsoft.EntityFrameworkCore`
- `Microsoft.EntityFrameworkCore.SqlServer`
- `Microsoft.EntityFrameworkCore.Tools`
- `Microsoft.AspNetCore.Authentication.Cookies`
- `Microsoft.AspNetCore.Authorization`
- `Microsoft.AspNetCore.Identity.EntityFrameworkCore`
- `Microsoft.VisualStudio.Web.CodeGeneration.Design`

---

## 🧭 Getting Started

### 1. Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- Visual Studio 2022 or later (with ASP.NET and web development workload)

---

### 2. Run SQL Server with Docker

From the root folder of the project:

```bash
docker compose up
```
Package management console
```
Update-databse
```
