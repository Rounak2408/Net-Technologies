# Experiment-09: Placement Management Dashboard (PlacementHub)
## Exercise 9 – ASP.NET Web Forms using Data Controls & ADO.NET

A comprehensive, enterprise-ready **Placement Management Dashboard** developed using **ASP.NET Web Forms**, **C#**, **ADO.NET**, and **SQL Server**.

> **Focus of Practical:** This experiment explicitly demonstrates the core **ASP.NET Server-Side Data Controls** (`GridView`, `DetailsView`, `FormView`, `Repeater`, `DataList`) with real SQL Server database persistence and data binding.

---

### 1. Technology Stack
- **Framework:** ASP.NET Web Forms (.NET Framework 4.8)
- **Language:** C#
- **Data Access:** Pure ADO.NET (`System.Data.SqlClient`)
- **Database:** Microsoft SQL Server / LocalDB (`PlacementDB`)
- **Frontend:** HTML5, CSS3, JavaScript, Bootstrap 5, Bootstrap Icons
- **Server Controls:** `<asp:GridView>`, `<asp:DetailsView>`, `<asp:FormView>`, `<asp:Repeater>`, `<asp:DataList>`
- **Validation Controls:** `RequiredFieldValidator`, `RegularExpressionValidator`, `RangeValidator`, `ValidationSummary`
- **Currency:** Indian Rupees (₹ LPA - Lakhs Per Annum)

---

### 2. ASP.NET Data Controls Implemented

| Data Control | Location | Description & Capabilities |
| :--- | :--- | :--- |
| **`<asp:GridView>`** | `Students.aspx` | • `AutoGenerateColumns="false"`<br>• `BoundField` for student attributes<br>• `CommandField` for inline Edit, Delete, and Select<br>• Server-side Paging (`AllowPaging="true"`)<br>• Dynamic Column Sorting (`AllowSorting="true"`)<br>• Status badges for Placed & Not Placed |
| **`<asp:DetailsView>`** | `Students.aspx` | • Complete student profile view<br>• Navigation paging through student records (`AllowPaging="true"`)<br>• Shows Name, Email, Department, CGPA, Company, Package |
| **`<asp:FormView>`** | `Students.aspx` | • `InsertItemTemplate` for student registration<br>• Dropdown binding for Department, Status, and Companies<br>• Integrated with `RequiredFieldValidator`, `RegularExpressionValidator`, `RangeValidator` |
| **`<asp:Repeater>`** | `Companies.aspx` | • Lightweight templated control for recruiting company cards<br>• Displays Company Name, Role, Location, Package (₹ LPA), Drive Date |
| **`<asp:DataList>`** | `Default.aspx` | • `RepeatColumns="3"`, `RepeatDirection="Horizontal"`<br>• Showcases placed students in an attractive card grid |

---

### 3. Database Design: `PlacementDB`

#### Tables:
1. **`Companies` Table**:
   - `CompanyId` (INT, Primary Key, Identity)
   - `CompanyName` (NVARCHAR(150), NOT NULL)
   - `Location` (NVARCHAR(100), NOT NULL)
   - `JobRole` (NVARCHAR(100), NOT NULL)
   - `Package` (DECIMAL(10,2), NOT NULL) – in LPA (Lakhs Per Annum)
   - `DriveDate` (DATE, NOT NULL)

2. **`Students` Table**:
   - `StudentId` (INT, Primary Key, Identity)
   - `FullName` (NVARCHAR(100), NOT NULL)
   - `Email` (NVARCHAR(100), NOT NULL)
   - `Department` (NVARCHAR(100), NOT NULL)
   - `CGPA` (DECIMAL(3,2), NOT NULL)
   - `GraduationYear` (INT, NOT NULL)
   - `PlacementStatus` (NVARCHAR(30), NOT NULL) – `Placed` / `Not Placed`
   - `CompanyId` (INT, Foreign Key referencing `Companies(CompanyId)`)

Complete creation and seed script available in:
📁 `Experiment-09/Database/Create_PlacementDB.sql`

---

### 4. Dashboard Metrics (`Default.aspx`)

- **Total Students** (Registered Batch Count)
- **Total Companies** (Campus Recruitment Partners)
- **Students Placed** (Offers Secured)
- **Students Not Placed** (Students in Pipeline)
- **Placement Percentage** (Dynamic calculation with animated progress bar)
- **Highest Package** (Top CTC in ₹ LPA)
- **Average Package** (Mean Annual CTC in ₹ LPA)

---

### 5. Project Directory Structure

```text
Experiment-09/
├── App_Code/
│   ├── Data/
│   │   └── DBConnection.cs             # Reusable ADO.NET helper & DB auto-initializer
│   └── Models/
│       ├── Student.cs                  # Student data model
│       └── Company.cs                  # Company data model
├── Database/
│   └── Create_PlacementDB.sql          # SQL database and table creation script
├── Content/
│   └── style.css                       # Navy Blue & White theme, custom table & card CSS
├── Scripts/
│   └── site.js                         # Application JavaScript & auto-dismiss alerts
├── Default.aspx                        # Dashboard metrics & <asp:DataList> placed students
├── Default.aspx.cs                     # Code-behind for Default.aspx
├── Students.aspx                       # <asp:GridView>, <asp:DetailsView> & <asp:FormView>
├── Students.aspx.cs                    # Code-behind with CRUD, paging & sorting logic
├── Companies.aspx                      # <asp:Repeater> corporate cards & add company modal
├── Companies.aspx.cs                   # Code-behind for Companies.aspx
├── PlacementReports.aspx               # Department-wise analytics & CTC tier reports
├── PlacementReports.aspx.cs            # Code-behind for reports
├── Web.config                          # Connection string & ASP.NET configuration
├── WebServer.cs                        # Native ASP.NET self-host server
└── run.bat                             # One-click startup script (http://localhost:5090)
```

---

### 6. How to Run the Application

#### Option A: One-Click Script (Recommended)
Double-click `run.bat` in `Experiment-09` or run in terminal:
```powershell
.\Experiment-09\run.bat
```
Browser opens automatically at `http://localhost:5090/Default.aspx`.

#### Option B: Standalone WebServer
```powershell
cd Experiment-09
.\bin\WebServer.exe
```
Open [http://localhost:5090/Default.aspx](http://localhost:5090/Default.aspx) in your web browser.
