using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DefaultPage : Page
{
    [Serializable]
    public class UserProfile
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public string Department { get; set; }
        public int CasualBalance { get; set; }
        public int MedicalBalance { get; set; }
        public int AcademicBalance { get; set; }

        public UserProfile()
        {
            Username = "rounak24";
            FullName = "Rounak Keshri";
            Role = "Student";
            Department = "Computer Science & Engineering";
            CasualBalance = 12;
            MedicalBalance = 10;
            AcademicBalance = 8;
        }
    }

    [Serializable]
    public class LeaveApplication
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string ApplicantName { get; set; }
        public string LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalDays { get; set; }
        public string Reason { get; set; }
        public string EmergencyContact { get; set; }
        public string DocumentFileName { get; set; }
        public string Status { get; set; }
        public DateTime AppliedDate { get; set; }
        public string HODRemark { get; set; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        InitializeSessionState();
        InitializeCookieState();

        if (!IsPostBack)
        {
            mvMain.ActiveViewIndex = 0;
            UpdateTabStyles();
            BindUserData();
            BindMyLeaves();
            BindAdminLeaves();
            BindSessionAndCookieInspector();
        }
    }

    private void InitializeSessionState()
    {
        if (Session["UserProfile"] == null)
        {
            Session["UserProfile"] = new UserProfile();
        }

        if (Session["LeaveRequests"] == null)
        {
            var sampleLeaves = new List<LeaveApplication>
            {
                new LeaveApplication
                {
                    Id = "LV-2026-001",
                    Username = "rounak24",
                    ApplicantName = "Rounak Keshri",
                    LeaveType = "Medical Leave",
                    StartDate = new DateTime(2026, 10, 5),
                    EndDate = new DateTime(2026, 10, 7),
                    TotalDays = 3,
                    Reason = "High fever and viral flu doctors advice for rest.",
                    EmergencyContact = "9876543210",
                    DocumentFileName = "medical_certificate.pdf",
                    Status = "Approved",
                    AppliedDate = DateTime.Now.AddDays(-10),
                    HODRemark = "Approved as per medical report submitted."
                },
                new LeaveApplication
                {
                    Id = "LV-2026-002",
                    Username = "rounak24",
                    ApplicantName = "Rounak Keshri",
                    LeaveType = "Academic Duty Leave",
                    StartDate = new DateTime(2026, 11, 5),
                    EndDate = new DateTime(2026, 11, 7),
                    TotalDays = 3,
                    Reason = "Attending National Inter-College Hackathon / TechFest 2026.",
                    EmergencyContact = "9876543210",
                    DocumentFileName = "hackathon_invitation.pdf",
                    Status = "Pending",
                    AppliedDate = DateTime.Now.AddDays(-2),
                    HODRemark = ""
                }
            };
            Session["LeaveRequests"] = sampleLeaves;
        }
    }

    private void InitializeCookieState()
    {
        HttpCookie lastVisitCookie = Request.Cookies["LastVisitTimestamp"];
        if (lastVisitCookie != null && !string.IsNullOrEmpty(lastVisitCookie.Value))
        {
            lblLastVisit.Text = lastVisitCookie.Value;
        }
        else
        {
            lblLastVisit.Text = "First Visit (No previous cookie)";
        }

        HttpCookie newLastVisit = new HttpCookie("LastVisitTimestamp", DateTime.Now.ToString("dd MMM yyyy, hh:mm:ss tt"));
        newLastVisit.Expires = DateTime.Now.AddDays(30);
        Response.Cookies.Add(newLastVisit);

        HttpCookie userCookie = Request.Cookies["RememberedUser"];
        if (userCookie != null && !string.IsNullOrEmpty(userCookie.Value))
        {
            lblRememberedUser.Text = userCookie.Value;
        }
        else
        {
            lblRememberedUser.Text = "Guest / Not Cookie-Saved";
        }

        HttpCookie themeCookie = Request.Cookies["UserThemePreference"];
        if (themeCookie != null && !string.IsNullOrEmpty(themeCookie.Value))
        {
            lblCurrentTheme.Text = themeCookie.Value;
        }
        else
        {
            lblCurrentTheme.Text = "Default Light";
        }
    }

    private void BindUserData()
    {
        UserProfile profile = (UserProfile)Session["UserProfile"];
        if (profile != null)
        {
            lblUserName.Text = profile.FullName;
            lblUserDept.Text = profile.Department;
            lblCasualLeave.Text = profile.CasualBalance.ToString();
            lblMedicalLeave.Text = profile.MedicalBalance.ToString();
            lblAcademicLeave.Text = profile.AcademicBalance.ToString();
            ddlUserRole.SelectedValue = profile.Role;
        }
    }

    protected void ddlUserRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        UserProfile profile = (UserProfile)Session["UserProfile"];
        if (profile != null)
        {
            profile.Role = ddlUserRole.SelectedValue;
            Session["UserProfile"] = profile;
            BindUserData();
            BindAdminLeaves();
            ShowNotification("Role updated to " + profile.Role + " in Session.", "info");
        }
    }

    #region Tab Navigation
    protected void btnTabCalendar_Click(object sender, EventArgs e)
    {
        mvMain.ActiveViewIndex = 0;
        UpdateTabStyles();
    }

    protected void btnTabApply_Click(object sender, EventArgs e)
    {
        mvMain.ActiveViewIndex = 1;
        UpdateTabStyles();
    }

    protected void btnTabStatus_Click(object sender, EventArgs e)
    {
        mvMain.ActiveViewIndex = 2;
        UpdateTabStyles();
        BindMyLeaves();
    }

    protected void btnTabAdmin_Click(object sender, EventArgs e)
    {
        mvMain.ActiveViewIndex = 3;
        UpdateTabStyles();
        BindAdminLeaves();
    }

    protected void btnTabInspector_Click(object sender, EventArgs e)
    {
        mvMain.ActiveViewIndex = 4;
        UpdateTabStyles();
        BindSessionAndCookieInspector();
    }

    private void UpdateTabStyles()
    {
        int idx = mvMain.ActiveViewIndex;
        btnTabCalendar.CssClass = idx == 0 ? "nav-btn active" : "nav-btn";
        btnTabApply.CssClass = idx == 1 ? "nav-btn active" : "nav-btn";
        btnTabStatus.CssClass = idx == 2 ? "nav-btn active" : "nav-btn";
        btnTabAdmin.CssClass = idx == 3 ? "nav-btn active" : "nav-btn";
        btnTabInspector.CssClass = idx == 4 ? "nav-btn active" : "nav-btn";
    }
    #endregion

    #region Academic Calendar
    protected void CalendarAcademic_DayRender(object sender, DayRenderEventArgs e)
    {
        DateTime day = e.Day.Date;

        if ((day.Month == 10 && day.Day == 2) ||
            (day.Month == 10 && day.Day == 24) ||
            (day.Month == 11 && day.Day == 12) ||
            (day.Month == 12 && day.Day == 25) ||
            (day.Month == 1 && day.Day == 26))
        {
            e.Cell.CssClass += " cal-day-holiday";
            Label lbl = new Label();
            lbl.Text = "<br/><span class='cal-badge badge-holiday'>Holiday</span>";
            e.Cell.Controls.Add(lbl);
        }
        else if (day.Month == 10 && day.Day >= 15 && day.Day <= 20)
        {
            e.Cell.CssClass += " cal-day-exam";
            Label lbl = new Label();
            lbl.Text = "<br/><span class='cal-badge badge-exam'>Mid-Sem Exam</span>";
            e.Cell.Controls.Add(lbl);
        }
        else if (day.Month == 11 && day.Day >= 5 && day.Day <= 7)
        {
            e.Cell.CssClass += " cal-day-event";
            Label lbl = new Label();
            lbl.Text = "<br/><span class='cal-badge badge-event'>TechFest Event</span>";
            e.Cell.Controls.Add(lbl);
        }

        List<LeaveApplication> leaves = (List<LeaveApplication>)Session["LeaveRequests"];
        if (leaves != null)
        {
            foreach (var leave in leaves)
            {
                if (day >= leave.StartDate.Date && day <= leave.EndDate.Date)
                {
                    Label lblLeave = new Label();
                    if (leave.Status == "Approved")
                    {
                        e.Cell.CssClass += " cal-day-approved-leave";
                        lblLeave.Text = "<br/><span class='cal-badge badge-approved'>Leave Approved</span>";
                    }
                    else if (leave.Status == "Pending")
                    {
                        e.Cell.CssClass += " cal-day-pending-leave";
                        lblLeave.Text = "<br/><span class='cal-badge badge-pending'>Leave Pending</span>";
                    }
                    e.Cell.Controls.Add(lblLeave);
                }
            }
        }
    }

    protected void CalendarAcademic_SelectionChanged(object sender, EventArgs e)
    {
        DateTime selectedDate = CalendarAcademic.SelectedDate;
        lblSelectedDateInfo.Text = selectedDate.ToString("dd MMMM yyyy (dddd)");

        string note = "Normal Academic / Lecture Day";
        if (selectedDate.Month == 10 && selectedDate.Day == 2) note = "Public Holiday: Gandhi Jayanti";
        else if (selectedDate.Month == 10 && selectedDate.Day == 24) note = "Public Holiday: Dussehra";
        else if (selectedDate.Month == 11 && selectedDate.Day == 12) note = "Public Holiday: Diwali";
        else if (selectedDate.Month == 12 && selectedDate.Day == 25) note = "Public Holiday: Christmas";
        else if (selectedDate.Month == 10 && selectedDate.Day >= 15 && selectedDate.Day <= 20) note = "Mid-Semester Examinations";
        else if (selectedDate.Month == 11 && selectedDate.Day >= 5 && selectedDate.Day <= 7) note = "TechFest 2026 & Cultural Fest";

        lblSelectedDateDetails.Text = note;

        txtStartDate.Text = selectedDate.ToString("yyyy-MM-dd");
        txtEndDate.Text = selectedDate.ToString("yyyy-MM-dd");
        pnlSelectedDate.Visible = true;
    }

    protected void btnQuickApply_Click(object sender, EventArgs e)
    {
        mvMain.ActiveViewIndex = 1;
        UpdateTabStyles();
    }
    #endregion

    #region Apply Leave
    protected void btnSubmitLeave_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            DateTime start, end;
            if (!DateTime.TryParse(txtStartDate.Text, out start) || !DateTime.TryParse(txtEndDate.Text, out end))
            {
                ShowNotification("Invalid date format selected.", "danger");
                return;
            }

            if (end < start)
            {
                ShowNotification("End Date cannot be earlier than Start Date!", "danger");
                return;
            }

            int days = (end - start).Days + 1;
            UserProfile profile = (UserProfile)Session["UserProfile"];
            string leaveType = ddlLeaveType.SelectedValue;

            if (leaveType == "Casual Leave" && days > profile.CasualBalance)
            {
                ShowNotification("Insufficient Casual Leave balance! Remaining: " + profile.CasualBalance + " days.", "warning");
                return;
            }
            if (leaveType == "Medical Leave" && days > profile.MedicalBalance)
            {
                ShowNotification("Insufficient Medical Leave balance! Remaining: " + profile.MedicalBalance + " days.", "warning");
                return;
            }
            if (leaveType == "Academic Duty Leave" && days > profile.AcademicBalance)
            {
                ShowNotification("Insufficient Academic Leave balance! Remaining: " + profile.AcademicBalance + " days.", "warning");
                return;
            }

            string savedFileName = "None";
            if (fileUploadDoc.HasFile)
            {
                try
                {
                    string ext = Path.GetExtension(fileUploadDoc.FileName).ToLower();
                    if (ext == ".pdf" || ext == ".png" || ext == ".jpg" || ext == ".jpeg" || ext == ".docx")
                    {
                        string uploadDir = Server.MapPath("~/uploads/");
                        if (!Directory.Exists(uploadDir))
                        {
                            Directory.CreateDirectory(uploadDir);
                        }
                        savedFileName = Guid.NewGuid().ToString().Substring(0, 8) + "_" + Path.GetFileName(fileUploadDoc.FileName);
                        fileUploadDoc.SaveAs(Path.Combine(uploadDir, savedFileName));
                    }
                    else
                    {
                        ShowNotification("Only PDF, PNG, JPG, or DOCX documents are allowed.", "danger");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowNotification("Error uploading document: " + ex.Message, "danger");
                    return;
                }
            }
            else if (leaveType == "Medical Leave" && days > 2)
            {
                ShowNotification("Medical Leave for more than 2 days requires uploading a medical certificate!", "danger");
                return;
            }

            if (chkRememberMe.Checked)
            {
                HttpCookie userCookie = new HttpCookie("RememberedUser", profile.FullName + " (" + profile.Username + ")");
                userCookie.Expires = DateTime.Now.AddDays(15);
                Response.Cookies.Add(userCookie);
                lblRememberedUser.Text = userCookie.Value;
            }

            List<LeaveApplication> leaves = (List<LeaveApplication>)Session["LeaveRequests"];
            string appId = "LV-" + DateTime.Now.Year + "-" + (leaves.Count + 1).ToString("D3");

            LeaveApplication newApp = new LeaveApplication
            {
                Id = appId,
                Username = profile.Username,
                ApplicantName = profile.FullName,
                LeaveType = leaveType,
                StartDate = start,
                EndDate = end,
                TotalDays = days,
                Reason = txtReason.Text.Trim(),
                EmergencyContact = txtEmergencyContact.Text.Trim(),
                DocumentFileName = savedFileName,
                Status = "Pending",
                AppliedDate = DateTime.Now,
                HODRemark = ""
            };

            leaves.Add(newApp);
            Session["LeaveRequests"] = leaves;

            ShowNotification("Leave Application " + appId + " submitted successfully! Status: Pending Approval.", "success");

            txtReason.Text = "";
            txtEmergencyContact.Text = "";

            mvMain.ActiveViewIndex = 2;
            UpdateTabStyles();
            BindMyLeaves();
        }
    }

    protected void btnResetLeave_Click(object sender, EventArgs e)
    {
        txtStartDate.Text = "";
        txtEndDate.Text = "";
        txtReason.Text = "";
        txtEmergencyContact.Text = "";
        ddlLeaveType.SelectedIndex = 0;
        pnlAlert.Visible = false;
    }
    #endregion

    #region My Leave Applications
    private void BindMyLeaves()
    {
        List<LeaveApplication> leaves = (List<LeaveApplication>)Session["LeaveRequests"];
        UserProfile profile = (UserProfile)Session["UserProfile"];

        var myLeaves = leaves.Where(l => l.Username == profile.Username).OrderByDescending(l => l.AppliedDate).ToList();

        rptMyLeaves.DataSource = myLeaves;
        rptMyLeaves.DataBind();

        lblTotalApplied.Text = myLeaves.Count.ToString();
        lblTotalApproved.Text = myLeaves.Count(l => l.Status == "Approved").ToString();
        lblTotalPending.Text = myLeaves.Count(l => l.Status == "Pending").ToString();
        lblTotalRejected.Text = myLeaves.Count(l => l.Status == "Rejected").ToString();
    }

    protected void rptMyLeaves_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "CancelRequest")
        {
            string appId = e.CommandArgument.ToString();
            List<LeaveApplication> leaves = (List<LeaveApplication>)Session["LeaveRequests"];
            var target = leaves.FirstOrDefault(l => l.Id == appId);
            if (target != null && target.Status == "Pending")
            {
                leaves.Remove(target);
                Session["LeaveRequests"] = leaves;
                ShowNotification("Leave Application " + appId + " was cancelled.", "info");
                BindMyLeaves();
            }
        }
    }
    #endregion

    #region Admin / Faculty Approval Portal
    private void BindAdminLeaves()
    {
        List<LeaveApplication> leaves = (List<LeaveApplication>)Session["LeaveRequests"];
        var pendingOrAll = leaves.OrderByDescending(l => l.AppliedDate).ToList();

        rptAdminLeaves.DataSource = pendingOrAll;
        rptAdminLeaves.DataBind();
    }

    protected void rptAdminLeaves_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        string appId = e.CommandArgument.ToString();
        List<LeaveApplication> leaves = (List<LeaveApplication>)Session["LeaveRequests"];
        UserProfile profile = (UserProfile)Session["UserProfile"];

        var target = leaves.FirstOrDefault(l => l.Id == appId);
        if (target != null)
        {
            TextBox txtRemark = (TextBox)e.Item.FindControl("txtAdminRemark");
            string remark = txtRemark != null ? txtRemark.Text.Trim() : "";

            if (e.CommandName == "Approve")
            {
                target.Status = "Approved";
                target.HODRemark = string.IsNullOrEmpty(remark) ? "Approved by HOD/Faculty." : remark;

                if (target.LeaveType == "Casual Leave") profile.CasualBalance = Math.Max(0, profile.CasualBalance - target.TotalDays);
                else if (target.LeaveType == "Medical Leave") profile.MedicalBalance = Math.Max(0, profile.MedicalBalance - target.TotalDays);
                else if (target.LeaveType == "Academic Duty Leave") profile.AcademicBalance = Math.Max(0, profile.AcademicBalance - target.TotalDays);

                Session["UserProfile"] = profile;
                ShowNotification("Application " + appId + " APPROVED! User leave balance updated.", "success");
            }
            else if (e.CommandName == "Reject")
            {
                target.Status = "Rejected";
                target.HODRemark = string.IsNullOrEmpty(remark) ? "Rejected by HOD/Faculty." : remark;
                ShowNotification("Application " + appId + " REJECTED.", "warning");
            }

            Session["LeaveRequests"] = leaves;
            BindUserData();
            BindAdminLeaves();
        }
    }
    #endregion

    #region Session & Cookie Inspector
    private void BindSessionAndCookieInspector()
    {
        lblSessionID.Text = Session.SessionID;
        lblSessionTimeout.Text = Session.Timeout + " minutes";
        lblSessionIsNew.Text = Session.IsNewSession ? "True (New Session)" : "False (Active Session)";
        lblSessionCount.Text = Session.Count.ToString();

        List<KeyValuePair<string, string>> cookieList = new List<KeyValuePair<string, string>>();
        for (int i = 0; i < Request.Cookies.Count; i++)
        {
            HttpCookie c = Request.Cookies[i];
            cookieList.Add(new KeyValuePair<string, string>(c.Name, c.Value));
        }

        rptCookies.DataSource = cookieList;
        rptCookies.DataBind();
    }

    protected void btnSetCookie_Click(object sender, EventArgs e)
    {
        string name = txtCookieName.Text.Trim();
        string val = txtCookieValue.Text.Trim();

        if (string.IsNullOrEmpty(name))
        {
            ShowNotification("Cookie Name cannot be empty.", "warning");
            return;
        }

        HttpCookie cookie = new HttpCookie(name, val);
        int days = Convert.ToInt32(ddlCookieExpiry.SelectedValue);
        if (days > 0)
        {
            cookie.Expires = DateTime.Now.AddDays(days);
        }
        Response.Cookies.Add(cookie);

        ShowNotification("Cookie '" + name + "' updated successfully!", "success");
        BindSessionAndCookieInspector();
        InitializeCookieState();
    }

    protected void btnSaveThemeCookie_Click(object sender, EventArgs e)
    {
        string theme = ddlThemePicker.SelectedValue;
        HttpCookie themeCookie = new HttpCookie("UserThemePreference", theme);
        themeCookie.Expires = DateTime.Now.AddDays(30);
        Response.Cookies.Add(themeCookie);

        lblCurrentTheme.Text = theme;
        ShowNotification("Theme cookie set to '" + theme + "'!", "info");
        BindSessionAndCookieInspector();
    }

    protected void btnClearCookies_Click(object sender, EventArgs e)
    {
        string[] cookiesToClear = new string[] { "RememberedUser", "UserThemePreference", "LastVisitTimestamp" };
        foreach (string name in cookiesToClear)
        {
            HttpCookie c = new HttpCookie(name);
            c.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(c);
        }

        lblRememberedUser.Text = "Guest / Not Cookie-Saved";
        lblCurrentTheme.Text = "Default Light";
        ShowNotification("All custom portal cookies cleared!", "info");
        BindSessionAndCookieInspector();
    }

    protected void btnRefreshSession_Click(object sender, EventArgs e)
    {
        BindSessionAndCookieInspector();
        ShowNotification("Session and Cookie status refreshed.", "info");
    }

    protected void btnClearSession_Click(object sender, EventArgs e)
    {
        Session.Clear();
        InitializeSessionState();
        BindUserData();
        BindMyLeaves();
        BindAdminLeaves();
        BindSessionAndCookieInspector();
        ShowNotification("Session data cleared (Session.Clear()). Re-initialized with defaults.", "warning");
    }

    protected void btnAbandonSession_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Response.Redirect(Request.RawUrl);
    }
    #endregion

    private void ShowNotification(string message, string type)
    {
        pnlAlert.Visible = true;
        lblAlertText.Text = message;
        pnlAlert.CssClass = "alert-box alert-" + type;
    }
}
