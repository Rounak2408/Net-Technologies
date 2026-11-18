# Experiment-08: Inventory Management System (InventoryHub)
## ASP.NET Core MVC using ADO.NET & SQL Server (Full CRUD)

A complete, enterprise-grade **Inventory Management System** built strictly using **ASP.NET Core MVC**, **C#**, **ADO.NET**, and **SQL Server**.

> **Note on Architecture:** This project directly implements ADO.NET (`SqlConnection`, `SqlCommand`, `SqlDataReader`, `SqlParameter`, `ExecuteNonQuery`, `ExecuteReader`, `ExecuteScalar`) without Entity Framework or EF Core.

---

### 1. Technology Stack
- **Framework:** ASP.NET Core MVC (.NET 10)
- **Language:** C#
- **Data Access:** ADO.NET (`Microsoft.Data.SqlClient`)
- **Database:** Microsoft SQL Server / LocalDB (`InventoryDB`)
- **Frontend:** HTML5, CSS3, JavaScript, Bootstrap 5, Bootstrap Icons
- **View Engine:** Strongly Typed Razor Views (`@model Product`, `@model DashboardViewModel`)
- **Security:** Parameterized SQL queries to prevent SQL Injection & Anti-Forgery Token validation
- **State Management:** `TempData` for post-redirect flash alert notifications

---

### 2. Database Schema & Architecture

#### Database: `InventoryDB`
#### Table: `Products`

| Column Name | Data Type | Constraints | Description |
| :--- | :--- | :--- | :--- |
| **ProductId** | `INT` | `PRIMARY KEY, IDENTITY(1,1)` | Auto-incrementing unique product identifier |
| **ProductName** | `NVARCHAR(100)` | `NOT NULL` | Name / Model of the product |
| **Category** | `NVARCHAR(50)` | `NOT NULL` | Product category (Electronics, Furniture, etc.) |
| **Description** | `NVARCHAR(500)` | `NULL` | Detailed specifications or notes |
| **Quantity** | `INT` | `NOT NULL, CHECK (Quantity >= 0)` | Stock units available in warehouse |
| **UnitPrice** | `DECIMAL(10,2)` | `NOT NULL, CHECK (UnitPrice > 0)` | Price per unit |
| **SupplierName**| `NVARCHAR(100)` | `NOT NULL` | Vendor or distributor name |
| **CreatedDate** | `DATETIME` | `NOT NULL, DEFAULT GETDATE()` | Timestamp when product was added |

A complete SQL creation and seed script is provided in:
📁 `Experiment-08/Database/Create_InventoryDB.sql`

*(Note: The application also includes an automatic database initializer in `ProductRepository.cs` that auto-verifies and creates `InventoryDB` and the `Products` table with seed data if they do not exist).*

---

### 3. ADO.NET Implementation Highlights

All data operations are performed using direct ADO.NET primitives:

- **`SqlConnection`**: Manages connections via connection strings in `appsettings.json`.
- **`SqlCommand`**: Prepares and executes parameterized SQL commands.
- **`SqlDataReader`**: High-performance forward-only reader used in `GetAll()` and `GetById()`.
- **`SqlParameter`**: Prevents SQL injection across all input fields.
- **`ExecuteNonQuery()`**: Used for `UPDATE` and `DELETE` operations.
- **`ExecuteReader()`**: Used for retrieving single and multiple records.
- **`ExecuteScalar()`**: Used for `INSERT` (capturing `SCOPE_IDENTITY()`), `COUNT(*)`, and `SUM()` aggregates.

---

### 4. Project Directory Structure

```text
Experiment-08/
├── Controllers/
│   └── ProductController.cs            # Index, Details, Create (GET/POST), Edit (GET/POST), Delete (POST)
├── Data/
│   ├── IProductRepository.cs           # Repository contract for CRUD & aggregations
│   └── ProductRepository.cs            # Pure ADO.NET implementation with SQL Server connectivity
├── Database/
│   └── Create_InventoryDB.sql          # Standalone SQL Server creation & seed script
├── Models/
│   ├── Product.cs                      # Product model with Data Annotations validation
│   ├── DashboardViewModel.cs           # Analytics stats and filtered product list
│   └── ErrorViewModel.cs
├── Views/
│   ├── Product/
│   │   ├── Index.cshtml                # Dashboard cards, search/filter toolbar, product table
│   │   ├── Create.cshtml               # Product registration form with validation
│   │   ├── Edit.cshtml                 # Edit product details form
│   │   └── Details.cshtml              # Full product specifications & stock level badge
│   ├── Shared/
│   │   ├── _Layout.cshtml              # Master admin layout with navbar, alerts & footer
│   │   ├── Error.cshtml
│   │   └── _ValidationScriptsPartial.cshtml
│   ├── _ViewImports.cshtml
│   └── _ViewStart.cshtml
├── wwwroot/
│   ├── css/
│   │   └── style.css                   # Navy blue & light blue admin dashboard theme
│   └── js/
│       └── site.js                     # Auto-dismiss alerts & client scripts
├── appsettings.json                    # SQL Server connection string
├── Experiment-08.csproj                # Project file with Microsoft.Data.SqlClient
└── run.bat                             # One-click startup script (http://localhost:5080)
```

---

### 5. Configuring SQL Server Connection String

Open `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=InventoryDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```

- For **SQL Server Express**: change `Server=(localdb)\\MSSQLLocalDB` to `Server=.\\SQLEXPRESS`.
- For **SQL Server Authentication**: specify `User Id=your_username;Password=your_password;`.

---

### 6. How to Run the Application

#### Option A: One-Click Script (Recommended)
Double-click `run.bat` inside `Experiment-08` or run:
```powershell
.\Experiment-08\run.bat
```
Browser opens automatically at `http://localhost:5080`.

#### Option B: Terminal Command
```powershell
cd .\Experiment-08
dotnet run --urls "http://localhost:5080"
```
Open [http://localhost:5080](http://localhost:5080) in your web browser.
