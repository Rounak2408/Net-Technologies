using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using Experiment_09.Data;

namespace Experiment_09
{
    public partial class Companies : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DBConnection.EnsureDatabaseAndTables();
                BindCompaniesRepeater();
                txtDriveDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        private void BindCompaniesRepeater()
        {
            try
            {
                DataTable dt = DBConnection.ExecuteDataTable("SELECT CompanyId, CompanyName, Location, JobRole, Package, DriveDate FROM Companies ORDER BY Package DESC");
                rptCompanies.DataSource = dt;
                rptCompanies.DataBind();
            }
            catch (Exception ex)
            {
                ShowAlert("Error loading companies: " + ex.Message, "danger");
            }
        }

        protected void btnAddCompany_Click(object sender, EventArgs e)
        {
            try
            {
                string name = txtCompName.Text.Trim();
                string loc = txtLocation.Text.Trim();
                string role = txtRole.Text.Trim();
                string pkgStr = txtPackage.Text.Trim();
                string driveDateStr = txtDriveDate.Text.Trim();

                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(loc) || string.IsNullOrEmpty(role) || string.IsNullOrEmpty(pkgStr))
                {
                    ShowAlert("Please fill all required company fields.", "danger");
                    return;
                }

                decimal pkg = Convert.ToDecimal(pkgStr);
                DateTime dt;
                DateTime driveDate = DateTime.TryParse(driveDateStr, out dt) ? dt : DateTime.Now;

                string query = @"INSERT INTO Companies (CompanyName, Location, JobRole, Package, DriveDate) 
                                 VALUES (@CompanyName, @Location, @JobRole, @Package, @DriveDate)";

                int rows = DBConnection.ExecuteNonQuery(query,
                    new SqlParameter("@CompanyName", name),
                    new SqlParameter("@Location", loc),
                    new SqlParameter("@JobRole", role),
                    new SqlParameter("@Package", pkg),
                    new SqlParameter("@DriveDate", driveDate));

                ShowAlert("Company '" + name + "' added successfully!", "success");

                // Reset fields
                txtCompName.Text = "";
                txtLocation.Text = "";
                txtRole.Text = "";
                txtPackage.Text = "";

                BindCompaniesRepeater();
            }
            catch (Exception ex)
            {
                ShowAlert("Failed to add company: " + ex.Message, "danger");
            }
        }

        private void ShowAlert(string message, string alertType)
        {
            pnlAlert.Visible = true;
            pnlAlert.CssClass = "alert alert-" + alertType + " alert-dismissible fade show border-0 shadow-sm rounded-3 mb-4";
            litAlertMsg.Text = "<strong>Notice:</strong> " + message;
        }
    }
}
