<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="DefaultPage" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>AcademiX - Academic Calendar & Leave Management System</title>
    <style>
        :root {
            --primary: #1d4ed8;
            --primary-hover: #1e40af;
            --bg-body: #f8fafc;
            --card-bg: #ffffff;
            --text-dark: #1e293b;
            --text-muted: #64748b;
            --border-color: #cbd5e1;
            
            --success-bg: #dcfce7;
            --success-text: #15803d;
            --warning-bg: #fef3c7;
            --warning-text: #b45309;
            --danger-bg: #fee2e2;
            --danger-text: #b91c1c;
            --info-bg: #e0f2fe;
            --info-text: #0369a1;

            --radius: 8px;
        }

        * {
            box-sizing: border-box;
            margin: 0;
            padding: 0;
        }

        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background-color: var(--bg-body);
            color: var(--text-dark);
            min-height: 100vh;
            padding: 20px 10px;
        }

        .wrapper {
            max-width: 1100px;
            margin: 0 auto;
        }

        /* Header */
        .app-header {
            background: #1e293b;
            color: #ffffff;
            padding: 20px 25px;
            border-radius: var(--radius);
            margin-bottom: 20px;
            display: flex;
            justify-content: space-between;
            align-items: center;
            flex-wrap: wrap;
            gap: 15px;
        }

        .app-title h1 {
            font-size: 1.6rem;
            font-weight: 700;
        }

        .app-title p {
            color: #94a3b8;
            font-size: 0.9rem;
            margin-top: 2px;
        }

        .badge-exp {
            background: #0284c7;
            color: #ffffff;
            padding: 6px 14px;
            border-radius: 4px;
            font-size: 0.85rem;
            font-weight: 600;
        }

        /* User Profile & Cookie Bar */
        .profile-bar {
            background: var(--card-bg);
            border: 1px solid var(--border-color);
            border-radius: var(--radius);
            padding: 18px 22px;
            margin-bottom: 20px;
            display: flex;
            flex-wrap: wrap;
            justify-content: space-between;
            align-items: center;
            gap: 15px;
        }

        .user-details h3 {
            font-size: 1.1rem;
            font-weight: 700;
            color: var(--text-dark);
        }

        .user-details p {
            font-size: 0.88rem;
            color: var(--text-muted);
        }

        .leave-balances {
            display: flex;
            gap: 10px;
            flex-wrap: wrap;
        }

        .bal-chip {
            background: #f1f5f9;
            border: 1px solid #cbd5e1;
            padding: 6px 12px;
            border-radius: 6px;
            text-align: center;
        }

        .bal-chip .bal-num {
            font-size: 1.1rem;
            font-weight: 700;
            color: var(--primary);
            display: block;
        }

        .bal-chip .bal-lbl {
            font-size: 0.75rem;
            text-transform: uppercase;
            font-weight: 700;
            color: var(--text-muted);
        }

        .cookie-status-box {
            background: #f8fafc;
            border-left: 4px solid var(--primary);
            padding: 8px 12px;
            border-radius: 4px;
            font-size: 0.82rem;
            color: var(--text-dark);
            line-height: 1.4;
        }

        /* Navigation Bar */
        .nav-bar {
            display: flex;
            gap: 6px;
            margin-bottom: 20px;
            border-bottom: 2px solid var(--border-color);
            padding-bottom: 2px;
            overflow-x: auto;
        }

        .nav-btn {
            background: transparent;
            border: 1px solid transparent;
            padding: 10px 18px;
            font-family: inherit;
            font-size: 0.95rem;
            font-weight: 600;
            color: var(--text-muted);
            cursor: pointer;
            border-radius: var(--radius) var(--radius) 0 0;
            transition: all 0.15s ease;
            white-space: nowrap;
        }

        .nav-btn:hover {
            color: var(--primary);
            background: #f1f5f9;
        }

        .nav-btn.active {
            color: var(--primary);
            background: #ffffff;
            border: 1px solid var(--border-color);
            border-bottom: 3px solid var(--primary);
        }

        /* Notice AdRotator */
        .ad-container {
            background: #eff6ff;
            border: 1px solid #bfdbfe;
            border-radius: var(--radius);
            padding: 12px 18px;
            margin-bottom: 20px;
            display: flex;
            align-items: center;
            gap: 15px;
        }

        .ad-container img {
            width: 40px;
            height: 40px;
            object-fit: contain;
        }

        .ad-text {
            font-size: 0.92rem;
            font-weight: 600;
            color: #1e40af;
        }

        /* Main Card Container */
        .main-card {
            background: var(--card-bg);
            border: 1px solid var(--border-color);
            border-radius: var(--radius);
            padding: 25px;
        }

        .view-title {
            font-size: 1.25rem;
            font-weight: 700;
            color: var(--text-dark);
            margin-bottom: 18px;
            padding-bottom: 8px;
            border-bottom: 1px solid var(--border-color);
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        /* Calendar Controls */
        .calendar-wrapper {
            display: grid;
            grid-template-columns: 2fr 1fr;
            gap: 20px;
        }

        @media (max-width: 850px) {
            .calendar-wrapper {
                grid-template-columns: 1fr;
            }
        }

        .rich-calendar {
            width: 100% !important;
            border-collapse: collapse !important;
            border: 1px solid #cbd5e1 !important;
            border-radius: 8px !important;
            overflow: hidden !important;
            background: #ffffff !important;
            font-family: inherit !important;
        }

        .rich-calendar td {
            height: 70px !important;
            vertical-align: top !important;
            padding: 4px !important;
            border: 1px solid #e2e8f0 !important;
        }

        .rich-calendar td:hover {
            background-color: #f1f5f9 !important;
        }

        .rich-calendar .cal-badge {
            display: block;
            font-size: 0.7rem;
            font-weight: 700;
            padding: 2px 4px;
            border-radius: 3px;
            margin-top: 3px;
            line-height: 1.2;
            word-break: break-word;
        }

        .badge-holiday { background: #fee2e2; color: #991b1b; }
        .badge-exam { background: #f3e8ff; color: #6b21a8; }
        .badge-event { background: #e0f2fe; color: #075985; }
        .badge-approved { background: #dcfce7; color: #166534; }
        .badge-pending { background: #fef3c7; color: #92400e; }

        .cal-day-holiday { background-color: #fff5f5 !important; }
        .cal-day-exam { background-color: #faf5ff !important; }
        .cal-day-event { background-color: #f0f9ff !important; }

        .calendar-legend {
            background: #f8fafc;
            border: 1px solid var(--border-color);
            padding: 15px;
            border-radius: var(--radius);
        }

        .legend-item {
            display: flex;
            align-items: center;
            gap: 8px;
            margin-bottom: 10px;
            font-size: 0.85rem;
            font-weight: 500;
        }

        .legend-color {
            width: 14px;
            height: 14px;
            border-radius: 3px;
        }

        /* Form Controls */
        .form-grid {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 15px;
        }

        @media (max-width: 650px) {
            .form-grid {
                grid-template-columns: 1fr;
            }
        }

        .form-group {
            margin-bottom: 15px;
        }

        .form-group.full-width {
            grid-column: 1 / -1;
        }

        .form-group label {
            display: block;
            font-weight: 600;
            font-size: 0.88rem;
            margin-bottom: 4px;
            color: var(--text-dark);
        }

        .form-control {
            width: 100%;
            padding: 9px 12px;
            font-family: inherit;
            font-size: 0.92rem;
            border: 1px solid #cbd5e1;
            border-radius: var(--radius);
            background-color: #ffffff;
            color: var(--text-dark);
        }

        .form-control:focus {
            outline: none;
            border-color: var(--primary);
        }

        .btn {
            padding: 9px 20px;
            font-family: inherit;
            font-size: 0.92rem;
            font-weight: 600;
            border: none;
            border-radius: var(--radius);
            cursor: pointer;
            display: inline-flex;
            align-items: center;
            gap: 6px;
        }

        .btn-primary { background: var(--primary); color: white; }
        .btn-primary:hover { background: var(--primary-hover); }

        .btn-success { background: #16a34a; color: white; }
        .btn-success:hover { background: #15803d; }

        .btn-danger { background: #dc2626; color: white; }
        .btn-danger:hover { background: #b91c1c; }

        .btn-secondary { background: #64748b; color: white; }
        .btn-secondary:hover { background: #475569; }

        /* Notification Alert Box */
        .alert-box {
            padding: 12px 16px;
            border-radius: var(--radius);
            margin-bottom: 15px;
            font-size: 0.92rem;
            font-weight: 600;
        }

        .alert-success { background: var(--success-bg); color: var(--success-text); border: 1px solid #bbf7d0; }
        .alert-warning { background: var(--warning-bg); color: var(--warning-text); border: 1px solid #fde68a; }
        .alert-danger { background: var(--danger-bg); color: var(--danger-text); border: 1px solid #fecaca; }
        .alert-info { background: var(--info-bg); color: var(--info-text); border: 1px solid #bae6fd; }

        .validation-summary {
            background: #fef2f2;
            border: 1px solid #fecaca;
            color: #991b1b;
            padding: 12px 16px;
            border-radius: var(--radius);
            margin-bottom: 15px;
            font-size: 0.88rem;
        }

        /* Metrics & Tables */
        .metrics-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(160px, 1fr));
            gap: 12px;
            margin-bottom: 20px;
        }

        .metric-card {
            background: #ffffff;
            border: 1px solid var(--border-color);
            padding: 14px;
            border-radius: var(--radius);
            text-align: center;
        }

        .metric-val {
            font-size: 1.6rem;
            font-weight: 700;
            color: var(--primary);
        }

        .metric-lbl {
            font-size: 0.8rem;
            font-weight: 600;
            color: var(--text-muted);
            text-transform: uppercase;
        }

        .data-table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 8px;
        }

        .data-table th {
            background: #f1f5f9;
            text-align: left;
            padding: 10px 12px;
            font-size: 0.85rem;
            font-weight: 700;
            color: var(--text-muted);
            border-bottom: 2px solid var(--border-color);
        }

        .data-table td {
            padding: 10px 12px;
            border-bottom: 1px solid var(--border-color);
            font-size: 0.9rem;
            vertical-align: middle;
        }

        .data-table tr:hover {
            background-color: #f8fafc;
        }

        .status-pill {
            display: inline-block;
            padding: 3px 8px;
            border-radius: 10px;
            font-size: 0.75rem;
            font-weight: 700;
            text-transform: uppercase;
        }

        .pill-approved { background: #dcfce7; color: #15803d; }
        .pill-pending { background: #fef3c7; color: #b45309; }
        .pill-rejected { background: #fee2e2; color: #b91c1c; }

        /* Inspector Box */
        .inspector-section {
            background: #f8fafc;
            border: 1px solid var(--border-color);
            border-radius: var(--radius);
            padding: 16px;
            margin-bottom: 20px;
        }

        .inspector-title {
            font-size: 1rem;
            font-weight: 700;
            color: var(--primary);
            margin-bottom: 10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
        <div class="wrapper">
            <!-- App Header -->
            <div class="app-header">
                <div class="app-title">
                    <h1>AcademiX Portal</h1>
                    <p>Academic Calendar &amp; Leave Management System</p>
                </div>
                <div class="badge-exp">
                    Experiment - 05 (.NET Technologies)
                </div>
            </div>

            <!-- User Profile & Cookie Status Bar -->
            <div class="profile-bar">
                <div class="user-details">
                    <h3><asp:Label ID="lblUserName" runat="server" Text="Rounak Keshri"></asp:Label></h3>
                    <p><asp:Label ID="lblUserDept" runat="server" Text="Computer Science &amp; Engineering"></asp:Label></p>
                </div>

                <div class="leave-balances">
                    <div class="bal-chip">
                        <span class="bal-num"><asp:Label ID="lblCasualLeave" runat="server">12</asp:Label></span>
                        <span class="bal-lbl">Casual (CL)</span>
                    </div>
                    <div class="bal-chip">
                        <span class="bal-num"><asp:Label ID="lblMedicalLeave" runat="server">10</asp:Label></span>
                        <span class="bal-lbl">Medical (ML)</span>
                    </div>
                    <div class="bal-chip">
                        <span class="bal-num"><asp:Label ID="lblAcademicLeave" runat="server">8</asp:Label></span>
                        <span class="bal-lbl">Duty (AL)</span>
                    </div>
                </div>

                <div style="display:flex; flex-direction:column; gap:4px;">
                    <div class="form-group" style="margin:0;">
                        <label style="font-size:0.8rem; margin:0;">Role View (Session):</label>
                        <asp:DropDownList ID="ddlUserRole" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlUserRole_SelectedIndexChanged" CssClass="form-control" style="padding:4px 8px; font-size:0.85rem;">
                            <asp:ListItem Value="Student">Student</asp:ListItem>
                            <asp:ListItem Value="Faculty">Faculty Member</asp:ListItem>
                            <asp:ListItem Value="HOD Admin">HOD / Admin</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="cookie-status-box">
                    <strong>Active Cookies:</strong><br />
                    Last Login: <asp:Label ID="lblLastVisit" runat="server"></asp:Label><br />
                    Saved User: <asp:Label ID="lblRememberedUser" runat="server"></asp:Label><br />
                    Theme: <asp:Label ID="lblCurrentTheme" runat="server"></asp:Label>
                </div>
            </div>

            <!-- Global Alert Box -->
            <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert-box">
                <asp:Label ID="lblAlertText" runat="server"></asp:Label>
            </asp:Panel>

            <!-- Navigation Bar -->
            <div class="nav-bar">
                <asp:Button ID="btnTabCalendar" runat="server" Text="Academic Calendar" OnClick="btnTabCalendar_Click" CausesValidation="false" />
                <asp:Button ID="btnTabApply" runat="server" Text="Apply for Leave" OnClick="btnTabApply_Click" CausesValidation="false" />
                <asp:Button ID="btnTabStatus" runat="server" Text="My Leave Status" OnClick="btnTabStatus_Click" CausesValidation="false" />
                <asp:Button ID="btnTabAdmin" runat="server" Text="Admin Approval Portal" OnClick="btnTabAdmin_Click" CausesValidation="false" />
                <asp:Button ID="btnTabInspector" runat="server" Text="Session &amp; Cookies Inspector" OnClick="btnTabInspector_Click" CausesValidation="false" />
            </div>

            <!-- Main Content Area with MultiView -->
            <div class="main-card">
                <asp:MultiView ID="mvMain" runat="server" ActiveViewIndex="0">
                    
                    <!-- VIEW 0: ACADEMIC CALENDAR -->
                    <asp:View ID="ViewCalendar" runat="server">
                        <div class="view-title">
                            <span>Academic Calendar &amp; Important Dates</span>
                            <span style="font-size:0.85rem; font-weight:normal; color:var(--text-muted);">Rich Control: &lt;asp:Calendar&gt; &amp; &lt;asp:AdRotator&gt;</span>
                        </div>

                        <!-- Notice Rotator Banner -->
                        <div class="ad-container">
                            <asp:AdRotator ID="AdRotatorNotices" runat="server" AdvertisementFile="advertisements.xml" KeywordFilter="" Target="_blank" />
                        </div>

                        <div class="calendar-wrapper">
                            <div>
                                <!-- ASP.NET Rich Calendar Control -->
                                <asp:Calendar ID="CalendarAcademic" runat="server" 
                                    OnDayRender="CalendarAcademic_DayRender" 
                                    OnSelectionChanged="CalendarAcademic_SelectionChanged"
                                    CssClass="rich-calendar"
                                    NextPrevFormat="FullMonth" 
                                    ShowGridLines="True">
                                    <TitleStyle BackColor="#1e293b" ForeColor="#ffffff" Font-Bold="True" Height="40px" Font-Size="12pt" />
                                    <NextPrevStyle ForeColor="#38bdf8" Font-Bold="True" />
                                    <DayHeaderStyle BackColor="#f1f5f9" ForeColor="#475569" Font-Bold="True" Height="30px" />
                                    <TodayDayStyle BackColor="#e0f2fe" Font-Bold="True" ForeColor="#0369a1" />
                                    <SelectedDayStyle BackColor="#1d4ed8" ForeColor="#ffffff" Font-Bold="True" />
                                    <OtherMonthDayStyle ForeColor="#cbd5e1" />
                                </asp:Calendar>
                            </div>

                            <div>
                                <!-- Legend Box -->
                                <div class="calendar-legend">
                                    <h4 style="margin-bottom:12px; font-weight:700; color:var(--text-dark);">Calendar Key &amp; Legend</h4>
                                    <div class="legend-item">
                                        <div class="legend-color" style="background:#fee2e2; border:1px solid #fecaca;"></div>
                                        <span>Public / Official Holiday</span>
                                    </div>
                                    <div class="legend-item">
                                        <div class="legend-color" style="background:#f3e8ff; border:1px solid #e9d5ff;"></div>
                                        <span>Mid-Sem / End-Sem Exams</span>
                                    </div>
                                    <div class="legend-item">
                                        <div class="legend-color" style="background:#e0f2fe; border:1px solid #bae6fd;"></div>
                                        <span>TechFest &amp; Campus Events</span>
                                    </div>
                                    <div class="legend-item">
                                        <div class="legend-color" style="background:#dcfce7; border:1px solid #bbf7d0;"></div>
                                        <span>Approved Leave Date</span>
                                    </div>
                                    <div class="legend-item">
                                        <div class="legend-color" style="background:#fef3c7; border:1px solid #fde68a;"></div>
                                        <span>Pending Leave Date</span>
                                    </div>
                                </div>

                                <!-- Selected Date Information Panel -->
                                <asp:Panel ID="pnlSelectedDate" runat="server" Visible="false" style="margin-top:15px; background:#eff6ff; border:1px solid #bfdbfe; border-radius:8px; padding:15px;">
                                    <h4 style="color:#1e40af; font-weight:700; margin-bottom:6px;">Selected Date Details:</h4>
                                    <p style="font-size:0.95rem; font-weight:700; color:#1e3a8a;"><asp:Label ID="lblSelectedDateInfo" runat="server"></asp:Label></p>
                                    <p style="font-size:0.88rem; color:#1e40af; margin-top:4px;"><asp:Label ID="lblSelectedDateDetails" runat="server"></asp:Label></p>
                                    
                                    <div style="margin-top:12px;">
                                        <asp:Button ID="btnQuickApply" runat="server" Text="Apply Leave for this Date" OnClick="btnQuickApply_Click" CssClass="btn btn-primary" style="width:100%; justify-content:center;" CausesValidation="false" />
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                    </asp:View>

                    <!-- VIEW 1: APPLY FOR LEAVE -->
                    <asp:View ID="ViewApplyLeave" runat="server">
                        <div class="view-title">
                            <span>Leave Application Form</span>
                            <span style="font-size:0.85rem; font-weight:normal; color:var(--text-muted);">Rich Control: &lt;asp:FileUpload&gt; &amp; Validation Controls</span>
                        </div>

                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="validation-summary" HeaderText="Please address the following validation errors:" />

                        <div class="form-grid">
                            <div class="form-group">
                                <label for="ddlLeaveType">Select Leave Type: *</label>
                                <asp:DropDownList ID="ddlLeaveType" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="Casual Leave">Casual Leave (CL)</asp:ListItem>
                                    <asp:ListItem Value="Medical Leave">Medical Leave (ML)</asp:ListItem>
                                    <asp:ListItem Value="Academic Duty Leave">Academic Duty Leave (AL)</asp:ListItem>
                                </asp:DropDownList>
                            </div>

                            <div class="form-group">
                                <label for="txtStartDate">Start Date: *</label>
                                <asp:TextBox ID="txtStartDate" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvStart" runat="server" ControlToValidate="txtStartDate" ErrorMessage="Start Date is required." ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>

                            <div class="form-group">
                                <label for="txtEndDate">End Date: *</label>
                                <asp:TextBox ID="txtEndDate" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvEnd" runat="server" ControlToValidate="txtEndDate" ErrorMessage="End Date is required." ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>

                            <div class="form-group">
                                <label for="txtEmergencyContact">Emergency Contact Mobile: *</label>
                                <asp:TextBox ID="txtEmergencyContact" runat="server" CssClass="form-control" placeholder="10-digit mobile number"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvContact" runat="server" ControlToValidate="txtEmergencyContact" ErrorMessage="Emergency Contact is required." ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="revContact" runat="server" ControlToValidate="txtEmergencyContact" ValidationExpression="^[0-9]{10}$" ErrorMessage="Enter a valid 10-digit mobile number." ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>
                            </div>

                            <div class="form-group full-width">
                                <label for="txtReason">Reason for Leave: *</label>
                                <asp:TextBox ID="txtReason" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control" placeholder="Provide detailed explanation for your leave request..."></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvReason" runat="server" ControlToValidate="txtReason" ErrorMessage="Reason for leave is required." ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>

                            <div class="form-group full-width" style="background:#f8fafc; border:1px dashed #cbd5e1; padding:15px; border-radius:8px;">
                                <label for="fileUploadDoc">Supporting Document Upload (Rich Control: &lt;asp:FileUpload&gt;):</label>
                                <asp:FileUpload ID="fileUploadDoc" runat="server" CssClass="form-control" style="padding:4px; background:#fff;" />
                                <span style="font-size:0.8rem; color:var(--text-muted); display:block; margin-top:4px;">
                                    Upload Medical Certificate / Duty Event Letter (PDF, PNG, JPG). Required for Medical Leave &gt; 2 days.
                                </span>
                            </div>

                            <div class="form-group full-width">
                                <asp:CheckBox ID="chkRememberMe" runat="server" Text="Remember my identity in Browser Cookie for future sessions" Checked="true" style="font-weight:600; font-size:0.9rem;" />
                            </div>
                        </div>

                        <div style="margin-top:15px; display:flex; gap:10px;">
                            <asp:Button ID="btnSubmitLeave" runat="server" Text="Submit Application" OnClick="btnSubmitLeave_Click" CssClass="btn btn-success" />
                            <asp:Button ID="btnResetLeave" runat="server" Text="Reset Form" OnClick="btnResetLeave_Click" CssClass="btn btn-secondary" CausesValidation="false" />
                        </div>
                    </asp:View>

                    <!-- VIEW 2: MY APPLICATIONS & HISTORY -->
                    <asp:View ID="ViewStatus" runat="server">
                        <div class="view-title">
                            <span>My Leave Applications &amp; History</span>
                            <span style="font-size:0.85rem; font-weight:normal; color:var(--text-muted);">Session Data State</span>
                        </div>

                        <div class="metrics-grid">
                            <div class="metric-card">
                                <div class="metric-val"><asp:Label ID="lblTotalApplied" runat="server">0</asp:Label></div>
                                <div class="metric-lbl">Total Applied</div>
                            </div>
                            <div class="metric-card">
                                <div class="metric-val" style="color:#16a34a;"><asp:Label ID="lblTotalApproved" runat="server">0</asp:Label></div>
                                <div class="metric-lbl">Approved</div>
                            </div>
                            <div class="metric-card">
                                <div class="metric-val" style="color:#b45309;"><asp:Label ID="lblTotalPending" runat="server">0</asp:Label></div>
                                <div class="metric-lbl">Pending</div>
                            </div>
                            <div class="metric-card">
                                <div class="metric-val" style="color:#dc2626;"><asp:Label ID="lblTotalRejected" runat="server">0</asp:Label></div>
                                <div class="metric-lbl">Rejected</div>
                            </div>
                        </div>

                        <asp:Repeater ID="rptMyLeaves" runat="server" OnItemCommand="rptMyLeaves_ItemCommand">
                            <HeaderTemplate>
                                <table class="data-table">
                                    <thead>
                                        <tr>
                                            <th>App ID</th>
                                            <th>Type</th>
                                            <th>Duration</th>
                                            <th>Reason</th>
                                            <th>Document</th>
                                            <th>Status</th>
                                            <th>HOD Remark</th>
                                            <th>Action</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td><strong><%# Eval("Id") %></strong></td>
                                    <td><%# Eval("LeaveType") %></td>
                                    <td>
                                        <%# Convert.ToDateTime(Eval("StartDate")).ToString("dd MMM") %> - <%# Convert.ToDateTime(Eval("EndDate")).ToString("dd MMM yyyy") %>
                                        <br/><small style="color:var(--text-muted);">(<%# Eval("TotalDays") %> days)</small>
                                    </td>
                                    <td><%# Server.HtmlEncode(Eval("Reason").ToString()) %></td>
                                    <td>
                                        <%# Eval("DocumentFileName").ToString() != "None" ? "<a href='uploads/" + Eval("DocumentFileName") + "' target='_blank'>View Document</a>" : "<span style='color:#94a3b8;'>None</span>" %>
                                    </td>
                                    <td>
                                        <span class="status-pill pill-<%# Eval("Status").ToString().ToLower() %>">
                                            <%# Eval("Status") %>
                                        </span>
                                    </td>
                                    <td><%# string.IsNullOrEmpty(Eval("HODRemark").ToString()) ? "<em>Awaiting review</em>" : Server.HtmlEncode(Eval("HODRemark").ToString()) %></td>
                                    <td>
                                        <%# Eval("Status").ToString() == "Pending" ? "<asp:LinkButton runat='server' CommandName='CancelRequest' CommandArgument='" + Eval("Id") + "' CssClass='btn btn-danger' style='padding:3px 8px; font-size:0.78rem;' OnClientClick='return confirm(\"Cancel this leave request?\");'>Cancel</asp:LinkButton>" : "<span style='color:#94a3b8;'>-</span>" %>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                    </tbody>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </asp:View>

                    <!-- VIEW 3: ADMIN / FACULTY APPROVAL PORTAL -->
                    <asp:View ID="ViewAdmin" runat="server">
                        <div class="view-title">
                            <span>HOD / Faculty Leave Approval Portal</span>
                            <span style="font-size:0.85rem; font-weight:normal; color:var(--text-muted);">Manage All Session Requests</span>
                        </div>

                        <p style="margin-bottom:15px; color:var(--text-muted); font-size:0.9rem;">
                            As an HOD or Faculty Administrator, review incoming leave applications, inspect uploaded documents, enter official remarks, and Approve or Reject applications.
                        </p>

                        <asp:Repeater ID="rptAdminLeaves" runat="server" OnItemCommand="rptAdminLeaves_ItemCommand">
                            <HeaderTemplate>
                                <table class="data-table">
                                    <thead>
                                        <tr>
                                            <th>App ID</th>
                                            <th>Applicant</th>
                                            <th>Leave Type</th>
                                            <th>Dates &amp; Days</th>
                                            <th>Reason</th>
                                            <th>Document</th>
                                            <th>Status</th>
                                            <th>HOD Remark &amp; Action</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td><strong><%# Eval("Id") %></strong></td>
                                    <td>
                                        <strong><%# Eval("ApplicantName") %></strong><br />
                                        <small style="color:var(--text-muted);"><%# Eval("Username") %></small>
                                    </td>
                                    <td><%# Eval("LeaveType") %></td>
                                    <td>
                                        <%# Convert.ToDateTime(Eval("StartDate")).ToString("dd MMM") %> - <%# Convert.ToDateTime(Eval("EndDate")).ToString("dd MMM yyyy") %>
                                        <br/><small style="color:var(--text-muted);">(<%# Eval("TotalDays") %> Days)</small>
                                    </td>
                                    <td><%# Server.HtmlEncode(Eval("Reason").ToString()) %></td>
                                    <td>
                                        <%# Eval("DocumentFileName").ToString() != "None" ? "<a href='uploads/" + Eval("DocumentFileName") + "' target='_blank'>View File</a>" : "<span style='color:#94a3b8;'>No Doc</span>" %>
                                    </td>
                                    <td>
                                        <span class="status-pill pill-<%# Eval("Status").ToString().ToLower() %>">
                                            <%# Eval("Status") %>
                                        </span>
                                    </td>
                                    <td>
                                        <div style="display:flex; flex-direction:column; gap:4px;">
                                            <asp:TextBox ID="txtAdminRemark" runat="server" Text='<%# Eval("HODRemark") %>' placeholder="Enter remark..." CssClass="form-control" style="padding:3px 6px; font-size:0.8rem;"></asp:TextBox>
                                            <div style="display:flex; gap:4px;">
                                                <asp:Button ID="btnApprove" runat="server" Text="Approve" CommandName="Approve" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-success" style="padding:3px 8px; font-size:0.78rem;" CausesValidation="false" />
                                                <asp:Button ID="btnReject" runat="server" Text="Reject" CommandName="Reject" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-danger" style="padding:3px 8px; font-size:0.78rem;" CausesValidation="false" />
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                    </tbody>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </asp:View>

                    <!-- VIEW 4: SESSION & COOKIES DIAGNOSTIC INSPECTOR -->
                    <asp:View ID="ViewInspector" runat="server">
                        <div class="view-title">
                            <span>ASP.NET Session State &amp; HTTP Cookies Inspector</span>
                            <span style="font-size:0.85rem; font-weight:normal; color:var(--text-muted);">Real-time State Diagnostics</span>
                        </div>

                        <!-- 1. Session Information Box -->
                        <div class="inspector-section">
                            <div class="inspector-title">ASP.NET Session State Metrics</div>
                            <div class="form-grid" style="margin-bottom:12px;">
                                <div><strong>Session ID:</strong> <asp:Label ID="lblSessionID" runat="server" ForeColor="#1d4ed8" Font-Bold="true"></asp:Label></div>
                                <div><strong>Session Timeout:</strong> <asp:Label ID="lblSessionTimeout" runat="server"></asp:Label></div>
                                <div><strong>Is New Session:</strong> <asp:Label ID="lblSessionIsNew" runat="server"></asp:Label></div>
                                <div><strong>Stored Session Objects Count:</strong> <asp:Label ID="lblSessionCount" runat="server"></asp:Label></div>
                            </div>
                            <div style="display:flex; gap:8px; flex-wrap:wrap;">
                                <asp:Button ID="btnRefreshSession" runat="server" Text="Refresh Inspector" OnClick="btnRefreshSession_Click" CssClass="btn btn-primary" CausesValidation="false" />
                                <asp:Button ID="btnClearSession" runat="server" Text="Clear Session Data (Session.Clear())" OnClick="btnClearSession_Click" CssClass="btn btn-secondary" CausesValidation="false" />
                                <asp:Button ID="btnAbandonSession" runat="server" Text="Abandon Session (Session.Abandon())" OnClick="btnAbandonSession_Click" CssClass="btn btn-danger" CausesValidation="false" OnClientClick="return confirm('This will destroy the active Session ID. Proceed?');" />
                            </div>
                        </div>

                        <!-- 2. Active Request Cookies Table -->
                        <div class="inspector-section">
                            <div class="inspector-title">Active Request Cookies Table</div>
                            <asp:Repeater ID="rptCookies" runat="server">
                                <HeaderTemplate>
                                    <table class="data-table">
                                        <thead>
                                            <tr>
                                                <th>Cookie Name</th>
                                                <th>Cookie Value</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td><strong><%# Eval("Key") %></strong></td>
                                        <td><code><%# Server.HtmlEncode(Eval("Value").ToString()) %></code></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                        </tbody>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>

                        <!-- 3. Cookie Management & Creator Tool -->
                        <div class="inspector-section">
                            <div class="inspector-title">Cookie Creation &amp; Management Tool</div>
                            <div class="form-grid">
                                <div class="form-group">
                                    <label>Cookie Name:</label>
                                    <asp:TextBox ID="txtCookieName" runat="server" CssClass="form-control" placeholder="e.g. UserThemePreference"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label>Cookie Value:</label>
                                    <asp:TextBox ID="txtCookieValue" runat="server" CssClass="form-control" placeholder="e.g. LightMode"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label>Expiration:</label>
                                    <asp:DropDownList ID="ddlCookieExpiry" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="0">Session Cookie (Browser Close)</asp:ListItem>
                                        <asp:ListItem Value="1">1 Day</asp:ListItem>
                                        <asp:ListItem Value="7">7 Days</asp:ListItem>
                                        <asp:ListItem Value="30">30 Days</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group" style="display:flex; align-items:flex-end;">
                                    <asp:Button ID="btnSetCookie" runat="server" Text="Set / Save Cookie" OnClick="btnSetCookie_Click" CssClass="btn btn-success" style="width:100%; justify-content:center;" CausesValidation="false" />
                                </div>
                            </div>

                            <hr style="margin:15px 0; border:none; border-top:1px solid var(--border-color);" />

                            <div class="form-grid">
                                <div class="form-group">
                                    <label>Quick Theme Switcher Cookie:</label>
                                    <asp:DropDownList ID="ddlThemePicker" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="Default Light">Default Light</asp:ListItem>
                                        <asp:ListItem Value="Classic Academic">Classic Academic</asp:ListItem>
                                        <asp:ListItem Value="Compact High Contrast">Compact High Contrast</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group" style="display:flex; align-items:flex-end; gap:8px;">
                                    <asp:Button ID="btnSaveThemeCookie" runat="server" Text="Save Theme Cookie" OnClick="btnSaveThemeCookie_Click" CssClass="btn btn-primary" CausesValidation="false" />
                                    <asp:Button ID="btnClearCookies" runat="server" Text="Clear Custom Cookies" OnClick="btnClearCookies_Click" CssClass="btn btn-danger" CausesValidation="false" />
                                </div>
                            </div>
                        </div>
                    </asp:View>

                </asp:MultiView>
            </div>
        </div>
    </form>
</body>
</html>
