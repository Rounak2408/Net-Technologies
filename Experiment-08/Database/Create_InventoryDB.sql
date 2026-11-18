-- ============================================================================
-- Script: Create_InventoryDB.sql
-- Description: Database and Table Creation Script with Seed Data
-- Project: Exercise 8 - Inventory Management System (ASP.NET Core MVC & ADO.NET)
-- ============================================================================

-- 1. Create Database if it does not already exist
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'InventoryDB')
BEGIN
    CREATE DATABASE InventoryDB;
    PRINT 'Database InventoryDB created successfully.';
END
ELSE
BEGIN
    PRINT 'Database InventoryDB already exists.';
END
GO

USE InventoryDB;
GO

-- 2. Create Products Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = N'Products' AND type = 'U')
BEGIN
    CREATE TABLE Products (
        ProductId INT IDENTITY(1,1) PRIMARY KEY,
        ProductName NVARCHAR(100) NOT NULL,
        Category NVARCHAR(50) NOT NULL,
        Description NVARCHAR(500) NULL,
        Quantity INT NOT NULL CHECK (Quantity >= 0),
        UnitPrice DECIMAL(10,2) NOT NULL CHECK (UnitPrice > 0),
        SupplierName NVARCHAR(100) NOT NULL,
        CreatedDate DATETIME NOT NULL DEFAULT GETDATE()
    );
    PRINT 'Table Products created successfully.';
END
ELSE
BEGIN
    PRINT 'Table Products already exists.';
END
GO

-- 3. Seed Realistic Sample Inventory Data
IF NOT EXISTS (SELECT 1 FROM Products)
BEGIN
    INSERT INTO Products (ProductName, Category, Description, Quantity, UnitPrice, SupplierName, CreatedDate)
    VALUES
    (N'Dell Latitude 5420 Laptop', N'Electronics', N'11th Gen Intel Core i5, 16GB RAM, 512GB SSD, 14-inch FHD Display', 25, 749.99, N'Dell Global Logistics', DATEADD(DAY, -15, GETDATE())),
    (N'Logitech MX Master 3S Mouse', N'Accessories', N'Advanced wireless mouse with 8K DPI track-on-glass sensor and Quiet Clicks', 42, 99.50, N'LogiTech Distribution India', DATEADD(DAY, -12, GETDATE())),
    (N'Ergonomic Mesh Office Chair', N'Furniture', N'High-back executive chair with 3D armrests and dynamic lumbar support', 18, 189.00, N'Urban Office Solutions', DATEADD(DAY, -10, GETDATE())),
    (N'Samsung 27-inch 4K UHD Monitor', N'Electronics', N'IPS display panel with HDR10, USB-C 65W charging and height adjustable stand', 8, 329.99, N'Samsung Electronics Ltd', DATEADD(DAY, -8, GETDATE())),
    (N'Cisco 24-Port Gigabit Switch', N'Networking', N'Smart managed switch with Gigabit Ethernet ports and basic Layer 3 routing', 4, 245.00, N'Cisco Systems Partner Hub', DATEADD(DAY, -5, GETDATE())),
    (N'HP LaserJet Pro MFP 3101fdw', N'Office Supplies', N'Monochrome multifunction laser printer with automatic duplex and wireless printing', 6, 289.50, N'HP Direct Enterprise', DATEADD(DAY, -4, GETDATE())),
    (N'Mechanical Gaming Keyboard', N'Accessories', N'RGB backlit mechanical keyboard with tactile blue switches and detachable cable', 35, 69.99, N'Redragon Tech Gear', DATEADD(DAY, -2, GETDATE())),
    (N'Cat6 Ethernet Cable (100m Roll)', N'Networking', N'High performance unshielded twisted pair solid copper network bulk cable', 50, 45.00, N'D-Link Connect Direct', DATEADD(DAY, -1, GETDATE())),
    (N'Steelcase Dual Monitor Arm', N'Furniture', N'Heavy-duty gas spring dual monitor mount for desks up to 32-inch screens', 3, 129.99, N'Urban Office Solutions', GETDATE());

    PRINT 'Seed inventory data inserted successfully.';
END
GO
