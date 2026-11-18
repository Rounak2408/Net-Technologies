using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Experiment_09.Data
{
    public static class DBConnection
    {
        public static string GetConnectionString()
        {
            var connSettings = ConfigurationManager.ConnectionStrings["PlacementDBConnection"];
            if (connSettings != null && !string.IsNullOrEmpty(connSettings.ConnectionString))
            {
                return connSettings.ConnectionString;
            }
            return @"Server=(localdb)\MSSQLLocalDB;Database=PlacementDB;Integrated Security=True;TrustServerCertificate=True";
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(GetConnectionString());
        }

        public static DataTable ExecuteDataTable(string query, params SqlParameter[] parameters)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                if (parameters != null && parameters.Length > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
            return dt;
        }

        public static int ExecuteNonQuery(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                if (parameters != null && parameters.Length > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public static object ExecuteScalar(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                if (parameters != null && parameters.Length > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                conn.Open();
                return cmd.ExecuteScalar();
            }
        }

        public static void EnsureDatabaseAndTables()
        {
            try
            {
                // Connect to master to verify PlacementDB
                var masterBuilder = new SqlConnectionStringBuilder(GetConnectionString())
                {
                    InitialCatalog = "master"
                };

                using (var conn = new SqlConnection(masterBuilder.ConnectionString))
                {
                    conn.Open();
                    string createDbSql = @"IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'PlacementDB')
                                           BEGIN
                                               CREATE DATABASE PlacementDB;
                                           END";
                    using (var cmd = new SqlCommand(createDbSql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                // Connect to PlacementDB and ensure tables + seed data
                using (var conn = GetConnection())
                {
                    conn.Open();
                    string checkTableSql = @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = N'Companies' AND type = 'U')
                    BEGIN
                        CREATE TABLE Companies (
                            CompanyId INT IDENTITY(1,1) PRIMARY KEY,
                            CompanyName NVARCHAR(150) NOT NULL,
                            Location NVARCHAR(100) NOT NULL,
                            JobRole NVARCHAR(100) NOT NULL,
                            Package DECIMAL(10,2) NOT NULL,
                            DriveDate DATE NOT NULL DEFAULT GETDATE()
                        );

                        INSERT INTO Companies (CompanyName, Location, JobRole, Package, DriveDate) VALUES
                        (N'Google India', N'Bangalore', N'Cloud Solutions Engineer', 48.00, '2026-09-15'),
                        (N'Microsoft IDC', N'Hyderabad', N'Software Development Engineer', 44.50, '2026-09-22'),
                        (N'Amazon AWS', N'Hyderabad', N'Associate SDE-1', 32.00, '2026-10-05'),
                        (N'Cisco Systems', N'Bangalore', N'Network Security Specialist', 22.50, '2026-10-18'),
                        (N'Deloitte USI', N'Gurugram', N'Technology Consultant', 12.00, '2026-11-02'),
                        (N'Infosys Limited', N'Bhubaneswar', N'Specialist Programmer', 9.50, '2026-11-10'),
                        (N'Tata Consultancy Services', N'Pune', N'Digital Systems Engineer', 7.50, '2026-11-15');
                    END

                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = N'Students' AND type = 'U')
                    BEGIN
                        CREATE TABLE Students (
                            StudentId INT IDENTITY(1,1) PRIMARY KEY,
                            FullName NVARCHAR(100) NOT NULL,
                            Email NVARCHAR(100) NOT NULL,
                            Department NVARCHAR(100) NOT NULL,
                            CGPA DECIMAL(3,2) NOT NULL,
                            GraduationYear INT NOT NULL,
                            PlacementStatus NVARCHAR(30) NOT NULL,
                            CompanyId INT NULL FOREIGN KEY REFERENCES Companies(CompanyId) ON DELETE SET NULL
                        );

                        INSERT INTO Students (FullName, Email, Department, CGPA, GraduationYear, PlacementStatus, CompanyId) VALUES
                        (N'Aarav Sharma', N'aarav.sharma@college.edu', N'Computer Science', 9.45, 2026, N'Placed', 1),
                        (N'Ananya Verma', N'ananya.v@college.edu', N'Computer Science', 9.20, 2026, N'Placed', 2),
                        (N'Rohan Das', N'rohan.das@college.edu', N'Information Technology', 8.85, 2026, N'Placed', 3),
                        (N'Sneha Kulkarni', N'sneha.k@college.edu', N'Electronics & Communication', 8.70, 2026, N'Placed', 4),
                        (N'Priya Patel', N'priya.patel@college.edu', N'Data Science', 8.95, 2026, N'Placed', 5),
                        (N'Vikram Aditya', N'vikram.a@college.edu', N'Computer Science', 8.40, 2026, N'Placed', 6),
                        (N'Neha Gupta', N'neha.gupta@college.edu', N'Information Technology', 8.10, 2026, N'Placed', 7),
                        (N'Aditya Roy', N'aditya.roy@college.edu', N'Computer Science', 7.90, 2026, N'Not Placed', NULL),
                        (N'Kavita Joshi', N'kavita.j@college.edu', N'Electronics & Communication', 7.65, 2026, N'Not Placed', NULL),
                        (N'Rahul Nair', N'rahul.nair@college.edu', N'Mechanical Engineering', 7.50, 2026, N'Not Placed', NULL),
                        (N'Ishaan Sen', N'ishaan.sen@college.edu', N'Data Science', 8.15, 2026, N'Not Placed', NULL),
                        (N'Tanvi Mishra', N'tanvi.m@college.edu', N'Information Technology', 7.80, 2026, N'Not Placed', NULL);
                    END";

                    using (var cmd = new SqlCommand(checkTableSql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Database auto-init exception: " + ex.Message);
            }
        }
    }
}
