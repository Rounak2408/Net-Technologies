# Experiment-10: Student Management REST API
## Exercise 10 – ASP.NET Core Web API (Middleware, Clean Architecture & SQL Server)

A complete, production-grade **Student Management REST API** built with **ASP.NET Core Web API**, **C#**, **ADO.NET**, and **SQL Server**.

This project demonstrates clean layered architecture, RESTful API design, dependency injection, custom middleware pipeline, and full database persistence.

---

### 1. Technology Stack
- **Framework:** ASP.NET Core Web API (.NET 10 / .NET 8 compatible)
- **Language:** C#
- **Database:** Microsoft SQL Server / LocalDB (`StudentManagementDB`)
- **Data Access:** Pure ADO.NET (`Microsoft.Data.SqlClient`) with Parameterized Queries
- **Documentation & Testing:** Swagger / OpenAPI UI (`Swashbuckle.AspNetCore`)
- **Architecture:** Controller → Service Layer → Repository Layer → Database

---

### 2. REST API Endpoints Specification

| HTTP Method | Endpoint | Status Code | Description |
| :--- | :--- | :--- | :--- |
| **`GET`** | `/api/students` | `200 OK` | Retrieves all students (Supports `?search=...` & `?department=...`) |
| **`GET`** | `/api/students/{id}` | `200 OK`, `404 Not Found` | Retrieves a single student record by unique ID |
| **`POST`** | `/api/students` | `201 Created`, `400 Bad Request` | Creates a new student record (Validates required fields & unique email) |
| **`PUT`** | `/api/students/{id}` | `200 OK`, `400 Bad Request`, `404 Not Found` | Updates an existing student record |
| **`DELETE`** | `/api/students/{id}` | `200 OK`, `404 Not Found` | Deletes a student record by ID |

---

### 3. Custom Middleware Pipeline

Registered in correct execution sequence inside `Program.cs`:

1. **`ExceptionMiddleware` (Global Exception Handler):**
   - Outermost middleware that catches unhandled exceptions.
   - Returns consistent JSON response: `{ success: false, message: "...", statusCode: 500 }`.
   - Prevents stack trace leakage in production.

2. **`RequestTimingMiddleware` (Execution Performance):**
   - Measures exact request-to-response duration using high-precision `Stopwatch`.
   - Injects custom HTTP response header: `X-Response-Time-ms: <milliseconds>`.

3. **`RequestLoggingMiddleware` (Telemetry & Audit):**
   - Logs incoming HTTP Method, Path, and UTC Timestamp.
   - Logs outgoing Response Status Code upon completion.

---

### 4. Standardized API Response Structure

#### Success Response Example (`200 OK` / `201 Created`):
```json
{
  "success": true,
  "message": "Student created successfully",
  "statusCode": 201,
  "data": {
    "studentId": 1,
    "fullName": "Aarav Sharma",
    "email": "aarav.sharma@example.edu",
    "phone": "9876543210",
    "department": "Computer Science",
    "semester": 6,
    "cgpa": 9.45,
    "createdAt": "2026-11-18T13:20:13.902Z"
  }
}
```

#### Error Response Example (`404 Not Found`):
```json
{
  "success": false,
  "message": "Student with ID 999 was not found",
  "statusCode": 404,
  "data": null
}
```

---

### 5. Project Directory Structure

```text
Experiment-10/
├── Controllers/
│   └── StudentsController.cs           # HTTP endpoints with Swagger attributes
├── DTOs/
│   ├── StudentCreateDto.cs             # Creation payload with Data Annotations
│   └── StudentUpdateDto.cs             # Update payload with Data Annotations
├── Models/
│   ├── Student.cs                      # Student domain model
│   └── ApiResponse.cs                  # Generic standardized response wrapper
├── Services/
│   ├── IStudentService.cs              # Business logic interface
│   └── StudentService.cs               # Business rules & response wrapping
├── Repositories/
│   ├── IStudentRepository.cs           # Database operations interface
│   └── StudentRepository.cs            # Pure ADO.NET SQL Server async operations
├── Middleware/
│   ├── RequestLoggingMiddleware.cs     # Request & response telemetry
│   ├── ExceptionMiddleware.cs          # Global exception handler
│   └── RequestTimingMiddleware.cs      # Performance timer & X-Response-Time-ms header
├── Database/
│   └── Create_StudentManagementDB.sql  # SQL Server creation & seed script
├── appsettings.json                    # Connection strings & logging config
├── Experiment-10.csproj                # Project file with Swashbuckle & SqlClient
├── Program.cs                          # Dependency injection & middleware pipeline
└── run.bat                             # One-click startup script (http://localhost:5100)
```

---

### 6. Database Schema: `StudentManagementDB`

```sql
CREATE TABLE Students (
    StudentId INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL CONSTRAINT UQ_Students_Email UNIQUE,
    Phone NVARCHAR(15) NULL,
    Department NVARCHAR(100) NOT NULL,
    Semester INT NOT NULL CHECK (Semester >= 1 AND Semester <= 8),
    CGPA DECIMAL(3,2) NOT NULL CHECK (CGPA >= 0.00 AND CGPA <= 10.00),
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
);
```

*(Note: The application includes auto-setup logic in `StudentRepository.cs` that verifies and creates `StudentManagementDB` and seeds initial student records on startup).*

---

### 7. How to Run & Test the Application

#### Option A: One-Click Script (Recommended)
Double-click `run.bat` or run:
```powershell
.\Experiment-10\run.bat
```
Browser opens immediately at **[http://localhost:5100](http://localhost:5100)** to interactive **Swagger UI**.

#### Option B: Terminal Command
```powershell
cd Experiment-10
dotnet run --urls "http://localhost:5100"
```

#### Option C: Testing via cURL / PowerShell
```powershell
# Get all students
curl.exe http://localhost:5100/api/students

# Get student by ID
curl.exe http://localhost:5100/api/students/1

# Create a student
curl.exe -X POST http://localhost:5100/api/students -H "Content-Type: application/json" -d "{\"fullName\":\"Rohan Verma\",\"email\":\"rohan.v@example.edu\",\"phone\":\"9876500000\",\"department\":\"Computer Science\",\"semester\":5,\"cgpa\":8.90}"
```
