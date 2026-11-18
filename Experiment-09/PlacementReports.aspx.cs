using System;
using System.Data;
using System.Web.UI;
using Experiment_09.Data;

namespace Experiment_09
{
    public partial class PlacementReports : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DBConnection.EnsureDatabaseAndTables();
                LoadDepartmentReports();
                LoadTierBreakdown();
                LoadCompanyRecruitmentSummary();
            }
        }

        private void LoadDepartmentReports()
        {
            try
            {
                string query = @"SELECT Department, 
                                        COUNT(*) as TotalStudents, 
                                        SUM(CASE WHEN PlacementStatus = 'Placed' THEN 1 ELSE 0 END) as PlacedCount, 
                                        SUM(CASE WHEN PlacementStatus = 'Not Placed' THEN 1 ELSE 0 END) as UnplacedCount, 
                                        CAST(ROUND(CAST(SUM(CASE WHEN PlacementStatus = 'Placed' THEN 1 ELSE 0 END) AS FLOAT) * 100.0 / COUNT(*), 1) as DECIMAL(5,1)) as PlacementRate 
                                 FROM Students 
                                 GROUP BY Department 
                                 ORDER BY PlacementRate DESC, TotalStudents DESC";

                DataTable dt = DBConnection.ExecuteDataTable(query);
                gvDeptReport.DataSource = dt;
                gvDeptReport.DataBind();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error in department report: " + ex.Message);
            }
        }

        private void LoadTierBreakdown()
        {
            try
            {
                // Tier 1 (> 20 LPA)
                object t1 = DBConnection.ExecuteScalar(@"SELECT COUNT(*) FROM Students s 
                                                         INNER JOIN Companies c ON s.CompanyId = c.CompanyId 
                                                         WHERE s.PlacementStatus = 'Placed' AND c.Package >= 20.00");
                lblTier1Count.Text = t1 != null ? t1.ToString() : "0";

                // Tier 2 (10 - 20 LPA)
                object t2 = DBConnection.ExecuteScalar(@"SELECT COUNT(*) FROM Students s 
                                                         INNER JOIN Companies c ON s.CompanyId = c.CompanyId 
                                                         WHERE s.PlacementStatus = 'Placed' AND c.Package >= 10.00 AND c.Package < 20.00");
                lblTier2Count.Text = t2 != null ? t2.ToString() : "0";

                // Tier 3 (< 10 LPA)
                object t3 = DBConnection.ExecuteScalar(@"SELECT COUNT(*) FROM Students s 
                                                         INNER JOIN Companies c ON s.CompanyId = c.CompanyId 
                                                         WHERE s.PlacementStatus = 'Placed' AND c.Package < 10.00");
                lblTier3Count.Text = t3 != null ? t3.ToString() : "0";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error in tier breakdown: " + ex.Message);
            }
        }

        private void LoadCompanyRecruitmentSummary()
        {
            try
            {
                string query = @"SELECT c.CompanyName, c.Package, 
                                        COUNT(s.StudentId) as HiredCount 
                                 FROM Companies c 
                                 LEFT JOIN Students s ON c.CompanyId = s.CompanyId AND s.PlacementStatus = 'Placed' 
                                 GROUP BY c.CompanyName, c.Package 
                                 ORDER BY HiredCount DESC, c.Package DESC";

                DataTable dt = DBConnection.ExecuteDataTable(query);
                gvCompanyHiring.DataSource = dt;
                gvCompanyHiring.DataBind();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error in company hiring summary: " + ex.Message);
            }
        }
    }
}
