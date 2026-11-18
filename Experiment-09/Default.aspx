<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Experiment_09.Default" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Placement Management Dashboard - Exercise 9</title>

    <!-- Google Fonts -->
    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
    <link href="https://fonts.googleapis.com/css2?family=Plus+Jakarta+Sans:wght@400;500;600;700;800&family=Inter:wght@400;500;600;700&display=swap" rel="stylesheet">

    <!-- Bootstrap 5 CSS -->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" />

    <!-- Bootstrap Icons CDN -->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />

    <!-- Custom Theme CSS -->
    <link rel="stylesheet" href="Content/style.css" />
</head>
<body class="bg-page">
    <form id="form1" runat="server">
        <div class="app-wrapper">
            <!-- Sidebar Navigation -->
            <aside class="app-sidebar">
                <div class="sidebar-header d-flex align-items-center">
                    <div class="brand-icon-box me-3">
                        <i class="bi bi-mortarboard-fill"></i>
                    </div>
                    <div>
                        <div class="brand-title">PlacementHub</div>
                        <div class="brand-subtitle">ASP.NET Data Controls</div>
                    </div>
                </div>

                <nav class="sidebar-nav">
                    <a href="Default.aspx" class="sidebar-link active">
                        <i class="bi bi-speedometer2"></i> Dashboard
                    </a>
                    <a href="Students.aspx" class="sidebar-link">
                        <i class="bi bi-people-fill"></i> Students (GridView)
                    </a>
                    <a href="Companies.aspx" class="sidebar-link">
                        <i class="bi bi-buildings-fill"></i> Companies (Repeater)
                    </a>
                    <a href="Students.aspx?status=Placed" class="sidebar-link">
                        <i class="bi bi-check-circle-fill text-success"></i> Placed Students
                    </a>
                    <a href="Students.aspx?status=Not+Placed" class="sidebar-link">
                        <i class="bi bi-hourglass-split text-warning"></i> Unplaced Students
                    </a>
                    <a href="PlacementReports.aspx" class="sidebar-link">
                        <i class="bi bi-bar-chart-line-fill"></i> Placement Reports
                    </a>
                </nav>

                <div class="mt-auto p-3 text-white-50 border-top border-secondary small">
                    <div class="d-flex align-items-center mb-1 text-white">
                        <i class="bi bi-database-check me-2 text-cyan"></i>
                        <span>SQL Server Active</span>
                    </div>
                    <span class="small opacity-75">ADO.NET & LocalDB</span>
                </div>
            </aside>

            <!-- Main Content Area -->
            <main class="app-main p-4 p-lg-5">
                <!-- Top Header Bar -->
                <div class="d-flex flex-wrap align-items-center justify-content-between mb-4 gap-3">
                    <div>
                        <div class="badge bg-navy-light text-cyan px-3 py-1 rounded-pill small fw-semibold mb-2">
                            <i class="bi bi-award-fill me-1"></i> Net Technologies Practical – Exercise 09
                        </div>
                        <h2 class="fw-bold text-navy mb-0">Placement Management Dashboard</h2>
                        <p class="text-muted small mb-0">Overview of institutional placement statistics, drives & placed students</p>
                    </div>
                    <div class="d-flex gap-2">
                        <a href="Students.aspx" class="btn btn-navy rounded-pill px-4 py-2 shadow-sm">
                            <i class="bi bi-people me-1"></i> Manage Students
                        </a>
                        <a href="Companies.aspx" class="btn btn-accent rounded-pill px-4 py-2 shadow-sm">
                            <i class="bi bi-building-add me-1"></i> View Companies
                        </a>
                    </div>
                </div>

                <!-- Statistics Grid (7 Dashboard Cards) -->
                <div class="row g-3 mb-4">
                    <!-- Total Students -->
                    <div class="col-xl-3 col-md-6">
                        <div class="card stat-card border-0 shadow-sm p-4 h-100">
                            <div class="d-flex align-items-center justify-content-between">
                                <div>
                                    <span class="text-muted small fw-semibold text-uppercase tracking-wider">Total Students</span>
                                    <h3 class="display-6 fw-bold text-navy mt-1 mb-0">
                                        <asp:Label ID="lblTotalStudents" runat="server" Text="0"></asp:Label>
                                    </h3>
                                    <span class="badge bg-primary-subtle text-primary mt-2 small">Registered Batch</span>
                                </div>
                                <div class="stat-icon-box bg-primary-subtle text-primary">
                                    <i class="bi bi-people-fill"></i>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Total Companies -->
                    <div class="col-xl-3 col-md-6">
                        <div class="card stat-card border-0 shadow-sm p-4 h-100">
                            <div class="d-flex align-items-center justify-content-between">
                                <div>
                                    <span class="text-muted small fw-semibold text-uppercase tracking-wider">Recruiting Companies</span>
                                    <h3 class="display-6 fw-bold text-navy mt-1 mb-0">
                                        <asp:Label ID="lblTotalCompanies" runat="server" Text="0"></asp:Label>
                                    </h3>
                                    <span class="badge bg-info-subtle text-info mt-2 small">Campus Partners</span>
                                </div>
                                <div class="stat-icon-box bg-info-subtle text-info">
                                    <i class="bi bi-buildings-fill"></i>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Students Placed -->
                    <div class="col-xl-3 col-md-6">
                        <div class="card stat-card border-0 shadow-sm p-4 h-100">
                            <div class="d-flex align-items-center justify-content-between">
                                <div>
                                    <span class="text-muted small fw-semibold text-uppercase tracking-wider">Students Placed</span>
                                    <h3 class="display-6 fw-bold text-success mt-1 mb-0">
                                        <asp:Label ID="lblStudentsPlaced" runat="server" Text="0"></asp:Label>
                                    </h3>
                                    <span class="badge bg-success-subtle text-success mt-2 small">
                                        <i class="bi bi-check-circle me-1"></i>Offers Secured
                                    </span>
                                </div>
                                <div class="stat-icon-box bg-success-subtle text-success">
                                    <i class="bi bi-patch-check-fill"></i>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Students Not Placed -->
                    <div class="col-xl-3 col-md-6">
                        <div class="card stat-card border-0 shadow-sm p-4 h-100">
                            <div class="d-flex align-items-center justify-content-between">
                                <div>
                                    <span class="text-muted small fw-semibold text-uppercase tracking-wider">Not Placed</span>
                                    <h3 class="display-6 fw-bold text-warning-emphasis mt-1 mb-0">
                                        <asp:Label ID="lblStudentsNotPlaced" runat="server" Text="0"></asp:Label>
                                    </h3>
                                    <span class="badge bg-warning-subtle text-warning-emphasis mt-2 small">In Pipeline</span>
                                </div>
                                <div class="stat-icon-box bg-warning-subtle text-warning">
                                    <i class="bi bi-hourglass-split"></i>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Placement Percentage -->
                    <div class="col-xl-4 col-md-6">
                        <div class="card stat-card border-0 shadow-sm p-4 h-100">
                            <div class="d-flex align-items-center justify-content-between">
                                <div>
                                    <span class="text-muted small fw-semibold text-uppercase tracking-wider">Placement Rate</span>
                                    <h3 class="display-6 fw-bold text-navy mt-1 mb-0">
                                        <asp:Label ID="lblPlacementPercentage" runat="server" Text="0%"></asp:Label>
                                    </h3>
                                    <div class="progress mt-2" style="height: 6px; width: 140px;">
                                        <asp:Literal ID="litProgressBar" runat="server"></asp:Literal>
                                    </div>
                                </div>
                                <div class="stat-icon-box bg-navy-light text-navy">
                                    <i class="bi bi-pie-chart-fill"></i>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Highest Package -->
                    <div class="col-xl-4 col-md-6">
                        <div class="card stat-card border-0 shadow-sm p-4 h-100">
                            <div class="d-flex align-items-center justify-content-between">
                                <div>
                                    <span class="text-muted small fw-semibold text-uppercase tracking-wider">Highest Package</span>
                                    <h3 class="display-6 fw-bold text-navy mt-1 mb-0">
                                        <asp:Label ID="lblHighestPackage" runat="server" Text="₹0.00 LPA"></asp:Label>
                                    </h3>
                                    <span class="badge bg-success-subtle text-success mt-2 small">
                                        <i class="bi bi-arrow-up-right me-1"></i>Top Offer
                                    </span>
                                </div>
                                <div class="stat-icon-box bg-success-subtle text-success">
                                    <i class="bi bi-trophy-fill"></i>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Average Package -->
                    <div class="col-xl-4 col-md-12">
                        <div class="card stat-card border-0 shadow-sm p-4 h-100">
                            <div class="d-flex align-items-center justify-content-between">
                                <div>
                                    <span class="text-muted small fw-semibold text-uppercase tracking-wider">Average Package</span>
                                    <h3 class="display-6 fw-bold text-navy mt-1 mb-0">
                                        <asp:Label ID="lblAveragePackage" runat="server" Text="₹0.00 LPA"></asp:Label>
                                    </h3>
                                    <span class="badge bg-info-subtle text-info mt-2 small">Mean Annual CTC</span>
                                </div>
                                <div class="stat-icon-box bg-info-subtle text-info">
                                    <i class="bi bi-calculator-fill"></i>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Section: Placed Students Card Layout using ASP.NET DataList -->
                <div class="card border-0 shadow-sm rounded-4 bg-white p-4 mb-4">
                    <div class="d-flex align-items-center justify-content-between mb-3 border-bottom pb-3">
                        <div>
                            <h4 class="fw-bold text-navy mb-1">
                                <i class="bi bi-stars text-warning me-2"></i>Placed Students Showcase
                            </h4>
                            <p class="text-muted small mb-0">Rendered via ASP.NET <strong>&lt;asp:DataList&gt;</strong> Server Control</p>
                        </div>
                        <a href="Students.aspx?status=Placed" class="btn btn-outline-navy rounded-pill px-3 py-1 small">
                            View All Placed <i class="bi bi-arrow-right ms-1"></i>
                        </a>
                    </div>

                    <!-- ASP.NET DataList Server Control -->
                    <asp:DataList ID="dlPlacedStudents" runat="server" RepeatColumns="3" RepeatDirection="Horizontal" Width="100%" CssClass="w-100">
                        <ItemTemplate>
                            <div class="p-2">
                                <div class="student-datalist-card h-100">
                                    <div class="card-top-accent accent-google"></div>
                                    <div class="p-4 d-flex flex-column h-100">
                                        <div class="d-flex align-items-start justify-content-between mb-3">
                                            <div class="d-flex align-items-center">
                                                <div class="avatar-circle me-3">
                                                    <%# Eval("FullName").ToString().Substring(0, 1) %>
                                                </div>
                                                <div>
                                                    <h6 class="fw-bold text-navy mb-0"><%# Eval("FullName") %></h6>
                                                    <span class="text-muted small"><%# Eval("Department") %></span>
                                                </div>
                                            </div>
                                            <span class="badge badge-placed rounded-pill px-3 py-1 small">
                                                <i class="bi bi-check-circle-fill me-1"></i> Placed
                                            </span>
                                        </div>

                                        <div class="bg-light p-3 rounded-3 mb-3">
                                            <div class="d-flex justify-content-between align-items-center mb-1">
                                                <span class="text-muted small">Company:</span>
                                                <span class="fw-bold text-navy"><i class="bi bi-building me-1"></i><%# Eval("CompanyName") %></span>
                                            </div>
                                            <div class="d-flex justify-content-between align-items-center">
                                                <span class="text-muted small">Package Offered:</span>
                                                <span class="fw-bold text-success fs-6">₹<%# Convert.ToDecimal(Eval("Package")).ToString("N2") %> LPA</span>
                                            </div>
                                        </div>

                                        <div class="d-flex justify-content-between text-muted small mt-auto pt-2 border-top">
                                            <span>CGPA: <strong><%# Eval("CGPA") %></strong></span>
                                            <span>Batch: <strong><%# Eval("GraduationYear") %></strong></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:DataList>
                </div>

                <!-- Technical Specification Card -->
                <div class="card border-0 shadow-sm rounded-4 bg-white p-4">
                    <h5 class="fw-bold text-navy mb-3">
                        <i class="bi bi-cpu-fill text-cyan me-2"></i>ASP.NET Data Controls Implemented
                    </h5>
                    <div class="row g-3 small text-secondary">
                        <div class="col-md-4">
                            <div class="p-3 bg-light rounded-3 h-100">
                                <div class="fw-bold text-dark mb-1"><i class="bi bi-table text-primary me-1"></i> &lt;asp:GridView&gt;</div>
                                <span>Used in <strong>Students.aspx</strong> for student records with paging, sorting, edit and delete command fields.</span>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="p-3 bg-light rounded-3 h-100">
                                <div class="fw-bold text-dark mb-1"><i class="bi bi-card-heading text-info me-1"></i> &lt;asp:DetailsView&gt; & &lt;asp:FormView&gt;</div>
                                <span>Used in <strong>Students.aspx</strong> for record navigation and student registration with validation controls.</span>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="p-3 bg-light rounded-3 h-100">
                                <div class="fw-bold text-dark mb-1"><i class="bi bi-grid-3x3-gap text-success me-1"></i> &lt;asp:Repeater&gt; & &lt;asp:DataList&gt;</div>
                                <span>Used in <strong>Companies.aspx</strong> for corporate cards and <strong>Default.aspx</strong> for placed student cards.</span>
                            </div>
                        </div>
                    </div>
                </div>
            </main>
        </div>
    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
    <script src="Scripts/site.js"></script>
</body>
</html>
