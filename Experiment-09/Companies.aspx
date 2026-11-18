<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Companies.aspx.cs" Inherits="Experiment_09.Companies" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Recruiting Companies - ASP.NET Repeater Control</title>

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
                    <a href="Companies.aspx" class="sidebar-link active">
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
            </aside>

            <!-- Main Content Area -->
            <main class="app-main p-4 p-lg-5">
                <!-- Top Header Bar -->
                <div class="d-flex flex-wrap align-items-center justify-content-between mb-4 gap-3">
                    <div>
                        <nav aria-label="breadcrumb">
                            <ol class="breadcrumb mb-1 small">
                                <li class="breadcrumb-item"><a href="Default.aspx" class="text-navy">Dashboard</a></li>
                                <li class="breadcrumb-item active" aria-current="page">Companies</li>
                            </ol>
                        </nav>
                        <h2 class="fw-bold text-navy mb-0">Recruiting Corporate Partners</h2>
                        <p class="text-muted small mb-0">Showcasing placement drives & corporate cards via ASP.NET <strong>&lt;asp:Repeater&gt;</strong></p>
                    </div>
                    <div>
                        <button type="button" class="btn btn-navy rounded-pill px-4 py-2 shadow-sm" data-bs-toggle="modal" data-bs-target="#addCompanyModal">
                            <i class="bi bi-plus-circle-fill me-1"></i> Add Partner Company
                        </button>
                    </div>
                </div>

                <!-- Alert Messages -->
                <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-success alert-dismissible fade show border-0 shadow-sm rounded-3 mb-4">
                    <asp:Literal ID="litAlertMsg" runat="server"></asp:Literal>
                    <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </asp:Panel>

                <!-- ASP.NET Repeater Control for Companies -->
                <div class="row g-4">
                    <asp:Repeater ID="rptCompanies" runat="server">
                        <ItemTemplate>
                            <div class="col-lg-4 col-md-6">
                                <div class="company-repeater-card h-100">
                                    <div class="card-top-accent accent-default"></div>
                                    <div class="p-4 d-flex flex-column h-100">
                                        <div class="d-flex align-items-start justify-content-between mb-3">
                                            <div>
                                                <h5 class="fw-bold text-navy mb-1"><%# Eval("CompanyName") %></h5>
                                                <span class="text-muted small">
                                                    <i class="bi bi-geo-alt me-1 text-danger"></i><%# Eval("Location") %>
                                                </span>
                                            </div>
                                            <span class="badge bg-navy-light text-cyan rounded-pill px-3 py-1 small fw-bold">
                                                ID #<%# Eval("CompanyId") %>
                                            </span>
                                        </div>

                                        <div class="bg-light p-3 rounded-3 mb-3">
                                            <div class="small text-muted mb-1">Offered Role:</div>
                                            <div class="fw-bold text-dark fs-6 mb-2">
                                                <i class="bi bi-briefcase-fill text-navy me-1"></i><%# Eval("JobRole") %>
                                            </div>

                                            <div class="d-flex justify-content-between align-items-center pt-2 border-top">
                                                <span class="small text-muted">CTC Package:</span>
                                                <span class="fs-5 fw-bold text-success">
                                                    ₹<%# Convert.ToDecimal(Eval("Package")).ToString("N2") %> LPA
                                                </span>
                                            </div>
                                        </div>

                                        <div class="d-flex justify-content-between align-items-center text-muted small mt-auto pt-2 border-top">
                                            <span>
                                                <i class="bi bi-calendar3 me-1"></i>Drive: <%# Convert.ToDateTime(Eval("DriveDate")).ToString("dd MMM yyyy") %>
                                            </span>
                                            <span class="badge bg-success-subtle text-success rounded-pill px-2 py-1">
                                                <i class="bi bi-check-circle me-1"></i>Active Drive
                                            </span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>

                <!-- Add Company Modal -->
                <div class="modal fade" id="addCompanyModal" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">
                    <div class="modal-dialog modal-dialog-centered">
                        <div class="modal-content rounded-4 border-0 shadow-lg">
                            <div class="modal-header bg-navy text-white p-4">
                                <h5 class="modal-title fw-bold" id="modalLabel">
                                    <i class="bi bi-building-add me-2 text-cyan"></i>Add Partner Company
                                </h5>
                                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                            </div>
                            <div class="modal-body p-4">
                                <div class="mb-3">
                                    <label class="form-label small fw-semibold">Company Name <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtCompName" runat="server" CssClass="form-control" placeholder="e.g. Oracle India"></asp:TextBox>
                                </div>
                                <div class="mb-3">
                                    <label class="form-label small fw-semibold">Location / Office <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtLocation" runat="server" CssClass="form-control" placeholder="e.g. Bangalore"></asp:TextBox>
                                </div>
                                <div class="mb-3">
                                    <label class="form-label small fw-semibold">Job Designation / Role <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtRole" runat="server" CssClass="form-control" placeholder="e.g. Applications Engineer"></asp:TextBox>
                                </div>
                                <div class="mb-3">
                                    <label class="form-label small fw-semibold">Offered Package (₹ in LPA) <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtPackage" runat="server" CssClass="form-control" placeholder="e.g. 16.50"></asp:TextBox>
                                </div>
                                <div class="mb-3">
                                    <label class="form-label small fw-semibold">Drive Date</label>
                                    <asp:TextBox ID="txtDriveDate" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="modal-footer p-3 bg-light">
                                <button type="button" class="btn btn-outline-secondary rounded-pill px-4" data-bs-dismiss="modal">Cancel</button>
                                <asp:Button ID="btnAddCompany" runat="server" Text="Save Company" CssClass="btn btn-navy rounded-pill px-4" OnClick="btnAddCompany_Click" />
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
