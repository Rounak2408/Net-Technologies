<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Students.aspx.cs" Inherits="Experiment_09.Students" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Student Records - ASP.NET Data Controls</title>

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
                    <a href="Students.aspx" class="sidebar-link active">
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
            </aside>

            <!-- Main Content Area -->
            <main class="app-main p-4 p-lg-5">
                <!-- Page Header Bar -->
                <div class="d-flex flex-wrap align-items-center justify-content-between mb-4 gap-3">
                    <div>
                        <nav aria-label="breadcrumb">
                            <ol class="breadcrumb mb-1 small">
                                <li class="breadcrumb-item"><a href="Default.aspx" class="text-navy">Dashboard</a></li>
                                <li class="breadcrumb-item active" aria-current="page">Students</li>
                            </ol>
                        </nav>
                        <h2 class="fw-bold text-navy mb-0">Student Placement Management</h2>
                        <p class="text-muted small mb-0">Powered by ASP.NET GridView, DetailsView & FormView Data Controls</p>
                    </div>
                    <div>
                        <a href="#registrationSection" class="btn btn-accent rounded-pill px-4 py-2 shadow-sm">
                            <i class="bi bi-person-plus-fill me-1"></i> Register New Student
                        </a>
                    </div>
                </div>

                <!-- Flash Alert Messages -->
                <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-success alert-dismissible fade show border-0 shadow-sm rounded-3 mb-4">
                    <asp:Literal ID="litAlertMsg" runat="server"></asp:Literal>
                    <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </asp:Panel>

                <!-- Filter & Search Toolbar -->
                <div class="card border-0 shadow-sm rounded-4 bg-white p-3 mb-4">
                    <div class="row g-2 align-items-center">
                        <div class="col-lg-4 col-md-6">
                            <div class="input-group">
                                <span class="input-group-text bg-white border-end-0 text-muted"><i class="bi bi-search"></i></span>
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control border-start-0 ps-0" placeholder="Search by student name or email..."></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-3 col-md-3">
                            <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-select">
                                <asp:ListItem Text="-- All Departments --" Value=""></asp:ListItem>
                                <asp:ListItem Text="Computer Science" Value="Computer Science"></asp:ListItem>
                                <asp:ListItem Text="Information Technology" Value="Information Technology"></asp:ListItem>
                                <asp:ListItem Text="Electronics & Communication" Value="Electronics & Communication"></asp:ListItem>
                                <asp:ListItem Text="Data Science" Value="Data Science"></asp:ListItem>
                                <asp:ListItem Text="Mechanical Engineering" Value="Mechanical Engineering"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-3 col-md-3">
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select">
                                <asp:ListItem Text="-- All Placement Statuses --" Value=""></asp:ListItem>
                                <asp:ListItem Text="Placed" Value="Placed"></asp:ListItem>
                                <asp:ListItem Text="Not Placed" Value="Not Placed"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-2 col-md-12 d-flex gap-2">
                            <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="btn btn-navy rounded-pill px-4 flex-grow-1" OnClick="btnFilter_Click" />
                            <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="btn btn-light rounded-pill px-3" OnClick="btnReset_Click" />
                        </div>
                    </div>
                </div>

                <!-- Section A: GridView Data Control -->
                <div class="card border-0 shadow-sm rounded-4 bg-white mb-5 overflow-hidden">
                    <div class="card-header bg-white border-bottom p-4">
                        <div class="d-flex align-items-center justify-content-between">
                            <div>
                                <h5 class="fw-bold text-navy mb-1">
                                    <i class="bi bi-table text-primary me-2"></i>Students Master Registry (GridView)
                                </h5>
                                <span class="text-muted small">Features: AutoGenerateColumns=false, BoundField, CommandField, Paging & Sorting</span>
                            </div>
                            <asp:Label ID="lblRecordsCount" runat="server" CssClass="badge bg-navy-light text-cyan px-3 py-2 rounded-pill small"></asp:Label>
                        </div>
                    </div>

                    <div class="table-responsive">
                        <!-- ASP.NET GridView Server Control -->
                        <asp:GridView ID="gvStudents" runat="server" 
                            AutoGenerateColumns="False" 
                            AllowPaging="True" 
                            PageSize="6" 
                            AllowSorting="True" 
                            DataKeyNames="StudentId" 
                            OnPageIndexChanging="gvStudents_PageIndexChanging" 
                            OnSorting="gvStudents_Sorting" 
                            OnRowEditing="gvStudents_RowEditing" 
                            OnRowCancelingEdit="gvStudents_RowCancelingEdit" 
                            OnRowUpdating="gvStudents_RowUpdating" 
                            OnRowDeleting="gvStudents_RowDeleting" 
                            OnSelectedIndexChanged="gvStudents_SelectedIndexChanged" 
                            CssClass="table table-hover table-gridview mb-0" 
                            PagerStyle-CssClass="gridview-pager">
                            
                            <Columns>
                                <asp:BoundField DataField="StudentId" HeaderText="ID" ReadOnly="True" SortExpression="StudentId" />
                                
                                <asp:BoundField DataField="FullName" HeaderText="Full Name" SortExpression="FullName" />
                                
                                <asp:BoundField DataField="Email" HeaderText="Email Address" SortExpression="Email" />
                                
                                <asp:BoundField DataField="Department" HeaderText="Department" SortExpression="Department" />
                                
                                <asp:BoundField DataField="CGPA" HeaderText="CGPA" SortExpression="CGPA" />
                                
                                <asp:BoundField DataField="GraduationYear" HeaderText="Batch" SortExpression="GraduationYear" />

                                <asp:TemplateField HeaderText="Status" SortExpression="PlacementStatus">
                                    <ItemTemplate>
                                        <span class='badge <%# Eval("PlacementStatus").ToString() == "Placed" ? "badge-placed" : "badge-unplaced" %> rounded-pill px-3 py-1'>
                                            <%# Eval("PlacementStatus") %>
                                        </span>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlEditStatus" runat="server" SelectedValue='<%# Bind("PlacementStatus") %>' CssClass="form-select form-select-sm">
                                            <asp:ListItem Text="Placed" Value="Placed"></asp:ListItem>
                                            <asp:ListItem Text="Not Placed" Value="Not Placed"></asp:ListItem>
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Company / Package">
                                    <ItemTemplate>
                                        <%# Eval("PlacementStatus").ToString() == "Placed" && Eval("CompanyName") != DBNull.Value 
                                            ? "<div class='fw-semibold text-navy'>" + Eval("CompanyName") + "</div><div class='small text-success'>₹" + Convert.ToDecimal(Eval("Package")).ToString("N2") + " LPA</div>" 
                                            : "<span class='text-muted'>-</span>" %>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:CommandField ShowSelectButton="True" ShowEditButton="True" ShowDeleteButton="True" 
                                    SelectText="<i class='bi bi-eye text-info me-1' title='Select Profile'></i>" 
                                    EditText="<i class='bi bi-pencil text-primary me-1' title='Inline Edit'></i>" 
                                    DeleteText="<i class='bi bi-trash3 text-danger' title='Delete Record'></i>" 
                                    UpdateText="<span class='badge bg-success me-1'>Save</span>" 
                                    CancelText="<span class='badge bg-secondary'>Cancel</span>" 
                                    HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" />
                            </Columns>

                            <EmptyDataTemplate>
                                <div class="text-center py-5">
                                    <div class="empty-icon-circle bg-light text-muted d-flex align-items-center justify-content-center mx-auto mb-2">
                                        <i class="bi bi-inbox fs-2"></i>
                                    </div>
                                    <h5 class="fw-bold text-navy">No Student Records Found</h5>
                                    <p class="text-muted small">Try modifying your search or filter criteria.</p>
                                </div>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </div>

                <!-- Section B & C: DetailsView and FormView Side-by-Side -->
                <div class="row g-4" id="registrationSection">
                    <!-- DetailsView: Selected Student Profile -->
                    <div class="col-lg-5">
                        <div class="card border-0 shadow-sm rounded-4 bg-white h-100 overflow-hidden">
                            <div class="card-header bg-navy text-white p-3">
                                <div class="d-flex align-items-center justify-content-between">
                                    <h6 class="fw-bold mb-0">
                                        <i class="bi bi-card-text text-cyan me-2"></i>Student DetailsView Control
                                    </h6>
                                    <span class="badge bg-navy-light text-cyan small">Navigation Active</span>
                                </div>
                            </div>
                            <div class="card-body p-0">
                                <asp:DetailsView ID="dvStudent" runat="server" 
                                    AutoGenerateRows="False" 
                                    AllowPaging="True" 
                                    DataKeyNames="StudentId" 
                                    OnPageIndexChanging="dvStudent_PageIndexChanging" 
                                    CssClass="table table-detailsview mb-0" 
                                    PagerStyle-CssClass="p-3 bg-light text-center border-top">
                                    
                                    <Fields>
                                        <asp:BoundField DataField="StudentId" HeaderText="Student ID" />
                                        <asp:BoundField DataField="FullName" HeaderText="Full Name" />
                                        <asp:BoundField DataField="Email" HeaderText="Email Address" />
                                        <asp:BoundField DataField="Department" HeaderText="Department" />
                                        <asp:BoundField DataField="CGPA" HeaderText="CGPA Score" />
                                        <asp:BoundField DataField="GraduationYear" HeaderText="Passing Year" />
                                        <asp:BoundField DataField="PlacementStatus" HeaderText="Status" />
                                        <asp:BoundField DataField="CompanyName" HeaderText="Selected Company" NullDisplayText="Not Applicable" />
                                        <asp:BoundField DataField="Package" HeaderText="Package (LPA)" DataFormatString="₹{0:N2} LPA" NullDisplayText="Not Applicable" />
                                    </Fields>

                                    <EmptyDataTemplate>
                                        <div class="p-4 text-center text-muted">
                                            <i class="bi bi-info-circle fs-3 d-block mb-2"></i>
                                            Select a student in the GridView above to view complete profile in this DetailsView.
                                        </div>
                                    </EmptyDataTemplate>
                                </asp:DetailsView>
                            </div>
                        </div>
                    </div>

                    <!-- FormView: Student Registration Form -->
                    <div class="col-lg-7">
                        <div class="card border-0 shadow-sm rounded-4 bg-white h-100 overflow-hidden">
                            <div class="card-header bg-navy text-white p-3">
                                <h6 class="fw-bold mb-0">
                                    <i class="bi bi-ui-checks text-cyan me-2"></i>Student FormView Control (Registration Form)
                                </h6>
                            </div>
                            <div class="card-body p-4">
                                <!-- Validation Summary -->
                                <asp:ValidationSummary ID="vsForm" runat="server" ValidationGroup="StudentInsertGroup" 
                                    CssClass="alert alert-danger border-0 shadow-sm small mb-3" HeaderText="Please correct the following fields:" />

                                <!-- ASP.NET FormView Server Control -->
                                <asp:FormView ID="fvStudent" runat="server" DefaultMode="Insert" DataKeyNames="StudentId" OnItemInserting="fvStudent_ItemInserting" CssClass="w-100">
                                    <InsertItemTemplate>
                                        <div class="row g-3">
                                            <!-- Full Name -->
                                            <div class="col-md-6">
                                                <label class="form-label small fw-semibold">Full Name <span class="text-danger">*</span></label>
                                                <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" placeholder="e.g. Aarav Sharma"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtFullName" 
                                                    ErrorMessage="Full Name is required." ValidationGroup="StudentInsertGroup" Display="Dynamic" CssClass="text-danger small mt-1"></asp:RequiredFieldValidator>
                                            </div>

                                            <!-- Email -->
                                            <div class="col-md-6">
                                                <label class="form-label small fw-semibold">Email Address <span class="text-danger">*</span></label>
                                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="student@college.edu"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" 
                                                    ErrorMessage="Email is required." ValidationGroup="StudentInsertGroup" Display="Dynamic" CssClass="text-danger small mt-1"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" 
                                                    ErrorMessage="Enter a valid email address." ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                                    ValidationGroup="StudentInsertGroup" Display="Dynamic" CssClass="text-danger small mt-1"></asp:RegularExpressionValidator>
                                            </div>

                                            <!-- Department -->
                                            <div class="col-md-6">
                                                <label class="form-label small fw-semibold">Department <span class="text-danger">*</span></label>
                                                <asp:DropDownList ID="ddlDept" runat="server" CssClass="form-select">
                                                    <asp:ListItem Text="Computer Science" Value="Computer Science"></asp:ListItem>
                                                    <asp:ListItem Text="Information Technology" Value="Information Technology"></asp:ListItem>
                                                    <asp:ListItem Text="Electronics & Communication" Value="Electronics & Communication"></asp:ListItem>
                                                    <asp:ListItem Text="Data Science" Value="Data Science"></asp:ListItem>
                                                    <asp:ListItem Text="Mechanical Engineering" Value="Mechanical Engineering"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>

                                            <!-- CGPA -->
                                            <div class="col-md-3">
                                                <label class="form-label small fw-semibold">CGPA <span class="text-danger">*</span></label>
                                                <asp:TextBox ID="txtCGPA" runat="server" CssClass="form-control" placeholder="0.00"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvCGPA" runat="server" ControlToValidate="txtCGPA" 
                                                    ErrorMessage="CGPA is required." ValidationGroup="StudentInsertGroup" Display="Dynamic" CssClass="text-danger small mt-1"></asp:RequiredFieldValidator>
                                                <asp:RangeValidator ID="rvCGPA" runat="server" ControlToValidate="txtCGPA" Type="Double" 
                                                    MinimumValue="0.00" MaximumValue="10.00" ErrorMessage="CGPA must be between 0.00 and 10.00" 
                                                    ValidationGroup="StudentInsertGroup" Display="Dynamic" CssClass="text-danger small mt-1"></asp:RangeValidator>
                                            </div>

                                            <!-- Graduation Year -->
                                            <div class="col-md-3">
                                                <label class="form-label small fw-semibold">Batch Year <span class="text-danger">*</span></label>
                                                <asp:TextBox ID="txtYear" runat="server" CssClass="form-control" Text="2026"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvYear" runat="server" ControlToValidate="txtYear" 
                                                    ErrorMessage="Graduation Year is required." ValidationGroup="StudentInsertGroup" Display="Dynamic" CssClass="text-danger small mt-1"></asp:RequiredFieldValidator>
                                            </div>

                                            <!-- Placement Status -->
                                            <div class="col-md-6">
                                                <label class="form-label small fw-semibold">Placement Status <span class="text-danger">*</span></label>
                                                <asp:DropDownList ID="ddlInsertStatus" runat="server" CssClass="form-select">
                                                    <asp:ListItem Text="Placed" Value="Placed"></asp:ListItem>
                                                    <asp:ListItem Text="Not Placed" Value="Not Placed"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>

                                            <!-- Company Selection -->
                                            <div class="col-md-6">
                                                <label class="form-label small fw-semibold">Assigned Company (if Placed)</label>
                                                <asp:DropDownList ID="ddlInsertCompany" runat="server" CssClass="form-select">
                                                </asp:DropDownList>
                                            </div>

                                            <!-- Submit LinkButton inside FormView -->
                                            <div class="col-12 pt-3 border-top d-flex gap-2">
                                                <asp:LinkButton ID="btnInsert" runat="server" CommandName="Insert" ValidationGroup="StudentInsertGroup" 
                                                    CssClass="btn btn-navy rounded-pill px-4 py-2 shadow-sm">
                                                    <i class="bi bi-save me-1"></i> Register Student
                                                </asp:LinkButton>
                                                <button type="reset" class="btn btn-outline-secondary rounded-pill px-4 py-2">
                                                    Reset Form
                                                </button>
                                            </div>
                                        </div>
                                    </InsertItemTemplate>
                                </asp:FormView>
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
