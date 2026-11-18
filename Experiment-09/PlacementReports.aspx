<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlacementReports.aspx.cs" Inherits="Experiment_09.PlacementReports" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Placement Reports & Analytics - Exercise 9</title>

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
                    <a href="Default.aspx" class="sidebar-link">
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
                    <a href="PlacementReports.aspx" class="sidebar-link active">
                        <i class="bi bi-bar-chart-line-fill"></i> Placement Reports
                    </a>
                </nav>
            </aside>

            <!-- Main Content Area -->
            <main class="app-main p-4 p-lg-5">
                <!-- Top Header Bar -->
                <div class="d-flex flex-wrap align-items-center justify-content-between mb-4 gap-3">
                    <div>
                        <nav aria-label="breadcrumb">
                            <ol class="breadcrumb mb-1 small">
                                <li class="breadcrumb-item"><a href="Default.aspx" class="text-navy">Dashboard</a></li>
                                <li class="breadcrumb-item active" aria-current="page">Placement Reports</li>
                            </ol>
                        </nav>
                        <h2 class="fw-bold text-navy mb-0">Placement Analytics & Institutional Reports</h2>
                        <p class="text-muted small mb-0">Detailed breakdown by Department, CTC Tiers & Hiring Distribution</p>
                    </div>
                    <div>
                        <button type="button" class="btn btn-outline-navy rounded-pill px-4 py-2" onclick="window.print();">
                            <i class="bi bi-printer me-1"></i> Print / Export Report
                        </button>
                    </div>
                </div>

                <!-- Department-Wise Placement Rate -->
                <div class="card border-0 shadow-sm rounded-4 bg-white mb-4 overflow-hidden">
                    <div class="card-header bg-navy text-white p-4">
                        <h5 class="fw-bold mb-0">
                            <i class="bi bi-pie-chart-fill text-cyan me-2"></i>Department-Wise Placement Statistics
                        </h5>
                    </div>
                    <div class="table-responsive">
                        <asp:GridView ID="gvDeptReport" runat="server" AutoGenerateColumns="False" CssClass="table table-hover align-middle mb-0 custom-table">
                            <Columns>
                                <asp:BoundField DataField="Department" HeaderText="Academic Department" ItemStyle-CssClass="fw-bold text-navy ps-4" HeaderStyle-CssClass="ps-4" />
                                <asp:BoundField DataField="TotalStudents" HeaderText="Total Students" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" />
                                <asp:BoundField DataField="PlacedCount" HeaderText="Placed" ItemStyle-CssClass="text-center text-success fw-bold" HeaderStyle-CssClass="text-center" />
                                <asp:BoundField DataField="UnplacedCount" HeaderText="Not Placed" ItemStyle-CssClass="text-center text-warning-emphasis" HeaderStyle-CssClass="text-center" />
                                <asp:TemplateField HeaderText="Placement %" HeaderStyle-CssClass="text-end pe-4" ItemStyle-CssClass="text-end pe-4">
                                    <ItemTemplate>
                                        <div class="d-flex align-items-center justify-content-end gap-2">
                                            <span class="fw-bold text-navy"><%# Eval("PlacementRate") %>%</span>
                                            <div class="progress" style="width: 80px; height: 6px;">
                                                <div class="progress-bar bg-success" role="progressbar" style='width: <%# Eval("PlacementRate") %>%;'></div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>

                <!-- Two-Column Grid: CTC Tier Analysis & Corporate Hiring Summary -->
                <div class="row g-4 mb-4">
                    <!-- CTC Tier Distribution -->
                    <div class="col-lg-6">
                        <div class="card border-0 shadow-sm rounded-4 bg-white h-100 overflow-hidden">
                            <div class="card-header bg-white border-bottom p-3">
                                <h6 class="fw-bold text-navy mb-0">
                                    <i class="bi bi-trophy text-warning me-2"></i>Package Tier Breakdown
                                </h6>
                            </div>
                            <div class="card-body p-4">
                                <ul class="list-group list-group-flush">
                                    <li class="list-group-item d-flex justify-content-between align-items-center py-3 border-0">
                                        <div>
                                            <strong class="text-navy">Super Dream / Tier 1 (> ₹20 LPA)</strong>
                                            <div class="text-muted small">Google, Microsoft, Amazon AWS, Cisco</div>
                                        </div>
                                        <span class="badge bg-success-subtle text-success fs-6 rounded-pill px-3 py-1">
                                            <asp:Label ID="lblTier1Count" runat="server" Text="0"></asp:Label> Students
                                        </span>
                                    </li>
                                    <li class="list-group-item d-flex justify-content-between align-items-center py-3 border-0">
                                        <div>
                                            <strong class="text-navy">Dream / Tier 2 (₹10 – ₹20 LPA)</strong>
                                            <div class="text-muted small">Deloitte, Oracle, Directi</div>
                                        </div>
                                        <span class="badge bg-info-subtle text-info fs-6 rounded-pill px-3 py-1">
                                            <asp:Label ID="lblTier2Count" runat="server" Text="0"></asp:Label> Students
                                        </span>
                                    </li>
                                    <li class="list-group-item d-flex justify-content-between align-items-center py-3 border-0">
                                        <div>
                                            <strong class="text-navy">Regular / Tier 3 (&lt; ₹10 LPA)</strong>
                                            <div class="text-muted small">Infosys, TCS, Wipro, Cognizant</div>
                                        </div>
                                        <span class="badge bg-secondary-subtle text-secondary fs-6 rounded-pill px-3 py-1">
                                            <asp:Label ID="lblTier3Count" runat="server" Text="0"></asp:Label> Students
                                        </span>
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </div>

                    <!-- Company Hiring Count -->
                    <div class="col-lg-6">
                        <div class="card border-0 shadow-sm rounded-4 bg-white h-100 overflow-hidden">
                            <div class="card-header bg-white border-bottom p-3">
                                <h6 class="fw-bold text-navy mb-0">
                                    <i class="bi bi-buildings text-primary me-2"></i>Company Recruitment Volume
                                </h6>
                            </div>
                            <div class="table-responsive">
                                <asp:GridView ID="gvCompanyHiring" runat="server" AutoGenerateColumns="False" CssClass="table table-hover align-middle mb-0">
                                    <Columns>
                                        <asp:BoundField DataField="CompanyName" HeaderText="Company" ItemStyle-CssClass="fw-bold text-navy ps-3" HeaderStyle-CssClass="ps-3" />
                                        <asp:BoundField DataField="Package" HeaderText="Package (LPA)" DataFormatString="₹{0:N2} LPA" ItemStyle-CssClass="text-success fw-semibold" />
                                        <asp:BoundField DataField="HiredCount" HeaderText="Recruited" ItemStyle-CssClass="text-center fw-bold" HeaderStyle-CssClass="text-center" />
                                    </Columns>
                                </asp:GridView>
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
