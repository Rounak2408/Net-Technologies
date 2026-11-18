using System;
using System.Data;
using System.Web.UI;
using Experiment_09.Data;

namespace Experiment_09
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DBConnection.EnsureDatabaseAndTables();
                LoadDashboardStatistics();
                LoadPlacedStudentsDataList();
            }
        }

        private void LoadDashboardStatistics()
        {
            try
            {
                // Total Students
                object objTotal = DBConnection.ExecuteScalar("SELECT COUNT(*) FROM Students");
                int totalStudents = objTotal != null ? Convert.ToInt32(objTotal) : 0;
                lblTotalStudents.Text = totalStudents.ToString();

                // Total Companies
                object objCompanies = DBConnection.ExecuteScalar("SELECT COUNT(*) FROM Companies");
                int totalCompanies = objCompanies != null ? Convert.ToInt32(objCompanies) : 0;
                lblTotalCompanies.Text = totalCompanies.ToString();

                // Students Placed
                object objPlaced = DBConnection.ExecuteScalar("SELECT COUNT(*) FROM Students WHERE PlacementStatus = 'Placed'");
                int placedStudents = objPlaced != null ? Convert.ToInt32(objPlaced) : 0;
                lblStudentsPlaced.Text = placedStudents.ToString();

                // Students Not Placed
                object objNotPlaced = DBConnection.ExecuteScalar("SELECT COUNT(*) FROM Students WHERE PlacementStatus = 'Not Placed'");
                int notPlacedStudents = objNotPlaced != null ? Convert.ToInt32(objNotPlaced) : 0;
                lblStudentsNotPlaced.Text = notPlacedStudents.ToString();

                // Placement Percentage
                double placementPercentage = totalStudents > 0 ? ((double)placedStudents / totalStudents) * 100.0 : 0.0;
                lblPlacementPercentage.Text = placementPercentage.ToString("0.0") + "%";
                litProgressBar.Text = string.Format("<div class=\"progress-bar bg-navy\" role=\"progressbar\" style=\"width: {0}%;\" aria-valuenow=\"{0}\" aria-valuemin=\"0\" aria-valuemax=\"100\"></div>", (int)placementPercentage);

                // Highest Package Offered to Placed Students
                object objMaxPackage = DBConnection.ExecuteScalar(@"SELECT ISNULL(MAX(c.Package), 0.0) 
                                                                    FROM Students s 
                                                                    INNER JOIN Companies c ON s.CompanyId = c.CompanyId 
                                                                    WHERE s.PlacementStatus = 'Placed'");
                decimal maxPackage = objMaxPackage != null ? Convert.ToDecimal(objMaxPackage) : 0.0m;
                lblHighestPackage.Text = string.Format("₹{0:N2} LPA", maxPackage);

                // Average Package Offered to Placed Students
                object objAvgPackage = DBConnection.ExecuteScalar(@"SELECT ISNULL(AVG(c.Package), 0.0) 
                                                                    FROM Students s 
                                                                    INNER JOIN Companies c ON s.CompanyId = c.CompanyId 
                                                                    WHERE s.PlacementStatus = 'Placed'");
                decimal avgPackage = objAvgPackage != null ? Convert.ToDecimal(objAvgPackage) : 0.0m;
                lblAveragePackage.Text = string.Format("₹{0:N2} LPA", avgPackage);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error loading dashboard stats: " + ex.Message);
            }
        }

        private void LoadPlacedStudentsDataList()
        {
            try
            {
                string query = @"SELECT TOP 6 s.StudentId, s.FullName, s.Email, s.Department, s.CGPA, s.GraduationYear, 
                                        c.CompanyName, c.Package 
                                 FROM Students s 
                                 INNER JOIN Companies c ON s.CompanyId = c.CompanyId 
                                 WHERE s.PlacementStatus = 'Placed' 
                                 ORDER BY c.Package DESC";

                DataTable dt = DBConnection.ExecuteDataTable(query);
                dlPlacedStudents.DataSource = dt;
                dlPlacedStudents.DataBind();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error loading DataList: " + ex.Message);
            }
        }
    }
}
