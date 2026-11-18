# Experiment-07: Feedback Management Module (FeedbackHub)
## ASP.NET Core MVC Architecture

A professional, responsive, and robust **Feedback Management System** built with **ASP.NET Core MVC**.

---

### 1. Technology Stack
- **Framework:** ASP.NET Core MVC (.NET 10)
- **Language:** C#
- **Frontend:** HTML5, CSS3, JavaScript, Bootstrap 5, Bootstrap Icons
- **View Engine:** Razor Views with Strongly Typed Models
- **Form Generation:** Razor HTML Helpers (`Html.BeginForm`, `Html.TextBoxFor`, `Html.DropDownListFor`, `Html.TextAreaFor`, `Html.ValidationMessageFor`, `Html.ValidationSummary`, `Html.HiddenFor`)
- **Data Validation:** Data Annotations (`[Required]`, `[EmailAddress]`, `[RegularExpression]`, `[StringLength]`, `[Range]`)
- **Client-Side Validation:** jQuery, jQuery Validate, jQuery Unobtrusive Validation
- **Server-Side Validation:** ASP.NET Core `ModelState.IsValid`
- **State Management:** `TempData` for post-redirect flash notifications

---

### 2. Project Architecture & File Structure

```text
Experiment-07/
├── Controllers/
│   └── FeedbackController.cs           # Index, Create (GET/POST), List, Delete (POST) actions
├── Models/
│   ├── Feedback.cs                     # Model with Data Annotations & Validation rules
│   └── ErrorViewModel.cs               # Standard error handling model
├── Services/
│   ├── IFeedbackRepository.cs          # Repository interface contract
│   └── FeedbackRepository.cs           # Thread-safe in-memory data store with realistic seed data
├── Views/
│   ├── Shared/
│   │   ├── _Layout.cshtml              # Master layout (Navbar, Alert container, Footer, Scripts)
│   │   ├── _Header.cshtml              # Modular navigation partial view
│   │   ├── _Footer.cshtml              # Modular responsive footer partial view
│   │   └── _ValidationScriptsPartial.cshtml # jQuery unobtrusive validation scripts
│   ├── Feedback/
│   │   ├── Index.cshtml                # Dashboard overview with live stats and quick links
│   │   ├── Create.cshtml               # Feedback submission page with validation guidelines
│   │   ├── List.cshtml                 # All feedback cards with category filters & empty state
│   │   ├── _FeedbackForm.cshtml        # Reusable form partial using Razor HTML Helpers
│   │   └── _FeedbackCard.cshtml        # Reusable card partial with star rating & delete form
│   ├── _ViewImports.cshtml             # Tag Helpers and namespace imports
│   └── _ViewStart.cshtml               # Global layout configuration
├── wwwroot/
│   ├── css/
│   │   └── style.css                   # Custom theme styling (Navy palette, animations, cards)
│   └── js/
│       └── site.js                     # Interactive star rating widget, char counter, auto-dismiss alerts
├── Program.cs                          # DI service registration and MVC route mapping
├── Experiment-07.csproj                # Project configuration file
└── run.bat                             # One-click startup script (http://localhost:5070)
```

---

### 3. Key Functional Features

1. **Dashboard Overview (`/Feedback/Index`):**
   - High-level metric cards: Total Submissions, Average Rating, Academic Categories, Campus & Other.
   - Live sample cards and technical architecture breakdown.
   - Quick action buttons to submit or browse feedback.

2. **Feedback Submission Form (`/Feedback/Create`):**
   - Full Name (Required, 3–100 characters)
   - Email Address (Required, valid email format)
   - Phone Number (Required, regex 10 numeric digits)
   - Category Dropdown (Course, Faculty, Infrastructure, Other)
   - Star Rating Component (Interactive 1–5 stars)
   - Subject (Required, 3–150 characters)
   - Feedback Message (Required, minimum 10 characters with live character counter)
   - Submission Date (Automatically stamped upon POST)

3. **Validation & Error Handling:**
   - Client-side real-time validation via jQuery Unobtrusive Validation.
   - Server-side validation via `ModelState.IsValid` in `FeedbackController`.
   - Meaningful error messages displayed below each field and in `ValidationSummary`.

4. **Feedback Browsing & Management (`/Feedback/List`):**
   - Responsive cards rendered using `_FeedbackCard.cshtml`.
   - Category pills for instant filtering (All, Course, Faculty, Infrastructure, Other).
   - Rating visual with gold stars and score badges.
   - Secure POST deletion with confirmation prompts.
   - Dedicated Empty State UI when no feedback exists.

---

### 4. How to Run the Application

#### Option A: Quick Script (Recommended)
Double-click `run.bat` or run in terminal:
```bash
.\run.bat
```
This automatically launches your browser at `http://localhost:5070`.

#### Option B: Terminal Command
```bash
cd Experiment-07
dotnet run --urls "http://localhost:5070"
```
Open [http://localhost:5070](http://localhost:5070) in your browser.
