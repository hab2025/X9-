# HAB X9 - Event Hall Management System

**HAB X9** is a professional, commercial-grade desktop application designed to streamline the complete management of event halls and venues. Built with a modern, robust architecture, it provides a comprehensive suite of tools to manage bookings, inventory, kitchen operations, user access, and financial reporting, all from a single, intuitive interface.

---

## ✨ Features

The system is packed with features designed to optimize the workflow of a busy event venue:

*   **Secure Authentication & Permissions:**
    *   A secure login system using industry-standard **BCrypt.Net** for password hashing.
    *   A powerful **Role-Based Access Control (RBAC)** system with pre-defined roles (Admin, Reception, Kitchen).
    *   The user interface dynamically adapts, showing only the features a user is authorized to access.

*   **Full-Fledged Booking System:**
    *   An interactive **booking calendar** providing a clear visual overview of all events.
    *   Full CRUD (Create, Read, Update, Delete) capabilities for bookings.
    *   Ability to add **customizable services** (e.g., DJ, Photography) to any booking.
    *   A **dynamic pricing engine** that automatically calculates the total cost, including services and flexible discounts.
    *   Support for both **fixed amount** and **percentage-based** discounts on any booking.

*   **Comprehensive Management Modules:**
    *   **Hall Management:** Add, edit, and delete event halls, specifying their capacity and rental price.
    *   **User Management:** A dedicated admin panel to create, edit, and deactivate user accounts and assign roles.
    *   **Service Management:** A dedicated admin panel to manage the list of offered services and their prices.

*   **Integrated Operations:**
    *   **Inventory Management:** Track stock levels, units, and suppliers. Features visual alerts for low-stock items.
    *   **Kitchen Dashboard:** A dedicated command center for kitchen staff, showing all upcoming orders linked to bookings and their required inventory items.
    *   **Status Tracking** for kitchen orders (e.g., Pending, In Progress, Completed).

*   **Powerful Reporting:**
    *   A **Sales Reporting** module to generate detailed financial reports in PDF format.
    *   Reports include a full breakdown of gross revenue, discounts applied, and final net revenue for any specified date range.

---

## 💻 Technologies Used

HAB X9 is built on a modern and reliable technology stack:

*   **Backend & Core Logic:** .NET (C#)
*   **User Interface:** Avalonia UI (for a truly cross-platform desktop experience on Windows, macOS, and Linux)
*   **Database ORM:** Entity Framework Core
*   **Database:** SQLite (for portability and ease of setup)
*   **PDF Reporting:** QuestPDF
*   **Security:** BCrypt.Net for password hashing

---

## 🚀 Installation Guide

To get the application running on your local machine, follow these simple steps:

1.  **Prerequisites:** Ensure you have the [.NET SDK](https://dotnet.microsoft.com/download) (version 8.0 or later) installed.

2.  **Clone the Repository:**
    ```bash
    git clone <repository-url>
    cd <repository-directory>
    ```

3.  **Run the Application:** Navigate to the main application project and use the `dotnet run` command. The application will automatically create and set up its SQLite database on the first run.
    ```bash
    cd HabCo.X9.App
    dotnet run
    ```

4.  **Login:** Use the default administrator credentials to log in:
    *   **Username:** `admin`
    *   **Password:** `admin`

---

## 📞 Support

For any inquiries, bug reports, or feature requests, please open an issue on the GitHub repository or contact our support team at `support@habco-x9.com` (placeholder).