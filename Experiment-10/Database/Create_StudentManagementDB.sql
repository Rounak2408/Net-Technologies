-- ============================================================================
-- Script: Create_StudentManagementDB.sql
-- Description: Student Management REST API Database & Table Creation Script
-- Project: Exercise 10 – ASP.NET Core Web API
-- ============================================================================

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'StudentManagementDB')
BEGIN
    CREATE DATABASE StudentManagementDB;
    PRINT 'Database StudentManagementDB created successfully.';
END
ELSE
BEGIN
    PRINT 'Database StudentManagementDB already exists.';
END
GO

USE StudentManagementDB;
GO

-- 1. Create Students Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = N'Students' AND type = 'U')
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
    PRINT 'Table Students created successfully.';
END
GO

-- 2. Seed Initial Student Records
IF NOT EXISTS (SELECT 1 FROM Students)
BEGIN
    INSERT INTO Students (FullName, Email, Phone, Department, Semester, CGPA, CreatedAt)
    VALUES
    (N'Aarav Sharma', N'aarav.sharma@example.edu', N'9876543210', N'Computer Science', 6, 9.45, DATEADD(DAY, -20, GETDATE())),
    (N'Ananya Verma', N'ananya.v@example.edu', N'9812345678', N'Computer Science', 6, 9.20, DATEADD(DAY, -18, GETDATE())),
    (N'Rohan Das', N'rohan.das@example.edu', N'9765432109', N'Information Technology', 4, 8.85, DATEADD(DAY, -15, GETDATE())),
    (N'Sneha Kulkarni', N'sneha.k@example.edu', N'9988776655', N'Electronics & Communication', 8, 8.70, DATEADD(DAY, -12, GETDATE())),
    (N'Priya Patel', N'priya.patel@example.edu', N'9871234560', N'Data Science', 4, 8.95, DATEADD(DAY, -10, GETDATE())),
    (N'Vikram Aditya', N'vikram.a@example.edu', N'9823456781', N'Computer Science', 6, 8.40, DATEADD(DAY, -5, GETDATE())),
    (N'Neha Gupta', N'neha.gupta@example.edu', N'9834567892', N'Information Technology', 2, 8.10, DATEADD(DAY, -2, GETDATE()));

    PRINT 'Sample student records seeded successfully.';
END
GO
