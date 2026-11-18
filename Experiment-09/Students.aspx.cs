using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using Experiment_09.Data;

namespace Experiment_09
{
    public partial class Students : Page
    {
        private string SortDirectionExpression
        {
            get { return ViewState["SortDirection"] as string ?? "ASC"; }
            set { ViewState["SortDirection"] = value; }
        }

        private string SortColumnExpression
        {
            get { return ViewState["SortColumn"] as string ?? "StudentId"; }
            set { ViewState["SortColumn"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DBConnection.EnsureDatabaseAndTables();

                // Check query string filter (e.g. Students.aspx?status=Placed)
                string statusParam = Request.QueryString["status"];
                if (!string.IsNullOrEmpty(statusParam))
                {
                    var item = ddlStatus.Items.FindByValue(statusParam);
                    if (item != null)
                    {
                        ddlStatus.SelectedValue = statusParam;
                    }
                }

                BindGridView();
                BindDetailsView(1); // Default to first student
                PopulateFormViewCompanyDropdown();
            }
        }

        private void BindGridView()
        {
            try
            {
                string query = @"SELECT s.StudentId, s.FullName, s.Email, s.Department, s.CGPA, s.GraduationYear, 
                                        s.PlacementStatus, c.CompanyName, c.Package 
                                 FROM Students s 
                                 LEFT JOIN Companies c ON s.CompanyId = c.CompanyId 
                                 WHERE 1=1";

                var parameters = new System.Collections.Generic.List<SqlParameter>();

                if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    query += " AND (s.FullName LIKE @Search OR s.Email LIKE @Search)";
                    parameters.Add(new SqlParameter("@Search", "%" + txtSearch.Text.Trim() + "%"));
                }

                if (!string.IsNullOrWhiteSpace(ddlDepartment.SelectedValue))
                {
                    query += " AND s.Department = @Department";
                    parameters.Add(new SqlParameter("@Department", ddlDepartment.SelectedValue));
                }

                if (!string.IsNullOrWhiteSpace(ddlStatus.SelectedValue))
                {
                    query += " AND s.PlacementStatus = @Status";
                    parameters.Add(new SqlParameter("@Status", ddlStatus.SelectedValue));
                }

                // Sorting
                query += string.Format(" ORDER BY s.{0} {1}", SortColumnExpression, SortDirectionExpression);

                DataTable dt = DBConnection.ExecuteDataTable(query, parameters.ToArray());
                gvStudents.DataSource = dt;
                gvStudents.DataBind();

                lblRecordsCount.Text = dt.Rows.Count + " Records Displayed";
            }
            catch (Exception ex)
            {
                ShowAlert("Error loading students: " + ex.Message, "danger");
            }
        }

        private void BindDetailsView(int studentId)
        {
            try
            {
                string query = @"SELECT s.StudentId, s.FullName, s.Email, s.Department, s.CGPA, s.GraduationYear, 
                                        s.PlacementStatus, c.CompanyName, c.Package 
                                 FROM Students s 
                                 LEFT JOIN Companies c ON s.CompanyId = c.CompanyId 
                                 ORDER BY s.StudentId";

                DataTable dt = DBConnection.ExecuteDataTable(query);
                dvStudent.DataSource = dt;
                dvStudent.DataBind();
            }
            catch (Exception ex)
            {
                ShowAlert("Error loading details view: " + ex.Message, "danger");
            }
        }

        private void PopulateFormViewCompanyDropdown()
        {
            try
            {
                var ddlCompany = fvStudent.FindControl("ddlInsertCompany") as DropDownList;
                if (ddlCompany != null)
                {
                    DataTable dtCompanies = DBConnection.ExecuteDataTable("SELECT CompanyId, CompanyName + ' (' + CAST(Package as NVARCHAR) + ' LPA)' as DisplayText FROM Companies ORDER BY CompanyName");
                    ddlCompany.DataSource = dtCompanies;
                    ddlCompany.DataTextField = "DisplayText";
                    ddlCompany.DataValueField = "CompanyId";
                    ddlCompany.DataBind();
                    ddlCompany.Items.Insert(0, new ListItem("-- Select Company (Optional) --", ""));
                }
            }
            catch (Exception ex)
            {
                ShowAlert("Error loading company list: " + ex.Message, "warning");
            }
        }

        #region GridView Event Handlers

        protected void gvStudents_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvStudents.PageIndex = e.NewPageIndex;
            BindGridView();
        }

        protected void gvStudents_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (SortColumnExpression == e.SortExpression)
            {
                SortDirectionExpression = SortDirectionExpression == "ASC" ? "DESC" : "ASC";
            }
            else
            {
                SortColumnExpression = e.SortExpression;
                SortDirectionExpression = "ASC";
            }
            BindGridView();
        }

        protected void gvStudents_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvStudents.EditIndex = e.NewEditIndex;
            BindGridView();
        }

        protected void gvStudents_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvStudents.EditIndex = -1;
            BindGridView();
        }

        protected void gvStudents_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int studentId = Convert.ToInt32(gvStudents.DataKeys[e.RowIndex].Value);
                GridViewRow row = gvStudents.Rows[e.RowIndex];

                string fullName = ((TextBox)row.Cells[1].Controls[0]).Text.Trim();
                string email = ((TextBox)row.Cells[2].Controls[0]).Text.Trim();
                string dept = ((TextBox)row.Cells[3].Controls[0]).Text.Trim();
                decimal cgpa = Convert.ToDecimal(((TextBox)row.Cells[4].Controls[0]).Text.Trim());
                int gradYear = Convert.ToInt32(((TextBox)row.Cells[5].Controls[0]).Text.Trim());

                var ddlStatus = row.FindControl("ddlEditStatus") as DropDownList;
                string status = ddlStatus != null ? ddlStatus.SelectedValue : "Not Placed";

                string query = @"UPDATE Students 
                                 SET FullName = @FullName, Email = @Email, Department = @Department, 
                                     CGPA = @CGPA, GraduationYear = @GraduationYear, PlacementStatus = @PlacementStatus 
                                 WHERE StudentId = @StudentId";

                int rows = DBConnection.ExecuteNonQuery(query,
                    new SqlParameter("@FullName", fullName),
                    new SqlParameter("@Email", email),
                    new SqlParameter("@Department", dept),
                    new SqlParameter("@CGPA", cgpa),
                    new SqlParameter("@GraduationYear", gradYear),
                    new SqlParameter("@PlacementStatus", status),
                    new SqlParameter("@StudentId", studentId));

                gvStudents.EditIndex = -1;
                BindGridView();
                BindDetailsView(studentId);
                ShowAlert("Student #" + studentId + " updated successfully via GridView Edit!", "success");
            }
            catch (Exception ex)
            {
                ShowAlert("Update failed: " + ex.Message, "danger");
            }
        }

        protected void gvStudents_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int studentId = Convert.ToInt32(gvStudents.DataKeys[e.RowIndex].Value);
                string query = "DELETE FROM Students WHERE StudentId = @StudentId";

                int rows = DBConnection.ExecuteNonQuery(query, new SqlParameter("@StudentId", studentId));
                BindGridView();
                BindDetailsView(1);
                ShowAlert("Student #" + studentId + " deleted successfully!", "success");
            }
            catch (Exception ex)
            {
                ShowAlert("Delete failed: " + ex.Message, "danger");
            }
        }

        protected void gvStudents_SelectedIndexChanged(object sender, EventArgs e)
        {
            int studentId = Convert.ToInt32(gvStudents.SelectedDataKey.Value);
            
            // Navigate DetailsView to selected student
            DataTable dt = dvStudent.DataSource as DataTable;
            if (dt == null)
            {
                dt = DBConnection.ExecuteDataTable(@"SELECT s.StudentId, s.FullName, s.Email, s.Department, s.CGPA, s.GraduationYear, 
                                                            s.PlacementStatus, c.CompanyName, c.Package 
                                                     FROM Students s 
                                                     LEFT JOIN Companies c ON s.CompanyId = c.CompanyId 
                                                     ORDER BY s.StudentId");
                dvStudent.DataSource = dt;
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (Convert.ToInt32(dt.Rows[i]["StudentId"]) == studentId)
                {
                    dvStudent.PageIndex = i;
                    break;
                }
            }
            dvStudent.DataBind();
        }

        #endregion

        #region DetailsView Event Handlers

        protected void dvStudent_PageIndexChanging(object sender, DetailsViewPageEventArgs e)
        {
            dvStudent.PageIndex = e.NewPageIndex;
            BindDetailsView(1);
        }

        #endregion

        #region FormView Event Handlers

        protected void fvStudent_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            try
            {
                var txtName = fvStudent.FindControl("txtFullName") as TextBox;
                var txtEmail = fvStudent.FindControl("txtEmail") as TextBox;
                var ddlDept = fvStudent.FindControl("ddlDept") as DropDownList;
                var txtCGPA = fvStudent.FindControl("txtCGPA") as TextBox;
                var txtYear = fvStudent.FindControl("txtYear") as TextBox;
                var ddlStatus = fvStudent.FindControl("ddlInsertStatus") as DropDownList;
                var ddlCompany = fvStudent.FindControl("ddlInsertCompany") as DropDownList;

                if (txtName == null || txtEmail == null || ddlDept == null || txtCGPA == null || txtYear == null)
                {
                    ShowAlert("Form controls could not be found.", "danger");
                    return;
                }

                string name = txtName.Text.Trim();
                string email = txtEmail.Text.Trim();
                string dept = ddlDept.SelectedValue;
                decimal cgpa = Convert.ToDecimal(txtCGPA.Text.Trim());
                int year = Convert.ToInt32(txtYear.Text.Trim());
                string status = ddlStatus != null ? ddlStatus.SelectedValue : "Not Placed";
                object companyId = DBNull.Value;

                if (status == "Placed" && ddlCompany != null && !string.IsNullOrEmpty(ddlCompany.SelectedValue))
                {
                    companyId = Convert.ToInt32(ddlCompany.SelectedValue);
                }

                string query = @"INSERT INTO Students (FullName, Email, Department, CGPA, GraduationYear, PlacementStatus, CompanyId)
                                 VALUES (@FullName, @Email, @Department, @CGPA, @GraduationYear, @PlacementStatus, @CompanyId)";

                int rows = DBConnection.ExecuteNonQuery(query,
                    new SqlParameter("@FullName", name),
                    new SqlParameter("@Email", email),
                    new SqlParameter("@Department", dept),
                    new SqlParameter("@CGPA", cgpa),
                    new SqlParameter("@GraduationYear", year),
                    new SqlParameter("@PlacementStatus", status),
                    new SqlParameter("@CompanyId", companyId));

                ShowAlert("Student '" + name + "' registered successfully via FormView!", "success");

                // Reset form inputs
                txtName.Text = "";
                txtEmail.Text = "";
                txtCGPA.Text = "";

                // Rebind views
                BindGridView();
                BindDetailsView(1);
            }
            catch (Exception ex)
            {
                ShowAlert("Registration failed: " + ex.Message, "danger");
            }
        }

        #endregion

        #region Toolbar Buttons

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            gvStudents.PageIndex = 0;
            BindGridView();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            ddlDepartment.SelectedIndex = 0;
            ddlStatus.SelectedIndex = 0;
            gvStudents.PageIndex = 0;
            BindGridView();
        }

        #endregion

        private void ShowAlert(string message, string alertType)
        {
            pnlAlert.Visible = true;
            pnlAlert.CssClass = "alert alert-" + alertType + " alert-dismissible fade show border-0 shadow-sm rounded-3 mb-4";
            litAlertMsg.Text = "<strong>Notice:</strong> " + message;
        }
    }
}
