using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Experiment_10.Models;

namespace Experiment_10.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly string _connectionString;
        private readonly string _masterConnectionString;

        public StudentRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

            var builder = new SqlConnectionStringBuilder(_connectionString)
            {
                InitialCatalog = "master"
            };
            _masterConnectionString = builder.ConnectionString;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<IEnumerable<Student>> GetAllAsync(string? search = null, string? department = null)
        {
            var students = new List<Student>();
            string query = @"SELECT StudentId, FullName, Email, Phone, Department, Semester, CGPA, CreatedAt 
                             FROM Students 
                             WHERE 1=1";

            if (!string.IsNullOrWhiteSpace(search))
            {
                query += " AND (FullName LIKE @Search OR Email LIKE @Search)";
            }

            if (!string.IsNullOrWhiteSpace(department) && department != "All")
            {
                query += " AND Department = @Department";
            }

            query += " ORDER BY StudentId DESC";

            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(query, conn))
            {
                if (!string.IsNullOrWhiteSpace(search))
                {
                    cmd.Parameters.Add(new SqlParameter("@Search", SqlDbType.NVarChar) { Value = $"%{search.Trim()}%" });
                }

                if (!string.IsNullOrWhiteSpace(department) && department != "All")
                {
                    cmd.Parameters.Add(new SqlParameter("@Department", SqlDbType.NVarChar) { Value = department.Trim() });
                }

                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        students.Add(MapReaderToStudent(reader));
                    }
                }
            }

            return students;
        }

        public async Task<Student?> GetByIdAsync(int id)
        {
            string query = @"SELECT StudentId, FullName, Email, Phone, Department, Semester, CGPA, CreatedAt 
                             FROM Students 
                             WHERE StudentId = @StudentId";

            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add(new SqlParameter("@StudentId", SqlDbType.Int) { Value = id });

                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return MapReaderToStudent(reader);
                    }
                }
            }

            return null;
        }

        public async Task<Student?> GetByEmailAsync(string email)
        {
            string query = @"SELECT StudentId, FullName, Email, Phone, Department, Semester, CGPA, CreatedAt 
                             FROM Students 
                             WHERE Email = @Email";

            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 100) { Value = email.Trim() });

                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return MapReaderToStudent(reader);
                    }
                }
            }

            return null;
        }

        public async Task<int> AddAsync(Student student)
        {
            string query = @"INSERT INTO Students (FullName, Email, Phone, Department, Semester, CGPA, CreatedAt)
                             VALUES (@FullName, @Email, @Phone, @Department, @Semester, @CGPA, @CreatedAt);
                             SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add(new SqlParameter("@FullName", SqlDbType.NVarChar, 100) { Value = student.FullName });
                cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 100) { Value = student.Email });
                cmd.Parameters.Add(new SqlParameter("@Phone", SqlDbType.NVarChar, 15) { Value = (object?)student.Phone ?? DBNull.Value });
                cmd.Parameters.Add(new SqlParameter("@Department", SqlDbType.NVarChar, 100) { Value = student.Department });
                cmd.Parameters.Add(new SqlParameter("@Semester", SqlDbType.Int) { Value = student.Semester });
                cmd.Parameters.Add(new SqlParameter("@CGPA", SqlDbType.Decimal) { Value = student.CGPA, Precision = 3, Scale = 2 });
                cmd.Parameters.Add(new SqlParameter("@CreatedAt", SqlDbType.DateTime) { Value = DateTime.Now });

                await conn.OpenAsync();
                object? result = await cmd.ExecuteScalarAsync();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        public async Task<bool> UpdateAsync(Student student)
        {
            string query = @"UPDATE Students 
                             SET FullName = @FullName, 
                                 Email = @Email, 
                                 Phone = @Phone, 
                                 Department = @Department, 
                                 Semester = @Semester, 
                                 CGPA = @CGPA 
                             WHERE StudentId = @StudentId";

            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add(new SqlParameter("@StudentId", SqlDbType.Int) { Value = student.StudentId });
                cmd.Parameters.Add(new SqlParameter("@FullName", SqlDbType.NVarChar, 100) { Value = student.FullName });
                cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 100) { Value = student.Email });
                cmd.Parameters.Add(new SqlParameter("@Phone", SqlDbType.NVarChar, 15) { Value = (object?)student.Phone ?? DBNull.Value });
                cmd.Parameters.Add(new SqlParameter("@Department", SqlDbType.NVarChar, 100) { Value = student.Department });
                cmd.Parameters.Add(new SqlParameter("@Semester", SqlDbType.Int) { Value = student.Semester });
                cmd.Parameters.Add(new SqlParameter("@CGPA", SqlDbType.Decimal) { Value = student.CGPA, Precision = 3, Scale = 2 });

                await conn.OpenAsync();
                int rows = await cmd.ExecuteNonQueryAsync();
                return rows > 0;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            string query = "DELETE FROM Students WHERE StudentId = @StudentId";

            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add(new SqlParameter("@StudentId", SqlDbType.Int) { Value = id });

                await conn.OpenAsync();
                int rows = await cmd.ExecuteNonQueryAsync();
                return rows > 0;
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            string query = "SELECT COUNT(1) FROM Students WHERE StudentId = @StudentId";

            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add(new SqlParameter("@StudentId", SqlDbType.Int) { Value = id });

                await conn.OpenAsync();
                object? result = await cmd.ExecuteScalarAsync();
                return result != null && Convert.ToInt32(result) > 0;
            }
        }

        public void EnsureDatabaseSetup()
        {
            try
            {
                using (var masterConn = new SqlConnection(_masterConnectionString))
                {
                    masterConn.Open();
                    string createDbSql = @"IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'StudentManagementDB')
                                           BEGIN
                                               CREATE DATABASE StudentManagementDB;
                                           END";
                    using (var cmd = new SqlCommand(createDbSql, masterConn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                using (var conn = GetConnection())
                {
                    conn.Open();
                    string checkTableSql = @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = N'Students' AND type = 'U')
                    BEGIN
                        CREATE TABLE Students (
                            StudentId INT IDENTITY(1,1) PRIMARY KEY,
                            FullName NVARCHAR(100) NOT NULL,
                            Email NVARCHAR(100) NOT NULL CONSTRAINT UQ_Students_Email UNIQUE,
                            Phone NVARCHAR(15) NULL,
                            Department NVARCHAR(100) NOT NULL,
                            Semester INT NOT NULL CHECK (Semester >= 1 AND Semester <= 8),
                            CGPA DECIMAL(3,2) NOT NULL CHECK (CGPA >= 0.00 AND CGPA <= 10.00),
                            CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
                        );

                        INSERT INTO Students (FullName, Email, Phone, Department, Semester, CGPA, CreatedAt)
                        VALUES
                        (N'Aarav Sharma', N'aarav.sharma@example.edu', N'9876543210', N'Computer Science', 6, 9.45, DATEADD(DAY, -20, GETDATE())),
                        (N'Ananya Verma', N'ananya.v@example.edu', N'9812345678', N'Computer Science', 6, 9.20, DATEADD(DAY, -18, GETDATE())),
                        (N'Rohan Das', N'rohan.das@example.edu', N'9765432109', N'Information Technology', 4, 8.85, DATEADD(DAY, -15, GETDATE())),
                        (N'Sneha Kulkarni', N'sneha.k@example.edu', N'9988776655', N'Electronics & Communication', 8, 8.70, DATEADD(DAY, -12, GETDATE())),
                        (N'Priya Patel', N'priya.patel@example.edu', N'9871234560', N'Data Science', 4, 8.95, DATEADD(DAY, -10, GETDATE())),
                        (N'Vikram Aditya', N'vikram.a@example.edu', N'9823456781', N'Computer Science', 6, 8.40, DATEADD(DAY, -5, GETDATE())),
                        (N'Neha Gupta', N'neha.gupta@example.edu', N'9834567892', N'Information Technology', 2, 8.10, DATEADD(DAY, -2, GETDATE()));
                    END";

                    using (var cmd = new SqlCommand(checkTableSql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Database Auto-Setup] Notice: {ex.Message}");
            }
        }

        private static Student MapReaderToStudent(SqlDataReader reader)
        {
            return new Student
            {
                StudentId = reader.GetInt32(reader.GetOrdinal("StudentId")),
                FullName = reader.GetString(reader.GetOrdinal("FullName")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString(reader.GetOrdinal("Phone")),
                Department = reader.GetString(reader.GetOrdinal("Department")),
                Semester = reader.GetInt32(reader.GetOrdinal("Semester")),
                CGPA = reader.GetDecimal(reader.GetOrdinal("CGPA")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
            };
        }
    }
}
