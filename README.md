# HAB X9 - Event Hall Management System

HAB X9 is a professional, cross-platform desktop application designed for comprehensive management of event halls and facilities. It provides a robust solution for handling bookings, managing resources, and streamlining operations.

## Key Features

- **Booking Management:** An interactive calendar for creating, viewing, editing, and deleting bookings.
- **Hall Management:** Manage multiple halls, their capacities, and rental prices.
- **Service Management:** Define and manage additional services (e.g., catering, sound systems) that can be added to bookings.
- **Inventory Control:** Keep track of inventory items, manage stock levels, and receive alerts for low stock.
- **Kitchen Dashboard:** A dedicated view for the kitchen staff to manage food and beverage orders linked to bookings.
- **User Management:** Role-based access control with predefined roles (Admin, Reception, Kitchen).
- **Financial Reporting:** Generate PDF sales reports with detailed summaries of revenue, discounts, and costs.
- **Secure Authentication:** User passwords are securely hashed using BCrypt.

## Tech Stack

- **Framework:** .NET 8
- **UI:** Avalonia UI (for cross-platform desktop applications)
- **Architecture:** MVVM (Model-View-ViewModel) with CommunityToolkit.Mvvm
- **Database:** SQLite
- **ORM:** Entity Framework Core 8
- **Reporting:** QuestPDF
- **Logging:** Serilog

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Setup & Installation

1.  **Clone the repository:**
    ```bash
    git clone <repository-url>
    cd X9-
    ```

2.  **Restore dependencies and build the project:**
    The first build will also create the SQLite database file (`hab_x9.db`) and apply the initial migration.
    ```bash
    dotnet build
    ```

3.  **Run the application:**
    ```bash
    dotnet run --project HabCo.X9.App/HabCo.X9.App.csproj
    ```

### Default Login

A default administrator account is created when the database is first seeded.

-   **Username:** `admin`
-   **Password:** `admin123`

## Project Structure

The solution follows a clean architecture approach, separating concerns into distinct projects:

-   **`HabCo.X9.Core`**: Contains the core business logic and domain entities (Models). This project has no dependencies on other layers.
-   **`HabCo.X9.Infrastructure`**: Manages data access, primarily through the Entity Framework Core `AppDbContext` and database migrations. It depends on `HabCo.X9.Core`.
-   **`HabCo.X9.App`**: The main Avalonia UI application project. It contains all Views, ViewModels, and application-specific services. It depends on both `HabCo.X9.Core` and `HabCo.X9.Infrastructure`.
-   **`HabCo.X9.Tests`**: Contains xUnit tests for the application, ensuring code quality and reliability.