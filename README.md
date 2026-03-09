# 🚀 Web API Shop | .NET 9 
**A professional RESTful API built with C# and .NET 9, following Clean Architecture principles.**

---

## 🏗️ Architecture & Layers
The project is structured into clearly separated layers to ensure **maintainability**, **testability**, and **scalability**. 

| Layer | Responsibility |
| :--- | :--- |
| **Application** | Entry point (Controllers), Middleware, and API Orchestration. |
| **Services** | Core business logic and domain operations. |
| **Repositories** | Data access abstraction and Database communication. |
| **Entities** | Database models (generated via Database First). |
| **DTOs** | Record-based objects for decoupled data transfer. |

> **Decoupling:** All layers are connected via **Dependency Injection (DI)**, making the system loosely coupled and easy to extend.

---

## 🔑 Key Features

### ⚡ Performance & Scalability
* **Asynchronous Everything:** All I/O operations (Database access) use `async/await` to free up threads and handle high loads.
* **Traffic Auditing:** Every request is logged into a dedicated `Rating` table for monitoring and analytics.

### 📦 Modern Data Handling
* **C# Records:** DTOs are implemented as `records`, providing immutability and concise syntax.
* **AutoMapper:** High-performance mapping between Entities and DTOs to prevent manual mapping boilerplate.
* **EF Core (ORM):** Utilizing Entity Framework Core for strongly-typed, LINQ-based data access.

### 🛡️ Reliability & Monitoring
* **Centralized Error Handling:** A custom `ErrorHandlingMiddleware` catches all exceptions globally, returning consistent JSON responses.
* **Structured Logging:** Deep integration with **NLog** for tracking application flow and debugging.
* **Configuration Management:** All settings (Connection Strings, etc.) are managed via `appsettings.json`.

---

## 🛠️ Tech Stack

| Technology | Purpose |
| :--- | :--- |
| **.NET 9 / C#** | Core Framework & Language |
| **EF Core** | Object-Relational Mapping (ORM) |
| **AutoMapper** | Object-to-Object Mapping |
| **NLog** | Structured Logging |
| **xUnit** | Unit & Integration Testing |

---

## 🧪 Testing Suite
The project includes a comprehensive test project covering:
* **Unit Tests:** Testing individual business logic in isolation.
* **Integration Tests:** Verifying the flow between the API, Services, and Database.

---

## 🚀 Getting Started

### Prerequisites
* [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
* SQL Server

### Installation & Run
1. **Clone the repository:**
   ```bash
   git clone [your-repository-url]



2.**Restore dependencies:
   ```bash
		dotnet restore

3.**Run the project:
   ```bash
	dotnet run --project WebApiShop

4.**Run Tests:
   ```bash
	dotnet test





