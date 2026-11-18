-- ============================================================================
-- Script: Create_PlacementDB.sql
-- Description: Placement Management Database and Table Creation Script
-- Project: Exercise 9 – ASP.NET Web Forms using Data Controls
-- ============================================================================

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'PlacementDB')
BEGIN
    CREATE DATABASE PlacementDB;
    PRINT 'Database PlacementDB created successfully.';
END
ELSE
BEGIN
    PRINT 'Database PlacementDB already exists.';
END
GO

USE PlacementDB;
GO

-- 1. Create Companies Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = N'Companies' AND type = 'U')
BEGIN
    CREATE TABLE Companies (
        CompanyId INT IDENTITY(1,1) PRIMARY KEY,
        CompanyName NVARCHAR(150) NOT NULL,
        Location NVARCHAR(100) NOT NULL,
        JobRole NVARCHAR(100) NOT NULL,
        Package DECIMAL(10,2) NOT NULL CHECK (Package > 0), -- Package in LPA (Lakhs Per Annum)
        DriveDate DATE NOT NULL DEFAULT GETDATE()
    );
    PRINT 'Table Companies created successfully.';
END
GO

-- 2. Create Students Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = N'Students' AND type = 'U')
BEGIN
    CREATE TABLE Students (
        StudentId INT IDENTITY(1,1) PRIMARY KEY,
        FullName NVARCHAR(100) NOT NULL,
        Email NVARCHAR(100) NOT NULL,
        Department NVARCHAR(100) NOT NULL,
        CGPA DECIMAL(3,2) NOT NULL CHECK (CGPA >= 0.00 AND CGPA <= 10.00),
        GraduationYear INT NOT NULL,
        PlacementStatus NVARCHAR(30) NOT NULL CHECK (PlacementStatus IN ('Placed', 'Not Placed')),
        CompanyId INT NULL FOREIGN KEY REFERENCES Companies(CompanyId) ON DELETE SET NULL
    );
    PRINT 'Table Students created successfully.';
END
GO

-- 3. Seed Companies Data
IF NOT EXISTS (SELECT 1 FROM Companies)
BEGIN
    INSERT INTO Companies (CompanyName, Location, JobRole, Package, DriveDate)
    VALUES
    (N'Google India', N'Bangalore', N'Cloud Solutions Engineer', 48.00, '2026-09-15'),
    (N'Microsoft IDC', N'Hyderabad', N'Software Development Engineer', 44.50, '2026-09-22'),
    (N'Amazon AWS', N'Hyderabad', N'Associate SDE-1', 32.00, '2026-10-05'),
    (N'Cisco Systems', N'Bangalore', N'Network Security Specialist', 22.50, '2026-10-18'),
    (N'Deloitte USI', N'Gurugram', N'Technology Consultant', 12.00, '2026-11-02'),
    (N'Infosys Limited', N'Bhubaneswar', N'Specialist Programmer', 9.50, '2026-11-10'),
    (N'Tata Consultancy Services', N'Pune', N'Digital Systems Engineer', 7.50, '2026-11-15');

    PRINT 'Companies seed data inserted successfully.';
END
GO

-- 4. Seed Students Data
IF NOT EXISTS (SELECT 1 FROM Students)
BEGIN
    INSERT INTO Students (FullName, Email, Department, CGPA, GraduationYear, PlacementStatus, CompanyId)
    VALUES
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

    PRINT 'Students seed data inserted successfully.';
END
GO
